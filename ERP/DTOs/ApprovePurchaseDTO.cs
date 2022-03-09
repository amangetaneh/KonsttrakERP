namespace ERP.DTOs
{
    public class ApprovePurchaseDTO
    {
        public int PurchaseId { get; set; }

        public int ApprovedById { get; set; }

        public ICollection<ApprovePurchaseItem> PurchaseItems { get; set; }


    }
    public class ApprovePurchaseItem
    {
        public int ItemId { get; set; }

        public int QtyApproved { get; set; }

    }
}