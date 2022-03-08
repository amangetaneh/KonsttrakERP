class ConsumableItem {
    constructor() {
        this.id = 0;
        this.name = "";
        this.category = 1;
        this.spec = "";
        this.unit = 0;
        this.units = [];
        this.qtyRequested = 0;
        this.qtyTransfered = 0;
        this.qtyReceived = 0;
        this.qtyAvailable = 0;
        this.sendQty = 0;
        this.cost = 0;
        this.total = 0;
        this.requestRemark = "";
        this.sendRemark = "";
    }

    load() {
        //set id and use this get units
        this.units = ["Cubic Meter", "Cubic Centimeter"];
        this.qtyAvailable = 10;
    }

    clear() {
        //set id and use this get units
        this.units = [];
        this.qtyAvailable = 0;
    }
}

export default ConsumableItem;
