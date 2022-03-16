namespace ERP
{
    public class TRANSFERSTATUS
    {

        public const int DECLINED = 0;
        public const int REQUESTED = 1;
        public const int APPROVED = 2;
        public const int SENT = 3;
        public const int RECEIVED = 4;
    }

    public class PurchaseStatus
    {
        public const int Declined = 0;
        public const int Requested = 1;
        public const int Approved = 2;
        public const int Checked = 3;
    }

    public class ReceiveStatus
    {
        public const int Received = 0;
        public const int Approved = 1;
    }

    public class ItemType
    {
        public const int Consumable = 1;
        public const int NonConsumable = 2;
    }

}
