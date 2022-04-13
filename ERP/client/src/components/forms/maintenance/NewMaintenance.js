import { useEffect, useRef, useState } from "react";
import { Form, Button, Container } from "react-bootstrap";
import Item from "../../../models/Item";
import MaintenanceRequestEquipment from "./fragments/MaintenanceRequestEquipment";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "react-query";
import { fetchSites } from "../../../api/site";
import { requestMaintenance } from "../../../api/maintenance";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";

function NewMaintenance() {
    const navigate = useNavigate();
    const [sendSite, setSendSite] = useState(0);
    const [addedItems, setAddedItems] = useState([new Item()]);

    const [allSites, setAllSites] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isError, setIsError] = useState(false);

    const sitesQuery = useQuery("sites", fetchSites, {
        onSuccess: (data) => {
            setAllSites(data);
            setSendSite(data[0].siteId);
        },
    });

    useEffect(() => {
        setIsLoading(sitesQuery.isLoading);
    }, [sitesQuery.isLoading]);

    useEffect(() => {
        setIsError(sitesQuery.isError);
    }, [sitesQuery.isError]);

    const {
        isError: isSubmitError,
        isLoading: isSubmitLoading,
        error: submitError,
        mutate: submitMaintenanceRequest,
    } = useMutation(requestMaintenance, {
        onSuccess: (res) => navigate(`/maintenance/${res.data}`),
    });

    function submit(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        var data = {
            siteId: sendSite,
            requestedById: 1, ////////////////////////////////////////////////////////////////////////////////
            itemId: addedItems[0].itemId,
            equipmentModelId: addedItems[0].equipmentModelId,
            reason: addedItems[0].reason,
        };

        if (addedItems[0].equipmentAssets[0].equipmentAssetId)
            data.equipmentAssetId = addedItems[0].equipmentAssets[0].equipmentAssetId;

        submitMaintenanceRequest(data);
    }

    //scroll when Item is added
    var pageend = useRef(null);
    const [scroll, setScroll] = useState(false);

    useEffect(() => {
        if (scroll) {
            pageend.scrollIntoView();
            setScroll(false);
        }
    }, [scroll]);

    function addItem() {
        setAddedItems([...addedItems, new Item()]);
        setScroll(true);
    }

    if (isLoading) return <LoadingSpinner />;

    if (isError || isSubmitError)
        return (
            <ConnectionError
                status={submitError?.response?.status ?? sitesQuery?.error?.response?.status}
            />
        );

    return (
        <>
            <Header title="New Maintenance Request" />

            <Container className="my-3">
                <Form onSubmit={submit}>
                    <div className="row mx-2">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Issue Site</Form.Label>
                                <Form.Select
                                    value={sendSite}
                                    onChange={(e) => setSendSite(e.target.value)}
                                >
                                    {allSites.map((site) => (
                                        <option key={site.siteId} value={site.siteId}>
                                            {site.name}
                                        </option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                        </div>
                    </div>

                    {addedItems.map((item, index) => (
                        <MaintenanceRequestEquipment
                            key={item.key}
                            index={index}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                        />
                    ))}

                    <div className="row">
                        <div className="col d-grid">
                            <Button type="submit" className="btn-teal-dark">
                                Request Maintenance
                            </Button>
                        </div>
                    </div>
                </Form>
                <div
                    style={{ float: "left", clear: "both" }}
                    ref={(el) => {
                        pageend = el;
                    }}
                ></div>
            </Container>
        </>
    );
}

export default NewMaintenance;
