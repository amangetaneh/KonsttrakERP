using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class NonConsumableItem
    {
        [Key]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public string Model { get; set; } = string.Empty;

        public string SerialNo { get; set; } = string.Empty;

        public string AssetNo { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }
    }
}
