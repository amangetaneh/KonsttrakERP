namespace ERP.DTOs
{
    public class CreatePurchaseDTO
    {
        public int RequestStoreId { get; set; }

        public int RequestedById { get; set; }

        public ICollection<CreatePurchaseItem> PurchaseItems { get; set; }

    }

    public class CreatePurchaseItem
    {
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public string RequestRemark { get; set; } = string.Empty;

    }
}
