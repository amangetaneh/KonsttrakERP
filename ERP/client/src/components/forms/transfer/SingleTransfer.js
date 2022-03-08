import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Form, Button, Container, Spinner } from "react-bootstrap";
import TransferOutItem from "../../fragments/TransferOutConsumableItem";
import Item from "../../../models/Item";
import Header from "../../layouts/Header";
import axios from "axios";

function SingleTransfer() {
    const { id } = useParams();
    const [transfer, setTransfer] = useState({});
    const [isLoading, setIsLoading] = useState(true);
    const [addedItems, setAddedItems] = useState([]);

    useEffect(() => {
        axios.get(`https://localhost:7067/api/transfer/${id}`).then((res) => {
            res.data.date = new Date(res.data.date).toLocaleDateString();

            console.log(res.data);
            setTransfer(res.data);

            let items = [];
            for (let transferItem of res.data.transferItems) {
                console.log(transferItem);
                if (transferItem.item.category == 1) {
                    // Consumable
                    let itemObj = new Item();

                    itemObj.id = transferItem.item.itemId;
                    itemObj.name = transferItem.item.name;
                    itemObj.cost = transferItem.cost;

                    itemObj.qtyRequested = transferItem.qtyRequested;
                    itemObj.requestRemark = transferItem.requestRemark;
                    itemObj.spec = transferItem.item.consumableItem.spec;
                    itemObj.units = [transferItem.item.consumableItem.unit];

                    items.push(itemObj);
                }
            }

            setAddedItems(items);
            setIsLoading(false);
        });
    }, []);

    function topForm() {
        return (
            <Form>
                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Date</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={transfer.date}
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Transfer Number</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={transfer.id}
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Sending Site</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={transfer.sendStore.site.name}
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Sending Store</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={transfer.sendStore.name}
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Receiving Site</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={transfer.receiveStore.site.name}
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Receiving Store</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={transfer.receiveStore.name}
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Requested By</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={`${transfer.requestedBy.fName} ${transfer.requestedBy.mName}`}
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>

                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Approved By</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={
                                            transfer.approvedBy
                                                ? `${transfer.approvedBy.fName} ${transfer.approvedBy.mName}`
                                                : ""
                                        }
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Received By</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={
                                            transfer.receivedBy
                                                ? `${transfer.receivedBy.fName} ${transfer.receivedBy.mName}`
                                                : ""
                                        }
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>

                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Sent By</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={
                                            transfer.sentBy
                                                ? `${transfer.sentBy.fName} ${transfer.sentBy.mName}`
                                                : ""
                                        }
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Delivered By</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={
                                            transfer.deliveredBy
                                                ? `${transfer.deliveredBy.fName} ${transfer.deliveredBy.mName}`
                                                : ""
                                        }
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>

                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-3 mt-1">
                                    <Form.Label>Vehicle Plate No.</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        readOnly
                                        value={
                                            transfer.vehiclePlateNo
                                                ? transfer.vehiclePlateNo
                                                : ""
                                        }
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                </div>
            </Form>
        );
    }

    return (
        <>
            <Header title="Transfer Request" />

            <Container className="my-3">
                {isLoading ? (
                    <div className="d-flex justify-content-center align-content-center mt-4 pt-4">
                        <Spinner animation="grow" className="bg-teal" />
                    </div>
                ) : (
                    <>
                        {topForm()}

                        {addedItems.map((item, index) => (
                            <TransferOutItem
                                key={index}
                                addedItems={addedItems}
                                setAddedItems={setAddedItems}
                                index={index}
                            />
                        ))}

                        <div className="row">
                            <Button className="bg-teal-dark">
                                Transfer Action
                            </Button>
                        </div>
                    </>
                )}
            </Container>
        </>
    );
}

export default SingleTransfer;
