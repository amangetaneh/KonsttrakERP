using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        public string FName { get; set; } = string.Empty;

        public string MName { get; set; } = string.Empty;

        public string LName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

    }
}
