import React from "react";
import PropTypes from "prop-types";
import TransferSendConsumableItem from "./fragments/TransferSendConsumableItem";
import { Button, Form } from "react-bootstrap";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import Info from "../../fragments/Info";

function SendTransfer({ addedItems, setAddedItems, transfer }) {
    const navigate = useNavigate();

    function submit(e) {
        e.preventDefault();

        const data = {
            id: transfer.id,
            sentById: 1,
            transferItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.id),
                qtySent: Number(item.qtySent),
                sendRemark: item.sendRemark,
            };
            data.transferItems.push(tempItem);
        }

        axios
            .post("https://localhost:7067/api/transfer/send", data)
            .then((res) => {
                navigate(-1);
            });
    }


    return (
        <>
            <Form onSubmit={submit}>
                {addedItems.map((item, index) => (
                    <TransferSendConsumableItem
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                ))}

                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        Transfer Out
                    </Button>
                </div>
            </Form>
        </>
    );
}

SendTransfer.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    transfer: PropTypes.object.isRequired,
};

export default SendTransfer;
