import { Form, InputGroup } from "react-bootstrap";
import PropTypes from "prop-types";

function TransferSendConsumableItem({ addedItems, setAddedItems, index }) {
    function valueChanged(e) {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index][e.target.name] = e.target.value;
        setAddedItems(addedItemsCpy);
    }

    return (
        <div className="shadow pt-3 px-4 mb-4 rounded">
            <div className=" row justify-content-between">
                <div className="col-auto">
                    <h1 className="display-6 fs-4">Item {index + 1}</h1>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Item</Form.Label>
                        <Form.Control
                            type="text"
                            readOnly
                            value={addedItems[index].name}
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Quantity Requested</Form.Label>
                        <InputGroup className="mb-3">
                            <Form.Control
                                type="text"
                                name="qty"
                                readOnly
                                value={addedItems[index].qtyRequested}
                            />
                            <Form.Control
                                name="unit"
                                type="text"
                                readOnly
                                value={addedItems[index].units[0]}
                            />
                        </InputGroup>
                    </Form.Group>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Specification</Form.Label>
                        <Form.Control
                            as="textarea"
                            name="spec"
                            rows={5}
                            value={addedItems[index].spec}
                            readOnly
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <div className="row">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Unit Cost</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="cost"
                                    value={addedItems[index].cost}
                                    readOnly
                                />
                            </Form.Group>
                        </div>
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Total Cost</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="total"
                                    readOnly
                                    value={
                                        addedItems[index].qtyRequested *
                                        addedItems[index].cost
                                    }
                                />
                            </Form.Group>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Quantity Transfered</Form.Label>
                                <Form.Control
                                    min="0"
                                    type="number"
                                    name="qtySent"
                                    value={addedItems[index].qtySent}
                                    onChange={valueChanged}
                                    required
                                />
                            </Form.Group>
                        </div>
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Total Cost</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="total"
                                    readOnly
                                    value={
                                        addedItems[index].qtySent *
                                        addedItems[index].cost
                                    }
                                />
                            </Form.Group>
                        </div>
                    </div>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Request Remark</Form.Label>
                        <Form.Control
                            type="text"
                            readOnly
                            value={addedItems[index].requestRemark}
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Send Remark</Form.Label>
                        <Form.Control
                            type="text"
                            name="sendRemark"
                            value={addedItems[index].sendRemark}
                            onChange={valueChanged}
                        />
                    </Form.Group>
                </div>
            </div>
        </div>
    );
}

TransferSendConsumableItem.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    index: PropTypes.number.isRequired,
};

export default TransferSendConsumableItem;
