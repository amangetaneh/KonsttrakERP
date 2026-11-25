using System.ComponentModel.DataAnnotations;
namespace ERP.DTOs.User
{
    public class LoginUserDTO
    {
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

    }
}
