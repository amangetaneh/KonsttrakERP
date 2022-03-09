namespace ERP.DTOs
{
    public class SendTransferDTO
    {
        public int Id { get; set; }

        public int SentById { get; set; }

       public ICollection<SendTransferItems> TransferItems { get; set; }
    }

    public class SendTransferItems
    {
        public int ItemId { get; set; }

        public int QtySent { get; set; }

        public string SendRemark { get; set; }
    }
}
