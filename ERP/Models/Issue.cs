using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class Issue
    {
        [Key]
        public int IssueId { get; set; }

        public int Status { get; set; } = 0;
        /*Status Values: 
           0: Issued
           1: Approved
           2: Handed
       */

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime? ApproveDate { get; set; }

        public DateTime? HandDate { get; set; }
     
        private Store Store { get; set; }

        private int StoreId { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Employee HandedBy { get; set; }

        public int? HandedById { get; set; }

        public Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        public ICollection<IssueItem> IssueItems { get; set; }
    }
}