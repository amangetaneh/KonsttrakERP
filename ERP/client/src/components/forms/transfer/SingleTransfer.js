import { useEffect, useState, useMemo } from "react";
import { useParams } from "react-router-dom";
import { Form, Container } from "react-bootstrap";
import Item from "../../../models/Item";
import Header from "../../layouts/Header";
import TransferStatusBadge from "./fragments/TransferStatusBadge";
import { ITEMTYPE, TRANSFERSTATUS } from "../../../Constants";
import ApproveTransfer from "./ApproveTransfer";
import SendTransfer from "./SendTransfer";
import ReceiveTransfer from "./ReceiveTransfer";
import ViewTransfer from "./ViewTransfer";
import DeclinedTransfer from "./DeclinedTransfer";
import { useQuery } from "react-query";
import { fetchTransfer } from "../../../api/transfer";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import AlertNotification from "../../fragments/AlertNotification";

function SingleTransfer() {
    const { id } = useParams();
    const [transfer, setTransfer] = useState({});
    const [addedItems, setAddedItems] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    //const userRole = useSelector((state) => state.auth.data[2]);
    
    var query = useQuery(["transfer", id], () => fetchTransfer(id));

    useEffect(() => {
        if (query.data === undefined) return;
        setTransfer(query.data);
        let items = [];
        for (let transferItem of query.data.transferItems) {
            let itemObj = new Item();

            itemObj.itemId = transferItem.itemId;
            itemObj.name = transferItem.item.name;
            itemObj.cost = transferItem.cost;
            itemObj.type = transferItem.item.type;

            itemObj.qtyRequested = transferItem.qtyRequested;
            itemObj.requestRemark = transferItem.requestRemark;

            if (
                query.data.status >= TRANSFERSTATUS.APPROVED ||
                query.data.status === TRANSFERSTATUS.DECLINED
            ) {
                itemObj.qtyApproved = transferItem.qtyApproved;
                itemObj.approveRemark = transferItem.approveRemark;
            }

            if (query.data.status >= TRANSFERSTATUS.SENT) {
                itemObj.sendRemark = transferItem.sendRemark;
            }

            if (query.data.status >= TRANSFERSTATUS.RECEIVED) {
                itemObj.receiveRemark = transferItem.receiveRemark;
            }

            if (transferItem.item.type === ITEMTYPE.MATERIAL) {
                itemObj.spec = transferItem.item.material.spec;
                itemObj.units = [transferItem.item.material.unit];
            }

            if (transferItem.item.type === ITEMTYPE.EQUIPMENT) {
                itemObj.description = transferItem.item.equipment.description;
                itemObj.units = [transferItem.item.equipment.unit];
                itemObj.equipmentModelId = transferItem.equipmentModelId;
                itemObj.equipmentAssets = transferItem.transferEquipmentAssets;
            }

            items.push(itemObj);
        }

        setAddedItems(items);
        setIsLoading(false);
    }, [query.data]);

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
                                    <Form.Control type="text" readOnly value={valueL} />
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
                                    <Form.Control type="text" readOnly value={valueR} />
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
                    valueL={transfer.transferId}
                    labelR="Status"
                    valueR={TransferStatusBadge(transfer.status)}
                />

                <FormRow
                    labelL="Sending Site"
                    valueL={transfer.sendSite.name}
                    labelR="Receiving Site"
                    valueR={transfer.receiveSite.name}
                />

                <FormRow
                    labelL="Requested By"
                    valueL={`${transfer.requestedBy.fName} ${transfer.requestedBy.mName}`}
                    labelR="Requested On"
                    valueR={new Date(transfer.requestDate).toLocaleString()}
                />

                {transfer.status >= TRANSFERSTATUS.APPROVED && (
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
                                    ? new Date(transfer.approveDate).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}

                {transfer.status >= TRANSFERSTATUS.SENT && (
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
                                    ? new Date(transfer.sendDate).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}

                {transfer.status >= TRANSFERSTATUS.RECEIVED && (
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
                                    ? new Date(transfer.receiveDate).toLocaleString()
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

    const titles = useMemo(
        () => [
            "Declined Transfer",
            "Approve Transfer",
            "Transfer Out",
            "Transfer In",
            "Completed Transfer",
        ],
        []
    );

    if (isLoading) return <LoadingSpinner />;

    if (query.isError) return <ConnectionError />;

    return (
        <>
            <Header title={titles[transfer.status]} />

            <Container className="my-3">
                <>
                    <AlertNotification
                        title="Transfer Waiting for Appproval"
                        content="This Transfer Needs Approval to Conitnue"
                    />
                    <TopForm />

                    {transfer.status === TRANSFERSTATUS.DECLINED && (
                        <DeclinedTransfer
                            addedItems={addedItems}
                            FormRow={FormRow}
                            transfer={transfer}
                        />
                    )}

                    {transfer.status === TRANSFERSTATUS.REQUESTED && (
                        <ApproveTransfer
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            transfer={transfer}
                        />
                    )}

                    {transfer.status === TRANSFERSTATUS.APPROVED && (
                        <SendTransfer
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            transfer={transfer}
                        />
                    )}

                    {transfer.status === TRANSFERSTATUS.SENT && (
                        <ReceiveTransfer
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            transfer={transfer}
                        />
                    )}

                    {transfer.status === TRANSFERSTATUS.RECEIVED && (
                        <ViewTransfer addedItems={addedItems} transfer={transfer} />
                    )}
                </>
            </Container>
        </>
    );
}

export default SingleTransfer;
