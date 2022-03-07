namespace ERP.DTOs
{
    public class CreateReceiveDTO
    {
        public DateTime Date { get; set; } = DateTime.Today;

        public int ReceiveStoreId { get; set; }

        public int PurchaseId { get; set; }

        public int DeliveredById { get; set; }
        
        public int ReceivedById { get; set; }

        public ICollection<CreateReceiveItem> ReceiveItems { get; set; }

    }

    public class CreateReceiveItem
    {
        public int ItemId { get; set; }

        public int Qty { get; set; }

    }
}
