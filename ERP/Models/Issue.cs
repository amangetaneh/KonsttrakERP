using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;

        private Site Site { get; set; }

        private int SiteId { get; set; }  

        public Employee HandedBy { get; set; }

        public int HandedById { get; set; }

        public Employee ReceivedBy { get; set; }

        public int ReceivedById { get; set; }
        
        public Employee ApprovedBy { get; set; }

        public int ApprovedById { get; set; }

        public ICollection<IssueItem> IssueItems { get; set; }
    }
}