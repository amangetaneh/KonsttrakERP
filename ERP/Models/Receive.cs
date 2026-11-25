using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class Receive
    {
        [Key]
        public int ReceiveId { get; set; }

        public int Status { get; set; }

        public DateTime? ReceiveDate { get; set; }

        public Purchase Purchase { get; set; } = null!;

        public int PurchaseId { get; set; }

        public Employee DeliveredBy { get; set; } = null!;

        public int? DeliveredById { get; set; }

        public Employee ReceivedBy { get; set; } = null!;

        public int? ReceivedById { get; set; }

        public DateTime? ApproveDate { get; set; }

        public Employee ApprovedBy { get; set; } = null!;

        public int? ApprovedById { get; set; }

        public string? ApproveRemark { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public Site ReceivingSite { get; set; } = null!;

        public int ReceivingSiteId { get; set; }

        public ICollection<ReceiveItem> ReceiveItems { get; set; } = [];
    }
}

