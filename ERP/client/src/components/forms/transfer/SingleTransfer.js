import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Form, Container, Spinner } from "react-bootstrap";
import Item from "../../../models/Item";
import Header from "../../layouts/Header";
import axios from "axios";
import TransferStatusBadge from "./fragments/TransferStatusBadge";
import { ItemType, TransferStatus } from "../../../Constants";
import ApproveTransfer from "./ApproveTransfer";
import TransferApproveConsumableItem from "./fragments/TransferApproveConsumableItem";

function SingleTransfer() {
    const { id } = useParams();
    const [transfer, setTransfer] = useState({});
    const [isLoading, setIsLoading] = useState(true);
    const [addedItems, setAddedItems] = useState([]);

    useEffect(() => {
        axios.get(`https://localhost:7067/api/transfer/${id}`).then((res) => {
            setTransfer(res.data);
            let items = [];
            for (let transferItem of res.data.transferItems) {
                let itemObj = new Item();

                itemObj.id = transferItem.itemId;
                itemObj.name = transferItem.item.name;
                itemObj.cost = transferItem.cost;

                itemObj.qtyRequested = transferItem.qtyRequested;
                itemObj.requestRemark = transferItem.requestRemark;

                itemObj.qtySent = transferItem.qtySent;
                itemObj.sendRemark = transferItem.sendRemark;

                itemObj.qtyReceived = transferItem.qtyReceived;
                itemObj.receiveRemark = transferItem.receiveRemark;

                if (transferItem.item.type === ItemType.Consumable) {
                    itemObj.spec = transferItem.item.consumableItem.spec;
                    itemObj.units = [transferItem.item.consumableItem.unit];
                }

                items.push(itemObj);
            }

            setAddedItems(items);
            setIsLoading(false);
        });
    }, [id]);

    function FormRow({ labelL, labelR, valueL, valueR }) {
        //defines layout of each row
        return (
            <div className="row mx-2">
                <div className="col">
                    <Form.Group className="mb-3">
                        <div className="row">
                            <div className="col-3 mt-1">
                                <Form.Label>{labelL}</Form.Label>
                            </div>
                            <div className="col">
                                {typeof valueL === "object" ? (
                                    valueL
                                ) : (
                                    <Form.Control readOnly value={valueL} />
                                )}
                            </div>
                        </div>
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <div className="row">
                            <div className="col-3 mt-1">
                                <Form.Label>{labelR}</Form.Label>
                            </div>
                            <div className="col">
                                {typeof valueR === "object" ? (
                                    valueR
                                ) : (
                                    <Form.Control readOnly value={valueR} />
                                )}
                            </div>
                        </div>
                    </Form.Group>
                </div>
            </div>
        );
    }

    function TopForm() {
        return (
            <Form>
                <FormRow
                    labelL="Transfer Number"
                    valueL={transfer.id}
                    labelR="Status"
                    valueR={TransferStatusBadge(transfer.status)}
                />

                <FormRow
                    labelL="Sending Site"
                    valueL={transfer.sendStore.site.name}
                    labelR="Sending Store"
                    valueR={transfer.sendStore.name}
                />

                <FormRow
                    labelL="Receiving Site"
                    valueL={transfer.receiveStore.site.name}
                    labelR="Receiving Store"
                    valueR={transfer.receiveStore.name}
                />

                <FormRow
                    labelL="Requested By"
                    valueL={`${transfer.requestedBy.fName} ${transfer.requestedBy.mName}`}
                    labelR="Requested On"
                    valueR={new Date(transfer.requestDate).toLocaleString()}
                />

                {transfer.status >= TransferStatus.Approved && (
                    <>
                        <FormRow
                            labelL="Approved By"
                            valueL={
                                transfer.approvedBy
                                    ? `${transfer.approvedBy.fName} ${transfer.approvedBy.mName}`
                                    : ""
                            }
                            labelR="Approved On"
                            valueR={
                                transfer.approveDate
                                    ? new Date(
                                          transfer.approveDate
                                      ).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}

                {transfer.status >= TransferStatus.Sent && (
                    <>
                        <FormRow
                            labelL="Sent By"
                            valueL={
                                transfer.sentBy
                                    ? `${transfer.sentBy.fName} ${transfer.sentBy.mName}`
                                    : ""
                            }
                            labelR="Sent On"
                            valueR={
                                transfer.sendDate
                                    ? new Date(
                                          transfer.sendDate
                                      ).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}

                {transfer.status >= TransferStatus.Received && (
                    <>
                        <FormRow
                            labelL="Received By"
                            valueL={
                                transfer.receivedBy
                                    ? `${transfer.receivedBy.fName} ${transfer.receivedBy.mName}`
                                    : ""
                            }
                            labelR="Received On"
                            valueR={
                                transfer.receiveDate
                                    ? new Date(
                                          transfer.receiveDate
                                      ).toLocaleString()
                                    : ""
                            }
                        />

                        <FormRow
                            labelL="Delivered By"
                            valueL={
                                transfer.deliveredBy
                                    ? `${transfer.deliveredBy.fName} ${transfer.deliveredBy.mName}`
                                    : ""
                            }
                            labelR="Vehicle Plate No."
                            valueR={transfer.vehiclePlateNo}
                        />
                    </>
                )}
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
                        <TopForm />

                        {transfer.status === TransferStatus.Requested && (
                            <ApproveTransfer
                                addedItems={addedItems}
                                transfer={transfer}
                            />
                        )}

                        {transfer.status === TransferStatus.Declined && (
                            <>
                                <FormRow
                                    labelL="Declined By"
                                    valueL={
                                        transfer.approvedBy
                                            ? `${transfer.approvedBy.fName} ${transfer.approvedBy.mName}`
                                            : ""
                                    }
                                    labelR="Declined On"
                                    valueR={
                                        transfer.approveDate
                                            ? new Date(
                                                  transfer.approveDate
                                              ).toLocaleString()
                                            : ""
                                    }
                                />

                                <h1 className="display-6 text-center mt-2 mb-4">
                                    Transfer Request Has Been Declined
                                </h1>

                                {addedItems.map((item, index) => (
                                    <TransferApproveConsumableItem
                                        key={index}
                                        addedItems={addedItems}
                                        index={index}
                                    />
                                ))}
                            </>
                        )}
                    </>
                )}
            </Container>
        </>
    );
}

export default SingleTransfer;
