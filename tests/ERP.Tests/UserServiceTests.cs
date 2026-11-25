using System.Security.Claims;
using ERP.Models;
using ERP.Services.User;
using Microsoft.AspNetCore.Http;

namespace ERP.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task MissingEmployeeSiteHasAnExplicitError()
        {
            using var context = TestContext.Create();
            context.UserAccounts.Add(new UserAccount
            {
                Username = "demo-user",
                Employee = new Employee { UserRole = new UserRole { Role = "Demo Role" } }
            });
            await context.SaveChangesAsync();
            var accessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, "demo-user")], "synthetic-test"))
                }
            };
            var exception = Assert.Throws<InvalidOperationException>(() => new UserService(context, accessor).GetMySiteId());
            Assert.Contains("Assign the employee to a site", exception.Message);
        }

        [Fact]
        public async Task SiteLookupUsesTheEmployeeSiteRatherThanTheEmployeeClaim()
        {
            using var context = TestContext.Create();
            var role = new UserRole { RoleId = 1, Role = "Demo role" };
            var employee = new Employee { EmployeeId = 9, EmployeeSiteId = 7, UserRoleId = 1, UserRole = role };
            context.UserAccounts.Add(new UserAccount { Username = "demo-user", Employee = employee, EmployeeId = 9 });
            await context.SaveChangesAsync();
            var accessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "demo-user"),
                        new Claim(ClaimTypes.Sid, "9")
                    }, "synthetic-test"))
                }
            };
            var service = new UserService(context, accessor);
            Assert.Equal(9, service.GetMyId());
            Assert.Equal(7, service.GetMySiteId());
        }
    }
}
