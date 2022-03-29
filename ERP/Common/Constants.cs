namespace ERP
{
    public class NOTIFICATIONTYPE
    {
        public const string TRANSFER = "transferNotification";
        public const string ISSUE = "issueNotification";
        public const string Buy = "buyNotification";
        public const string PURCHASE = "purchaseNotification";
        public const string BORROW = "borrowNotification";
        public const string RECEIVE = "receiveNotification";
        public const string MAINTAINANCE = "maintainanceNotification";
        public const string RETURN = "returnNotification";
    }

    public class TRANSFERSTATUS
    {

        public const int DECLINED = 0;
        public const int REQUESTED = 1;
        public const int APPROVED = 2;
        public const int SENT = 3;
        public const int RECEIVED = 4;
    }

    public class ISSUESTATUS
    {
        public const int DECLINED = 0;
        public const int REQUESTED = 1;
        public const int APPROVED = 2;
        public const int HANDED = 3;
    }

    public class BORROWSTATUS
    {
        public const int DECLINED = 0;
        public const int REQUESTED = 1;
        public const int APPROVED = 2;
        public const int HANDED = 3;
    }

    public class MAINTENANCESTATUS
    {
        public const int DECLINED = 0;
        public const int REQUESTED = 1;
        public const int APPROVED = 2;
        public const int FIXED = 3;
    }

    public class PURCHASESTATUS
    {
        public const int DECLINED = 0;
        public const int REQUESTED = 1;
        public const int CHECKED = 2;
        public const int APPROVED = 3;
        public const int PURCHASED = 4;
        public const int QUEUED = 5;
    }

    public class BUYSTATUS
    {
        public const int DECLINED = 0;
        public const int REQUESTED = 1;
        public const int CHECKED = 2;
        public const int APPROVED = 3;
        public const int BOUGHT = 4;
        public const int QUEUED = 5;
    }

    public class RECEIVESTATUS
    {
        public const int RECEIVED = 1;
        public const int APPROVED = 2;
    }

    public class ITEMTYPE
    {
        public const int MATERIAL = 1;
        public const int EQUIPMENT = 2;
    }

}
