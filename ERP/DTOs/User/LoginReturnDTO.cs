using ERP.Models;

namespace ERP.DTOs.User
{
    public class LoginReturnDTO
    {
        public string token { get; set; } = string.Empty;

        public Employee employee { get; set; } = null!;

        public Models.UserRole userRole { get; set; } = null!;

        public string username { get; set; } = string.Empty;

    }
}
