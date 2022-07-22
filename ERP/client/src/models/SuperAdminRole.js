class SuperAdminRole {
  constructor() {
    this.roleId = 0
    this.role = ""
    this.canRequestBorrow = true
    this.canApproveBorrow = true
    this.canHandBorrow = true
    this.canReturnBorrow = true
    this.canViewBorrow = true

    this.canRequestBuy = true
    this.canCheckBuy = true
    this.canApproveBuy = true
    this.canConfirmBuy = true
    this.canViewBuy = true

    this.canReceive = true
    this.canApproveReceive = true
    this.canViewReceive = true

    this.canEditUser = true

    this.canRequestPurchase = true
    this.canCheckPurchase = true
    this.canApprovePurchase = true
    this.canConfirmPurchase = true
    this.canViewPurchase = true

    this.canRequestBulkPurchase = true
    this.canApproveBulkPurchase = true
    this.canConfirmBulkPurchase = true
    this.canViewBulkPurchase = true

    this.canFixMaintenance = true
    this.canApproveMaintenance = true
    this.canRequestMaintenance = true
    this.canViewMaintenance = true

    this.canRequestIssue = true
    this.canApproveIssue = true
    this.canHandIssue = true
    this.canViewIssue = true

    this.canRequestTransfer = true
    this.canApproveTransfer = true
    this.canReceiveTransfer = true
    this.canSendTransfer = true
    this.canViewTransfer = true

    this.canGetStockNotification = true
    this.isAdmin = true
    this.isFinance = true
  }
}

export default SuperAdminRole
