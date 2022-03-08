using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class PurchaseItem
    {
        [Key]
        public int PurchaseId { get; set; }

        [Key]
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public int? QtyApproved { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalCost => (QtyApproved * Cost);

        public string? Remark { get; set; }

        [JsonIgnore]

        [ForeignKey("PurchaseId")]
        public Purchase Purchase { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}
