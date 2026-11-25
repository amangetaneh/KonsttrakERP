using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class UserAccount
    {
        [Key]
        public string Username { get; set; } = string.Empty;

        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = [];

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = [];

        public Employee Employee { get; set; } = null!;

        public int? EmployeeId { get; set; }

    }
}
