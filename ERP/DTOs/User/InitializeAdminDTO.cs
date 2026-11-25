using System.ComponentModel.DataAnnotations;
namespace ERP.DTOs.User
{
    public class InitializeAdminDTO
    {
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

    }
}
