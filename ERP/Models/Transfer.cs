using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class Transfer
    {
        [Key]
        public int Id { get; set; }

        public int Status { get; set; } = TransferStatus.Requested;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime? SendDate { get; set; }

        public DateTime? ApproveDate { get; set; }

        public DateTime? ReceiveDate { get; set; }

        public Store SendStore { get; set; }

        public int SendStoreId { get; set; }
        
        public Store ReceiveStore { get; set; }

        public int ReceiveStoreId { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Employee ReceivedBy { get; set; }

        public int? ReceivedById { get; set; }

        public Employee DeliveredBy { get; set; }

        public int? DeliveredById { get; set; }

        public Employee SentBy { get; set; }

        public int? SentById { get; set; }

        public string? VehiclePlateNo { get; set; }

        public virtual Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        public ICollection<TransferItem> TransferItems { get; set; }
    }
}
