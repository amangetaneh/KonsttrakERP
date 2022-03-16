class Item {
    constructor() {
        this.itemId = 0;
        this.name = "";
        this.type = 1;
        this.spec = "";
        this.unit = 0;
        this.units = [];
        this.qtyRequested = 0;
        this.qtySent = 0;
        this.qtyReceived = 0;
        this.qtyAvailable = 0;
        this.cost = 0;
        this.total = 0;
        this.requestRemark = "";
        this.sendRemark = "";
        this.receiveRemark = "";
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

export default Item;
