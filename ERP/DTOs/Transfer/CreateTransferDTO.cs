namespace ERP.DTOs
{
    public class CreateTransferDTO
    {

        public int SendStoreId { get; set; }

        public int ReceiveStoreId { get; set; }

        public int RequestedById { get; set; }

        public ICollection<CreateTransferItem> TransferItems { get; set; }

    }

    public class CreateTransferItem
    {
        public int ItemId { get; set; }
        
        public int QtyRequested { get; set; }

        public string Model { get; set; }

        public string SerialNo { get; set; }

        public string AssetNo { get; set; }

        public string RequestRemark { get; set; } = string.Empty;

    }
}
