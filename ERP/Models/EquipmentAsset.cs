using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class EquipmentAsset
    {
        [Key]
        public int EquipmentAssetId { get; set; }

        public int EquipmentModelId { get; set; }

        public EquipmentModel EquipmentModel { get; set; } = null!;

        public string AssetNo { get; set; } = string.Empty;

        public string? SerialNo { get; set; }

        public int? CurrentSiteId { get; set; }

        public Site CurrentSite { get; set; } = null!;

        public int? AssetDamageId { get; set; }

        public AssetDamage? AssetDamage { get; set; }

        public int? CurrentEmployeeId { get; set; }

        public Employee? CurrentEmployee { get; set; }


    }
}
