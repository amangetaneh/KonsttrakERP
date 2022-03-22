using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        public int Status { get; set; } = 0;
        /*Status Values: 
           0: Requested
           1: Approved
           2: Checked
       */

        public DateTime RequestDate { get; set; } = DateTime.Now;

        [JsonIgnore]
        public Store RequestStore { get; set; }

        public int RequestStoreId { get; set; }

        [JsonIgnore]
        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public DateTime? CheckDate { get; set; }

        [JsonIgnore]
        public Employee CheckedBy { get; set; }

        public int? CheckedById { get; set; }

        public DateTime? ApproveDate { get; set; }

        [JsonIgnore]
        public Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
