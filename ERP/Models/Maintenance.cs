using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class Maintenance
    {
        [Key]
        public int MaintenanceId { get; set; }

        public int Status { get; set; } = MAINTENANCESTATUS.REQUESTED;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime? ApproveDate { get; set; }

        public DateTime? FixDate { get; set; }

        public Site Site { get; set; } = null!;

        public int SiteId { get; set; }

        public Item Item { get; set; } = null!;

        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; }

        public EquipmentModel EquipmentModel { get; set; } = null!;

        public int? EquipmentAssetId { get; set; }

        public EquipmentAsset EquipmentAsset { get; set; } = null!;

        public string Reason { get; set; } = string.Empty;

        public Employee RequestedBy { get; set; } = null!;

        public int RequestedById { get; set; }

        public Employee ApprovedBy { get; set; } = null!;

        public int? ApprovedById { get; set; }

        public Employee FixedBy { get; set; } = null!;

        public int? FixedById { get; set; }

        public string? ApproveRemark { get; set; }

        public string? FixRemark { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }
    }
}
