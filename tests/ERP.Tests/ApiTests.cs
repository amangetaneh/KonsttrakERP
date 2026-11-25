using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ERP.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ERP.Tests
{
    public class ApiTests
    {
        [Fact]
        public async Task BootstrapLoginAndProtectedEmployeeAccessWorkEndToEnd()
        {
            await using var factory = new ApiFactory();
            using var client = factory.CreateClient();
            Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/employee")).StatusCode);
            var before = await client.GetFromJsonAsync<JsonElement>("/api/employee/initialization-status");
            Assert.False(before.GetProperty("initialized").GetBoolean());
            var setup = await client.PostAsJsonAsync("/api/employee/initialize", new { password = "Synthetic-test-password!" });
            setup.EnsureSuccessStatusCode();
            var payload = await setup.Content.ReadAsStringAsync();
            Assert.DoesNotContain("password", payload, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(HttpStatusCode.NotFound, (await client.PostAsJsonAsync("/api/employee/initialize", new { password = "Synthetic-test-password!" })).StatusCode);
            var invalid = await client.PostAsJsonAsync("/api/authorization/login", new { username = "admin", password = "incorrect-password" });
            Assert.Equal(HttpStatusCode.BadRequest, invalid.StatusCode);
            var login = await client.PostAsJsonAsync("/api/authorization/login", new { username = "admin", password = "Synthetic-test-password!" });
            login.EnsureSuccessStatusCode();
            var data = await login.Content.ReadFromJsonAsync<JsonElement>();
            Assert.True(data.GetProperty("userRole").GetProperty("isAdmin").GetBoolean());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.GetProperty("token").GetString());
            Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/employee")).StatusCode);
        }

        [Fact]
        public async Task RejectsShortBootstrapPasswordsAndPrivateFileAccess()
        {
            await using var factory = new ApiFactory();
            using var client = factory.CreateClient();
            Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/api/employee/initialize", new { password = "short" })).StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/file/private-demo.pdf")).StatusCode);
            Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/file/blank_forms.pdf")).StatusCode);
        }

        [Fact]
        public async Task CorsAllowsOnlyTheConfiguredFrontendOrigin()
        {
            await using var factory = new ApiFactory();
            using var client = factory.CreateClient();
            foreach (var origin in new[] { "http://localhost:5173", "https://untrusted.example" })
            {
                using var request = new HttpRequestMessage(HttpMethod.Options, "/api/employee");
                request.Headers.Add("Origin", origin);
                request.Headers.Add("Access-Control-Request-Method", "GET");
                using var response = await client.SendAsync(request);
                Assert.Equal(origin == "http://localhost:5173", response.Headers.Contains("Access-Control-Allow-Origin"));
            }
        }
    }

    internal sealed class ApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=localhost;Database=synthetic-tests",
                ["AppSettings:Token"] = new string('t', 64),
                ["Cors:AllowedOrigins"] = "http://localhost:5173"
            }));
            var databaseName = Guid.NewGuid().ToString();
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<DbContextOptions<DataContext>>();
                services.RemoveAll<IDbContextOptionsConfiguration<DataContext>>();
                services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName));
            });
        }
    }
}
