namespace ERP
{
    public class TransferStatus
    {

        public const int Declined = 0;
        public const int Requested = 1;
        public const int Approved = 2;
        public const int Sent = 3;
        public const int Received = 4;
    }

    public class PurchaseStatus
    {
        public const int Requested = 0;
        public const int Approved = 1;
        public const int Checked = 2;
    }

    public class ItemType
    {
        public const int Consumable = 1;
        public const int NonConsumable = 2;
    }

}
