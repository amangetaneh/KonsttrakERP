namespace ERP.DTOs
{
    public class ReceiveTransferDTO
    {
        public int Id { get; set; }

        public int ReceivedById { get; set; }

        public int DeliveredById { get; set; }

        public string VehiclePlateNo { get; set; } = string.Empty;

        public ICollection<ReceiveTransferItems> TransferItems { get; set; }
    }

    public class ReceiveTransferItems
    {
        public int ItemId { get; set; }

        public int QtyReceived { get; set; }

        public string ReceiveRemark { get; set; }
    }
}
