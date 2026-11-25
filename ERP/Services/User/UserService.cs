using ERP.Context;
using ERP.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERP.Services.User
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UserRole? _userRole;
        public UserRole UserRole => _userRole ?? throw new UnauthorizedAccessException("An assigned role is required.");
        private readonly Employee? _employee;
        public Employee Employee => _employee ?? throw new UnauthorizedAccessException("An employee account is required.");


        public UserService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            var UserAccount = context.UserAccounts
                .Where(u => u.Username == GetMyName())
                .Include(u => u.Employee)
                .ThenInclude(e => e.UserRole)
                .Include(u => u.Employee)
                .ThenInclude(e => e.EmployeeSite)
                .FirstOrDefault();

            _employee = UserAccount?.Employee;
            _userRole = _employee?.UserRole;
        }

        public string GetMyName()
        {
            var result = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
            {

                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            }

            return result;
        }

        public int GetMyId()
        {
            var result = 0;

            if (_httpContextAccessor.HttpContext != null)
            {
                var employeeId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);

                result = Convert.ToInt32(employeeId);

            }

            return result;
        }

        public int GetMySiteId()
        {
            return Employee.EmployeeSiteId ?? throw new InvalidOperationException("Assign the employee to a site before performing this operation.");
        }
    }
}
