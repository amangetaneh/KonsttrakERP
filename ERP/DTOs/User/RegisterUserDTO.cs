using System.ComponentModel.DataAnnotations;
namespace ERP.DTOs.User
{
    public class RegisterUserDTO
    {
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        public int UserRoleId { get; set; }

        public string FName { get; set; } = string.Empty;

        public string MName { get; set; } = string.Empty;

        public string LName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public int EmployeeSiteId { get; set; }

    }
}
