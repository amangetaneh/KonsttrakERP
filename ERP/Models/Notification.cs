using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public int Status { get; set; }

        public int ActionId { get; set; }

        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        public int SiteId { get; set; }

        public Site Site { get; set; } = null!;

        public bool IsCleared { get; set; } = false;

        public DateTime Date { get; set; } = DateTime.Now;


    }
}
