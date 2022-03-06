using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class Receive
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;

        public Store ReceiveStore { get; set; }

        public int ReceiveStoreId { get; set; }

        public string DeliveredFrom { get; set; }

        public Purchase Purchase { get; set; }

        public int PurchaseId { get; set; }

        public Employee DeliveredBy { get; set; }

        public int DeliveredById { get; set; }

        public Employee ReceivedBy { get; set; }

        public int ReceivedById { get; set; }

        public Employee ApprovedBy { get; set; }

        public int ApprovedById { get; set; }

        public ICollection<ReceiveItem> ReceiveItems { get; set; }
    }
}

