using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class Receive
    {
        [Key]
        public int Id { get; set; }

        public int Status { get; set; } = 0;
        /*Status Values: 
           0: Received
           1: Approved
       */

        [Column(TypeName = "Date")]
        public DateTime ReceiveDate { get; set; } = DateTime.Now;
        
        [JsonIgnore]
        public Store ReceiveStore { get; set; }

        public int ReceiveStoreId { get; set; }

        public string DeliveredFrom { get; set; }

        [JsonIgnore]
        public Purchase Purchase { get; set; }

        public int PurchaseId { get; set; }

        [JsonIgnore]
        public Employee DeliveredBy { get; set; }

        public int DeliveredById { get; set; }

        [JsonIgnore]
        public Employee ReceivedBy { get; set; }

        public int ReceivedById { get; set; }

        public DateTime? ApproveDate { get; set; }
        
        [JsonIgnore]
        public Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        public ICollection<ReceiveItem> ReceiveItems { get; set; }
    }
}

