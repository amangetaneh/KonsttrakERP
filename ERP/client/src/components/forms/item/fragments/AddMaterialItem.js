import { Form, InputGroup } from "react-bootstrap";

function AddMaterialItem({ addedItems, setAddedItems, index }) {

    function valueChanged(e) {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index][e.target.name] = e.target.value;
        setAddedItems(addedItemsCpy);
    }

    return (
        <div className="shadow pt-3 px-4 mb-4 rounded">
            <div className=" row justify-content-between">
                <div className="col-auto">
                    <h1 className="display-6 fs-4">Material {index + 1}</h1>
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
            <div className="row ">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Item Name</Form.Label>
                        <Form.Control
                            type="text"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        />
                        
                    </Form.Group>
                </div>
            </div>
            <div className="row ">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Item Specification</Form.Label>
                        <Form.Control
                            as="textarea"
                            value={spec}
                            onChange={(e) => setSpec(e.target.value)}
                        />
                        
                    </Form.Group>
                </div>
            </div>
            <div className="row ">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Item Unit</Form.Label>
                        <Form.Control
                            type="text"
                            value={unit}
                            onChange={(e) => setUnit(e.target.value)}
                        />
                        
                    </Form.Group>
                </div>
            </div>
            <div className="row ">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Item Cost</Form.Label>
                        <Form.Control
                            min="1"
                            type="number"
                            value={cost}
                            onChange={(e) => setCost(e.target.value)}
                        />
                        
                    </Form.Group>
                </div>
            </div>
        </div>
    );
}

export default AddMaterialItem;
