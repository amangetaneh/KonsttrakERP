namespace ERP
{
    public class TRANSFERREPORTGROUPBY
    {
        public const string STATUS = "Status";
        public const string REQUESTDATE = "Request Date";
        public const string APPROVEDATE = "Approve Date";
        public const string SENDDATE = "Send Date";
        public const string RECEIVEDATE = "Receive Date";
        public const string REQUESTEDBY = "Requesting Employee";
        public const string APPROVEDBY = "Approving Employee";
        public const string SENTBY = "Sending Employee";
        public const string RECEIVEDBY = "Receiveing Employee";
        public const string SENDSITE = "Sending Site";
        public const string RECEIVESITE = "Receiving Site";
        
    }

    public class MICELLANEOUSKEYS
    {
        public const string COMPANYPREFIX = "companyPrefix";
        public const string COMPANYNAME = "companyName";
    }

    public class NOTIFICATIONTYPE
    {
        public const string TRANSFER = "transferNotification";
        public const string ISSUE = "issueNotification";
        public const string PURCHASE = "purchaseNotification";
        public const string BORROW = "borrowNotification";
        public const string RECEIVE = "receiveNotification";
        public const string MAINTENANCE = "maintenanceNotification";
        public const string RETURN = "returnNotification";
        public const string BUY = "buyNotification";
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
        public const int QUEUED = 1;
        public const int REQUESTED = 2;
        public const int CHECKED = 3;
        public const int APPROVED = 4;
        public const int PURCHASED = 5;
    }

    public class BUYSTATUS
    {
        public const int DECLINED = 0;
        public const int REQUESTED = 1;
        public const int QUEUED = 2;
        public const int CHECKED = 3;
        public const int APPROVED = 4;
        public const int BOUGHT = 5;
    }

    public class RECEIVESTATUS
    {
        public const int PURCHASED = 1;
        public const int RECEIVED = 2;
        public const int APPROVED = 3;
    }

    public class ITEMTYPE
    {
        public const int MATERIAL = 1;
        public const int EQUIPMENT = 2;
    }

}
