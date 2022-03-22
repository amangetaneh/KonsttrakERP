namespace ERP.DTOs
{
    public class CreateReceiveDTO
    {
        public int ReceiveStoreId { get; set; }

        public int PurchaseId { get; set; }

        public int DeliveredById { get; set; }

        public string DeliveredFrom { get; set; }

        public int ReceivedById { get; set; }

        public ICollection<CreateReceiveItem> ReceiveItems { get; set; }

    }

    public class CreateReceiveItem
    {
        public int ItemId { get; set; }

        public int Qty { get; set; }

        public string Remark { get; set; } = string.Empty;

    }
}
