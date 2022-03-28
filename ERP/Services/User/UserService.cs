using System.Security.Claims;

namespace ERP.Services.User
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetMyName()
        {
            var result = string.Empty;
            
            if (_httpContextAccessor.HttpContext != null)
            {

                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
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
            var result = 0;

            if (_httpContextAccessor.HttpContext != null)
            {
                var employeeId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);

                result = Convert.ToInt32(employeeId);

            }

            return result;
        }
    }
}
