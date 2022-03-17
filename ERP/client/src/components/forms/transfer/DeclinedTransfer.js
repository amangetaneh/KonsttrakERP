import React from "react";
import PropTypes from "prop-types";
import TransferDecliendConsumableItem from "./fragments/TransferDeclinedConsumableItem";

function DeclinedTransfer({ transfer, addedItems, FormRow }) {
    return (
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
                        ? new Date(transfer.approveDate).toLocaleString()
                        : ""
                }
            />

            <h1 className="display-6 text-center mt-2 mb-4">
                Transfer Request Has Been Declined
            </h1>

            {addedItems.map((item, index) => (
                <TransferDecliendConsumableItem
                    key={index}
                    addedItems={addedItems}
                    index={index}
                />
            ))}
        </>
    );
}

DeclinedTransfer.propTypes = {
    transfer: PropTypes.object.isRequired,
    addedItems: PropTypes.array.isRequired,
};

export default DeclinedTransfer;
