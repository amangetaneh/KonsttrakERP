import React, { useRef } from "react";
import PropTypes from "prop-types";
import TransferReceiveConsumableItem from "./fragments/TransferReceiveConsumableItem";
import { Button, Form } from "react-bootstrap";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function ReceiveTransfer({ addedItems, setAddedItems, transfer }) {
    const navigate = useNavigate();
    const vehiclePlateRef = useRef();

    function submit(e) {
        e.preventDefault();

        const data = {
            id: transfer.id,
            receivedById: 1,//////////////////////////
            deliveredById: 1,
            vehiclePlateNo: vehiclePlateRef.current.value,
            transferItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.id),
                qtyReceived: Number(item.qtyReceived),
                receiveRemark: item.receiveRemark,
            };
            data.transferItems.push(tempItem);
        }

        axios
            .post("https://localhost:7067/api/transfer/receive", data)
            .then((res) => {
                navigate(-1);
            });
    }

    function DeliveredByForm() {
        return (
            <div className="row mx-2">
                <div className="col">
                    <Form.Group className="mb-3">
                        <div className="row">
                            <div className="col-3 mt-1">
                                <Form.Label>Delivered By</Form.Label>
                            </div>
                            <div className="col">
                                <Form.Select >
                                    <option value="">Select Driver</option>
                                </Form.Select>
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
                                    type="text"
                                    required
                                    ref={vehiclePlateRef}
                                />
                            </div>
                        </div>
                    </Form.Group>
                </div>
            </div>
        );
    }

    return (
        <>
            <Form onSubmit={submit}>
                {addedItems.map((item, index) => (
                    <TransferReceiveConsumableItem
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                ))}

                <DeliveredByForm />

                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        Transfer In
                    </Button>
                </div>
            </Form>
        </>
    );
}

ReceiveTransfer.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    transfer: PropTypes.object.isRequired,
};

export default ReceiveTransfer;
