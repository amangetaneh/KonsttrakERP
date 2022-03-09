using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public Site Site { get; set; }

        public Item Item { get; set; }

        public string Reason { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }
        
        public DateTime? ApproveDate { get; set; }

        public Employee ApprovedBy { get; set; }

        public int ApprovedById { get; set; }
    }
}
