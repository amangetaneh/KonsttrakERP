using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class Transfer
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;

        public Store SendStore { get; set; }

        public int SendStoreId { get; set; }

        public Store ReceiveStore { get; set; }

        public int ReceiveStoreId { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Employee ReceivedBy { get; set; }

        public int ReceivedById { get; set; }

        public Employee DeliveredBy { get; set; }

        public int DeliveredById { get; set; }

        public string VehiclePlateNo { get; set; } = string.Empty;

        public Employee ApprovedBy { get; set; }

        public int ApprovedById { get; set; }

        public ICollection<TransferItem> TransferItems { get; set; }
    }
}
