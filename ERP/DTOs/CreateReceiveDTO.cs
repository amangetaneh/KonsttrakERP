namespace ERP.DTOs
{
    public class CreateReceiveDTO
    {
        public int PurchaseId { get; set; }

        public int DeliveredById { get; set; }

        public string DeliveredFrom { get; set; }

        public int ItemId { get; set; }

        public int QtyReceived { get; set; }

        public string ReceiveRemark { get; set; } = string.Empty;

    }
}
