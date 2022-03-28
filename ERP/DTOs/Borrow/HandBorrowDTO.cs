namespace ERP.DTOs
{
    public class HandBorrowDTO
    {
        public int BorrowId { get; set; }

        public int HandedById { get; set; }

        public ICollection<HandBorrowItems> BorrowItems { get; set; }

    }

    public class HandBorrowItems
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; }

        public string HandRemark { get; set; }
        
        public ICollection<HandBorrowAsset>? EquipmentAssets { get; set; }

    }

    public class HandBorrowAsset
    {
        public int EquipmentAssetId { get; set; }
    }

}
