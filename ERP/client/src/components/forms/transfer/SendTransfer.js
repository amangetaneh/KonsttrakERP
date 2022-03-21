import React from "react";
import PropTypes from "prop-types";
import TransferSendMaterial from "./fragments/TransferSendMaterial";
import { Button, Form, Spinner } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { sendTransfer } from "../../../api/transfer";
import { useMutation } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function SendTransfer({ addedItems, setAddedItems, transfer }) {
    const navigate = useNavigate();

    const {
        isLoading: isSendLoading,
        isError: isSendError,
        mutate: submitSendTransfer,
    } = useMutation(sendTransfer, {
        onSuccess: (res) => navigate(-1),
    });

    function submit(e) {
        e.preventDefault();
        if (isSendLoading) return;

        const data = {
            transferId: transfer.transferId,
            sentById: 1,
            transferItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                sendRemark: item.sendRemark,
            };
            data.transferItems.push(tempItem);
        }

        submitSendTransfer(data);
    }

    if (isSendError) return <ConnectionError />;

    return (
        <>
            <Form onSubmit={submit}>
                {addedItems.map((item, index) => (
                    <TransferSendMaterial
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                ))}

                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        {isSendLoading && (
                            <Spinner
                                className="me-2"
                                animation="grow"
                                size="sm"
                            />
                        )}
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
