export const NOTIFICATIONTYPE = {
    TRANSFER: "transferNotification",
    ISSUE: "issueNotification",
    PURCHASE: "purchaseNotification",
    BORROW: "borrowNotification",
    RECEIVE: "receiveNotification",
    MAINTAINANCE: "maintainanceNotification",
};

export const NOTIFICATIONROUTE={
    [NOTIFICATIONTYPE.TRANSFER]: "/transfer",
    [NOTIFICATIONTYPE.ISSUE]: "/issue",
    [NOTIFICATIONTYPE.PURCHASE]: "/purchase",
    [NOTIFICATIONTYPE.BORROW]: "/borrow",
    [NOTIFICATIONTYPE.RECEIVE]: "/receive",
    [NOTIFICATIONTYPE.MAINTAINANCE]: "/maintainance",     
}

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

export const PurchaseStatus = {
    Declined: 0,
    Requested: 1,
    Approved: 2,
    Checked: 3,
    0: "Declined",
    1: "Requested",
    2: "Approved",
    3: "Checked",
};

export const ReceiveStatus = {
    Received: 0,
    Approved: 1,
    0: "Received",
    1: "Approved",
};

export const ITEMTYPE = {
    MATERIAL: 1,
    EQUIPMENT: 2,
};
