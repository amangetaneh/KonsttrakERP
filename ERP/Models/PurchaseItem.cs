using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class PurchaseItem
    {
        [Key]
        public int PurchaseId { get; set; }

        [Key]
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public int QtyApproved { get; set; }

        public Double Cost { get; set; }

        public Double TotalCost => (QtyApproved * Cost);

        public string? Remark { get; set; }

        [ForeignKey("PurchaseId")]
        public Purchase Purchase { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}
