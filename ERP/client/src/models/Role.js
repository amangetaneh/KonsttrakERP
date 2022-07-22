class Role {
  constructor() {
    this.roleId = 0
    this.role = ""
    this.canRequestBorrow = true
    this.canApproveBorrow = false
    this.canHandBorrow = false
    this.canReturnBorrow = false
    this.canViewBorrow = true

    this.canRequestBuy = true
    this.canCheckBuy = false
    this.canApproveBuy = false
    this.canConfirmBuy = false
    this.canViewBuy = true

    this.canReceive = false
    this.canApproveReceive = false
    this.canViewReceive = true

    this.canEditUser = false

    this.canRequestPurchase = false
    this.canCheckPurchase = false
    this.canApprovePurchase = false
    this.canConfirmPurchase = false
    this.canViewPurchase = true

    this.canRequestBulkPurchase = false
    this.canApproveBulkPurchase = false
    this.canConfirmBulkPurchase = false
    this.canViewBulkPurchase = true

    this.canFixMaintenance = false
    this.canApproveMaintenance = false
    this.canRequestMaintenance = false
    this.canViewMaintenance = true

    this.canRequestIssue = true
    this.canApproveIssue = false
    this.canHandIssue = false
    this.canViewIssue = true

    this.canRequestTransfer = false
    this.canApproveTransfer = false
    this.canReceiveTransfer = false
    this.canSendTransfer = false
    this.canViewTransfer = true

    this.canGetStockNotification = false
    this.isAdmin = false
    this.isFinance = false
  }
}

export default Role
