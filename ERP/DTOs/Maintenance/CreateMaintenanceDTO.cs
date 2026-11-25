namespace ERP.DTOs
{
    public class CreateMaintenanceDTO
    {

        public int RequestedById { get; set; }

        public int EquipmentModelId { get; set; }

        public int? EquipmentAssetId { get; set; }

        public int SiteId { get; set; }

        public string Reason { get; set; } = string.Empty;

        public int ItemId { get; set; }



    }
}
