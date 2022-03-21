import React, { useRef } from "react";
import PropTypes from "prop-types";
import { Button, Form } from "react-bootstrap";
import TransferViewMaterial from "./fragments/TransferViewMaterial";
import { useNavigate } from "react-router-dom";

function ViewTransfer({ addedItems }) {
    const navigate = useNavigate();

    function goBack() {
        navigate(-1);
    }

    return (
        <>
            <>
                {addedItems.map((item, index) => (
                    <TransferViewMaterial
                        key={index}
                        addedItems={addedItems}
                        index={index}
                    />
                ))}

                <div className="d-grid">
                    <Button className="btn-teal" onClick={goBack}>
                        Back
                    </Button>
                </div>
            </>
        </>
    );
}

ViewTransfer.propTypes = {
    addedItems: PropTypes.array.isRequired,
};

export default ViewTransfer;
