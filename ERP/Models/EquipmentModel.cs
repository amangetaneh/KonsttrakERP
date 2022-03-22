using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class EquipmentModel
    {
        [Key]
        public int EquipmentModelId { get; set; }

        public int EquipmentId { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        public Equipment Equipment { get; set; }

        public ICollection<EquipmentAsset> EquipmentAssets { get; set; }
    }
}
