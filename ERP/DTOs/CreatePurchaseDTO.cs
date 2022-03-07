namespace ERP.DTOs
{
    public class CreatePurchaseDTO
    {
        public DateTime Date { get; set; } = DateTime.Today;

        public int RequestStoreId { get; set; }

        public int RequestedById { get; set; }

        public ICollection<CreatePurchaseItem> PurchaseItems { get; set; }

    }

    public class CreatePurchaseItem
    {
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

    }
}
