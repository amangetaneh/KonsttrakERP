import { useEffect, useState, useMemo } from "react";
import { useParams } from "react-router-dom";
import { Form, Container, Alert } from "react-bootstrap";
import Item from "../../../models/Item";
import Header from "../../layouts/Header";
import MaintenanceStatusBadge from "./fragments/MaintenanceStatusBadge";
import { MAINTENANCESTATUS, ITEMTYPE } from "../../../Constants";
import ApproveMaintenance from "./ApproveMaintenance";
import FixMaintenance from "./FixMaintenance";
import ViewMaintenance from "./ViewMaintenance.js";
import DeclinedMaintenance from "./DeclinedMaintenance";
import { useQuery } from "react-query";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import { fetchMaintenance } from "../../../api/maintenance";

function SingleMaintenance() {
    const { id } = useParams();
    const [maintenance, setMaintenance] = useState({});
    const [addedItems, setAddedItems] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    var query = useQuery(["maintenance", id], () => fetchMaintenance(id));

    useEffect(() => {
        if (query.data === undefined) return;
        setMaintenance(query.data);
        let items = [];
        let itemObj = new Item();

        itemObj.itemId = query.data.itemId;
        itemObj.equipmentModelId = query.data.equipmentModelId;
        itemObj.equipmentAssetId = query.data.equipmentAssetId;
        itemObj.name = query.data.item.name;
        itemObj.cost = query.data.cost;
        itemObj.description = query.data.item.equipment.description;

        itemObj.reason = query.data.reason;

        itemObj.equipmentAssets = [query.data.equipmentAsset];

        if (
            query.data.status >= MAINTENANCESTATUS.APPROVED ||
            query.data.status === MAINTENANCESTATUS.DECLINED
        )
            itemObj.approveRemark = query.data.approveRemark;

        if (query.data.status >= MAINTENANCESTATUS.FIXED) itemObj.fixRemark = query.data.fixRemark;

        items.push(itemObj);

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
                    labelL="Maintenance Number"
                    valueL={maintenance.maintenanceId}
                    labelR="Status"
                    valueR={MaintenanceStatusBadge(maintenance.status)}
                />

                <div className="row mx-2">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-1 mt-1" style={{ width: "12.499999995%" }}>
                                    <Form.Label>Site</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control
                                        type="text"
                                        readOnly
                                        value={maintenance.site.name}
                                    />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                </div>

                <FormRow
                    labelL="Requested By"
                    valueL={`${maintenance.requestedBy.fName} ${maintenance.requestedBy.mName}`}
                    labelR="Requested On"
                    valueR={new Date(maintenance.requestDate).toLocaleString()}
                />

                {maintenance.status >= MAINTENANCESTATUS.APPROVED && (
                    <>
                        <FormRow
                            labelL="Approved By"
                            valueL={
                                maintenance.approvedBy
                                    ? `${maintenance.approvedBy.fName} ${maintenance.approvedBy.mName}`
                                    : ""
                            }
                            labelR="Approved On"
                            valueR={
                                maintenance.approveDate
                                    ? new Date(maintenance.approveDate).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}

                {maintenance.status >= MAINTENANCESTATUS.FIXED && (
                    <>
                        <FormRow
                            labelL="Fixed By"
                            valueL={
                                maintenance.fixedBy
                                    ? `${maintenance.fixedBy.fName} ${maintenance.fixedBy.mName}`
                                    : ""
                            }
                            labelR="Fixed On"
                            valueR={
                                maintenance.fixDate
                                    ? new Date(maintenance.fixDate).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}
            </Form>
        );
    }

    const titles = useMemo(
        () => [
            "Declined Maintenance Request",
            "Approve Maintenance Request",
            "Fix Maintenance Request",
            "Completed Maintenance Request",
        ],
        []
    );

    if (isLoading) return <LoadingSpinner />;

    if (query.isError) return <ConnectionError />;

    return (
        <>
            <Header title={titles[maintenance.status]} />

            <Container className="my-3">
                <>
                    <TopForm />

                    {maintenance.status === MAINTENANCESTATUS.DECLINED && (
                        <DeclinedMaintenance
                            addedItems={addedItems}
                            FormRow={FormRow}
                            maintenance={maintenance}
                        />
                    )}

                    {maintenance.status === MAINTENANCESTATUS.REQUESTED && (
                        <ApproveMaintenance
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            maintenance={maintenance}
                        />
                    )}

                    {maintenance.status === MAINTENANCESTATUS.APPROVED && (
                        <FixMaintenance
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            maintenance={maintenance}
                        />
                    )}

                    {maintenance.status === MAINTENANCESTATUS.FIXED && (
                        <ViewMaintenance addedItems={addedItems} />
                    )}
                </>
            </Container>
        </>
    );
}

export default SingleMaintenance;
