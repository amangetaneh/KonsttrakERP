import React from "react";
import PropTypes from "prop-types";
import TransferApproveConsumableItem from "./fragments/TransferApproveConsumableItem";
import { Button } from "react-bootstrap";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function ApproveTransfer({ addedItems, transfer }) {
    const navigate = useNavigate();

    function submit(type) {
        const data = {
            transferId: transfer.id,
            approvedById: 1, ////////////////////////
        };

        axios
            .post(`https://localhost:7067/api/transfer/${type}`, data)
            .then((res) => {
                navigate(-1);
            });
    }

    return (
        <>
            {addedItems.map((item, index) => (
                <TransferApproveConsumableItem
                    key={index}
                    addedItems={addedItems}
                    index={index}
                />
            ))}

            <div className="row">
                <div className="col-8 d-grid">
                    <Button
                        className="btn-teal-dark"
                        onClick={submit.bind(this, "approve")}
                    >
                        Approve
                    </Button>
                </div>
                <div className="col-4 d-grid">
                    <Button
                        variant="danger"
                        onClick={submit.bind(this, "decline")}
                    >
                        Decline
                    </Button>
                </div>
            </div>
        </>
    );
}

ApproveTransfer.propTypes = {
    addedItems: PropTypes.array.isRequired,
};

export default ApproveTransfer;
