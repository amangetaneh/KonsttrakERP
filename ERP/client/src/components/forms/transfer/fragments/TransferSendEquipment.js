import { useState, useEffect } from "react";
import { Form, InputGroup } from "react-bootstrap";
import PropTypes from "prop-types";
import { useQuery } from "react-query";
import { fetchEquipmentModel } from "../../../../api/item";

function TransferSendEquipment({ addedItems, setAddedItems, index }) {
    const [isSetAssets, SetIsSetAssets] = useState(false);

    var modelQuery = useQuery(
        ["model", addedItems[index].equipmentModelId],
        () =>
            addedItems[index].equipmentModelId &&
            fetchEquipmentModel(addedItems[index].equipmentModelId)
    );

    useEffect(() => {
        //Initialize asset number entry array
        const addedItemsCpy = [...addedItems];

        addedItemsCpy[index].equipmentAssets = Array(addedItems[index].qtyApproved).fill({
            equipmentAssetId: "",
        });

        setAddedItems(addedItemsCpy);
    }, []);

    function assetChanged(changedIndex, e) {
        const newAddedItemsCpy = addedItems.map((item, i) => {
            if (i !== index) return item;

            let { equipmentAssets } = item;

            const newEquipmentAssets = equipmentAssets.map((asset, assetIndex) => {
                if (assetIndex !== changedIndex) return asset;
                return { equipmentAssetId: e.target.value };
            });

            return { ...item, equipmentAssets: newEquipmentAssets };
        });

        setAddedItems(newAddedItemsCpy);
    }

    function valueChanged(e) {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index][e.target.name] = e.target.value;
        setAddedItems(addedItemsCpy);
    }

    return (
        <div className="shadow pt-3 px-4 mb-4 rounded">
            <div className=" row justify-content-between">
                <div className="col-auto">
                    <h1 className="display-6 fs-4">Equipment {index + 1}</h1>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Item</Form.Label>
                        <Form.Control type="text" readOnly value={addedItems[index].name} />
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Quantity Approved</Form.Label>
                        <InputGroup className="mb-3">
                            <Form.Control
                                type="text"
                                name="qty"
                                readOnly
                                value={addedItems[index].qtyApproved}
                            />
                            <Form.Control
                                name="unit"
                                type="text"
                                readOnly
                                value={addedItems[index].units[0]}
                            />
                        </InputGroup>
                    </Form.Group>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Model</Form.Label>
                        <Form.Control
                            type="text"
                            name="cost"
                            value={modelQuery?.data?.name || ""}
                            readOnly
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <div className="row">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Unit Cost</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="cost"
                                    value={addedItems[index].cost}
                                    readOnly
                                />
                            </Form.Group>
                        </div>
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Total Cost</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="total"
                                    readOnly
                                    value={addedItems[index].qtyApproved * addedItems[index].cost}
                                />
                            </Form.Group>
                        </div>
                    </div>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Description</Form.Label>
                        <Form.Control
                            as="textarea"
                            name="spec"
                            rows={1}
                            value={addedItems[index].description}
                            readOnly
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Approve Remark</Form.Label>
                            <Form.Control
                                type="text"
                                name="approveRemark"
                                value={addedItems[index].approveRemark}
                                readOnly
                            />
                        </Form.Group>
                    </div>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Request Remark</Form.Label>
                        <Form.Control
                            type="text"
                            readOnly
                            value={addedItems[index].requestRemark}
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Send Remark</Form.Label>
                        <Form.Control
                            type="text"
                            name="sendRemark"
                            value={addedItems[index].sendRemark}
                            onChange={valueChanged}
                        />
                    </Form.Group>
                </div>
            </div>

            <div className="pb-3">
                <Form.Check
                    type="switch"
                    id="insert asset numbers"
                    label="Insert Asset Numbers"
                    checked={isSetAssets}
                    onChange={() =>
                        SetIsSetAssets((prev) => {
                            addedItems[index].isSetAssets = !prev;
                            return !prev;
                        })
                    }
                />
            </div>
            {isSetAssets &&
                addedItems[index].equipmentAssets.map((asset, assetIndex) => {
                    return (
                        <div className="row" key={assetIndex}>
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>{assetIndex + 1}. Asset Number</Form.Label>
                                    <Form.Select
                                        value={asset.equipmentAssetId}
                                        name={assetIndex}
                                        onChange={(e) => assetChanged(assetIndex, e)}
                                        required
                                    >
                                        <option value="">Select Asset Number</option>
                                        {modelQuery.data?.equipmentAssets
                                            ?.filter(
                                                (currentItem) =>
                                                    addedItems[index].equipmentAssets.filter(
                                                        (addedItem) =>
                                                            addedItem.equipmentAssetId ===
                                                                currentItem.equipmentAssetId.toString() &&
                                                            addedItem.equipmentAssetId !==
                                                                addedItems[index].equipmentAssets[
                                                                    assetIndex
                                                                ].equipmentAssetId
                                                    ).length === 0
                                            )
                                            ?.map((equipmentAsset) => (
                                                <option
                                                    key={equipmentAsset.equipmentAssetId}
                                                    value={equipmentAsset.equipmentAssetId}
                                                >
                                                    {equipmentAsset.assetNo}
                                                </option>
                                            ))}
                                    </Form.Select>
                                </Form.Group>
                            </div>
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Serial Number</Form.Label>
                                    <Form.Control
                                        type="text"
                                        value={
                                            modelQuery.data?.equipmentAssets?.find(
                                                (e) =>
                                                    e.equipmentAssetId.toString() ===
                                                    asset.equipmentAssetId
                                            )?.serialNo || ""
                                        }
                                        readOnly
                                    />
                                </Form.Group>
                            </div>
                        </div>
                    );
                })}
        </div>
    );
}

TransferSendEquipment.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    index: PropTypes.number.isRequired,
};

export default TransferSendEquipment;
