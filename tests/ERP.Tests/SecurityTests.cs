using System.Text.Json;
using ERP.Configuration;
using ERP.Models;
using ERP.Security;
using Microsoft.Extensions.Configuration;

namespace ERP.Tests
{
    public class SecurityTests
    {
        [Fact]
        public void PasswordsUseRandomSaltsAndRejectIncorrectValues()
        {
            PasswordHashing.Create("Synthetic-test-password!", out var hash, out var salt);
            PasswordHashing.Create("Synthetic-test-password!", out var secondHash, out var secondSalt);
            Assert.True(PasswordHashing.Verify("Synthetic-test-password!", hash, salt));
            Assert.False(PasswordHashing.Verify("Incorrect-test-password!", hash, salt));
            Assert.NotEqual(salt, secondSalt);
            Assert.NotEqual(hash, secondHash);
        }

        [Fact]
        public void PasswordHashesAndSaltsAreNeverSerialized()
        {
            var json = JsonSerializer.Serialize(new UserAccount { Username = "demo-user", PasswordHash = [1, 2], PasswordSalt = [3, 4] });
            Assert.DoesNotContain("PasswordHash", json);
            Assert.DoesNotContain("PasswordSalt", json);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("short")]
        [InlineData("CHANGE_ME_GENERATE_A_RANDOM_SIGNING_KEY")]
        public void RejectsMissingWeakOrPlaceholderSigningKeys(string? token)
        {
            Assert.Throws<InvalidOperationException>(() => RuntimeSettings.FromConfiguration(Config(token)));
        }

        [Fact]
        public void AcceptsConfiguredKeyAndRejectsWildcardCorsOrPlaceholderDatabase()
        {
            var key = new string('s', 64); // Synthetic test value, never used by the application.
            Assert.Equal(key, RuntimeSettings.FromConfiguration(Config(key)).SigningKey);
            Assert.Throws<InvalidOperationException>(() => RuntimeSettings.FromConfiguration(Config(key, "*")));
            Assert.Throws<InvalidOperationException>(() => RuntimeSettings.FromConfiguration(Config(key, "http://localhost:5173", "CHANGE_ME")));
        }

        private static IConfiguration Config(string? token, string origins = "http://localhost:5173", string connection = "Server=localhost;Database=synthetic") =>
            new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AppSettings:Token"] = token,
                ["Cors:AllowedOrigins"] = origins,
                ["ConnectionStrings:DefaultConnection"] = connection
            }).Build();
    }
}
