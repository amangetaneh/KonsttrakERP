import React from "react";
import PropTypes from "prop-types";
import TransferApproveConsumableItem from "./fragments/TransferApproveConsumableItem";
import { Button, Spinner } from "react-bootstrap";
import { approveTransfer, declineTransfer } from "../../../api/transfer";
import { useNavigate } from "react-router-dom";
import { useMutation } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function ApproveTransfer({ addedItems, setAddedItems, transfer }) {
    const navigate = useNavigate();

    const {
        isLoading: isApproveLoading,
        isError: isApproveError,
        mutate: submitApproveTransfer,
    } = useMutation(approveTransfer, {
        onSuccess: (res) => navigate(-1),
    });

    const {
        isLoading: isDeclineLoading,
        isError: isDeclineError,
        mutate: submitDeclineTransfer,
    } = useMutation(declineTransfer, {
        onSuccess: (res) => navigate(-1),
    });

    function submit(type, e) {
        e.preventDefault();
        if (isApproveLoading || isDeclineLoading) return;

        const data = {
            transferId: transfer.transferId,
            approvedById: 1, ////////////////////////
            transferItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                qtyApproved: Number(item.qtyApproved),
                approveRemark: item.approveRemark,
            };

            data.transferItems.push(tempItem);
        }

        if (type === "decline") submitDeclineTransfer(data);
        else submitApproveTransfer(data);
    }

    if (isApproveError || isDeclineError) return <ConnectionError />;

    return (
        <>
            {addedItems.map((item, index) => (
                <TransferApproveConsumableItem
                    key={index}
                    addedItems={addedItems}
                    setAddedItems={setAddedItems}
                    index={index}
                />
            ))}

            <div className="row">
                <div className="col-8 d-grid">
                    <Button
                        className="btn-teal-dark"
                        onClick={submit.bind(this, "approve")}
                    >
                        {isApproveLoading && (
                            <Spinner
                                className="me-2"
                                animation="grow"
                                size="sm"
                            />
                        )}
                        Approve
                    </Button>
                </div>
                <div className="col-4 d-grid">
                    <Button
                        variant="danger"
                        onClick={submit.bind(this, "decline")}
                    >
                        {isDeclineLoading && (
                            <Spinner
                                className="me-2"
                                animation="grow"
                                size="sm"
                            />
                        )}
                        Decline
                    </Button>
                </div>
            </div>
        </>
    );
}

ApproveTransfer.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    transfer: PropTypes.object.isRequired,
};

export default ApproveTransfer;
