export const TRANSFERREPORTGROUPBY = {
    STATUS: "Status",
    REQUESTDATE: "Request Date",
    APPROVEDATE: "Approve Date",
    SENDDATE: "Send Date",
    RECEIVEDATE: "Receive Date",
    REQUESTEDBY: "Requesting Employee",
    APPROVEDBY: "Approving Employee",
    SENTBY: "Sending Employee",
    RECEIVEDBY: "Receiveing Employee",
    SENDSITE: "Sending Site",
    RECEIVESITE: "Receiving Site",
};

export const NOTIFICATIONTYPE = {
    TRANSFER: "transferNotification",
    ISSUE: "issueNotification",
    PURCHASE: "purchaseNotification",
    BORROW: "borrowNotification",
    RECEIVE: "receiveNotification",
    MAINTENANCE: "maintenanceNotification",
    RETURN: "returnNotification",
    BUY: "buyNotification",
};

export const NOTIFICATIONROUTE = {
    [NOTIFICATIONTYPE.TRANSFER]: "/transfer",
    [NOTIFICATIONTYPE.ISSUE]: "/issue",
    [NOTIFICATIONTYPE.PURCHASE]: "/purchase",
    [NOTIFICATIONTYPE.BORROW]: "/borrow",
    [NOTIFICATIONTYPE.RECEIVE]: "/receive",
    [NOTIFICATIONTYPE.MAINTENANCE]: "/maintenance",
    [NOTIFICATIONTYPE.RETURN]: "/return",
    [NOTIFICATIONTYPE.BUY]: "/buy"
};

export const MAINTENANCESTATUS = {
    DECLINED: 0,
    REQUESTED: 1,
    APPROVED: 2,
    FIXED: 3,
    0: "Declined",
    1: "Requested",
    2: "Approved",
    3: "Fixed",
};

export const TRANSFERSTATUS = {
    DECLINED: 0,
    REQUESTED: 1,
    APPROVED: 2,
    SENT: 3,
    RECEIVED: 4,
    0: "Declined",
    1: "Requested",
    2: "Approved",
    3: "Sent",
    4: "Received",
};

export const ISSUESTATUS = {
    DECLINED: 0,
    REQUESTED: 1,
    APPROVED: 2,
    HANDED: 3,
    0: "Declined",
    1: "Requested",
    2: "Approved",
    3: "Handed",
};

export const BORROWSTATUS = {
    DECLINED: 0,
    REQUESTED: 1,
    APPROVED: 2,
    HANDED: 3,
    0: "Declined",
    1: "Requested",
    2: "Approved",
    3: "Handed",
};

export const PURCHASESTATUS = {
    DECLINED: 0,
    QUEUED: 1,
    REQUESTED: 2,
    CHECKED: 3,
    APPROVED: 4,
    PURCHASED: 5,

    0: "Declined",
    1: "Queued",
    2: "Requested",
    3: "Checked",
    4: "Approved",
    5: "Purchased",
};

export const BUYSTATUS = {
    DECLINED: 0,
    REQUESTED: 1,
    QUEUED: 2,
    CHECKED: 3,
    APPROVED: 4,
    BOUGHT: 5,
    0: "Declined",
    1: "Requested",
    2: "Queued",
    3: "Checked",
    4: "Approved",
    5: "Bought",
};

export const RECEIVESTATUS = {
    RECEIVED: 1,
    APPROVED: 2,
    0: "Received",
    1: "Approved",
};

export const ITEMTYPE = {
    MATERIAL: 1,
    EQUIPMENT: 2,
};
