namespace ERP.DTOs
{
    public class SendTransferDTO
    {
        public int TransferId { get; set; }

        public int SentById { get; set; }

        public ICollection<SendMaterialTransferItems> TransferItems { get; set; }

    }

    public class SendMaterialTransferItems
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; } = 0;

        public string SendRemark { get; set; }

        public ICollection<SendEquipmentAsset>? EquipmentAssets { get; set; }

    }
    public class SendEquipmentAsset
    {
        public int EquipmentAssetId { get; set; }
    }

}
