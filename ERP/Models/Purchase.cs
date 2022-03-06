using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;

        public Store RequestStore { get; set; }

        public int RequestStoreId { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Employee CheckedBy { get; set; }

        public int CheckedById { get; set; }

        public Employee ApprovedBy { get; set; }

        public int ApprovedById { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
