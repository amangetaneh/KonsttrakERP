import { useEffect, useRef, useState, useMemo } from "react";
import { Form, Button, Container, Spinner } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import axios from "axios";
import Item from "../../../models/Item";
import TransferRequestConsumableItem from "./fragments/TransferRequestConsumableItem";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "react-query";
import { fetchSites } from "../../../api/site";
import { fetchStores } from "../../../api/store";
import { fetchConsumableItems } from "../../../api/item";
import { requestTransfer } from "../../../api/transfer";

function NewTransfer() {
    const navigate = useNavigate();
    const [sendSite, setSendSite] = useState(0);
    const [sendStore, setSendStore] = useState(0);
    const [receiveSite, setReceiveSite] = useState(0);
    const [receiveStore, setReceiveStore] = useState(0);
    const [addedItems, setAddedItems] = useState([new Item()]);

    const [sendSiteStores, setSendSiteStores] = useState([]);
    const [receiveSiteStores, setReceiveSiteStores] = useState([]);
    const [allStores, setAllStores] = useState([]);
    const [allSites, setAllSites] = useState([]);
    const [allItems, setAllItems] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    const sitesQuery = useQuery("sites", fetchSites, {
        onSuccess: (data) => setAllSites(data),
    });
    const storesQuery = useQuery("stores", fetchStores, {
        onSuccess: (data) => {
            setAllStores(data);
            setSendSite(data[0].storeId);
            setReceiveSite(data[0].siteId);
        },
    });
    const itemQuery = useQuery("consumableItems", fetchConsumableItems, {
        onSuccess: (data) => setAllItems(data),
    });

    useEffect(() => {
        setIsLoading(
            sitesQuery.isLoading || storesQuery.isLoading || itemQuery.isLoading
        );
    }, [sitesQuery.isLoading, storesQuery.isLoading, itemQuery.isLoading]);

    useEffect(() => {
        //update send stores when send site changes
        setSendSiteStores(
            allStores.filter((store) => store.siteId == sendSite)
        );
    }, [sendSite]);

    useEffect(() => {
        //select first store when send stores change
        if (sendSiteStores.length > 0) setSendStore(sendSiteStores[0].storeId);
    }, [sendSiteStores]);

    useEffect(() => {
        //update receive stores when receive site changes
        setReceiveSiteStores(
            allStores.filter((store) => store.siteId == receiveSite)
        );
    }, [receiveSite]);

    useEffect(() => {
        //select first store when receive stores change
        if (receiveSiteStores.length > 0)
            setReceiveStore(receiveSiteStores[0].storeId);
    }, [receiveSiteStores]);

    const { isLoading: isSubmitting, mutate: submitTransferRequest } =
        useMutation(requestTransfer, {
            onSuccess: (res) => navigate(`/transfer/${res.data}`),
        });
    let data;
    function submitTransferRequestForm(e) {
        e.preventDefault();

        data = {
            sendStoreId: Number(sendStore),
            receiveStoreId: Number(receiveStore),
            requestedById: 1, ////////////////////////////////////////////////////////////////////////////////
            transferItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                qtyRequested: Number(item.qtyRequested),
                requestRemark: item.requestRemark,
            };

            data.transferItems.push(tempItem);
        }

        submitTransferRequest(data);
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

    function InputForm() {
        return (
            <Form onSubmit={submitTransferRequestForm}>
                <div className="row mx-2">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Sending Site</Form.Label>
                            <Form.Select
                                value={sendSite}
                                onChange={(e) => setSendSite(e.target.value)}
                            >
                                {allSites.map((site) => (
                                    <option
                                        key={site.siteId}
                                        value={site.siteId}
                                    >
                                        {site.name}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Sending Store</Form.Label>
                            <Form.Select
                                value={sendStore}
                                onChange={(e) => setSendStore(e.target.value)}
                            >
                                {sendSiteStores.map((store) => (
                                    <option
                                        key={store.storeId}
                                        value={store.storeId}
                                    >
                                        {store.name}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                    </div>
                </div>

                <div className="row mx-2">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Receiving Site</Form.Label>
                            <Form.Select
                                value={receiveSite}
                                onChange={(e) => setReceiveSite(e.target.value)}
                            >
                                {allSites.map((site) => (
                                    <option
                                        key={site.siteId}
                                        value={site.siteId}
                                    >
                                        {site.name}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Receiving Store</Form.Label>
                            <Form.Select
                                value={receiveStore}
                                onChange={(e) =>
                                    setReceiveStore(e.target.value)
                                }
                            >
                                {receiveSiteStores.map((store) => (
                                    <option
                                        key={store.storeId}
                                        value={store.storeId}
                                    >
                                        {store.name}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                    </div>
                </div>

                {addedItems.map((item, index) => (
                    <TransferRequestConsumableItem
                        key={index}
                        index={index}
                        allItems={allItems}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                    />
                ))}

                <div className="row">
                    <div className="col-9 d-grid">
                        <Button type="submit" className="btn-teal-dark">
                            Request Transfer
                        </Button>
                    </div>
                    <div className="col-3 d-grid">
                        <Button className="btn-teal" onClick={addItem}>
                            <FaPlus className="me-2 mb-1" /> Add Item
                        </Button>
                    </div>
                </div>
            </Form>
        );
    }

    return (
        <>
            <Header title="New Transfer Request" />

            {isLoading ? (
                <div className="d-flex justify-content-center align-content-center mt-4 pt-4">
                    <Spinner animation="grow" className="bg-teal" />
                </div>
            ) : (
                <Container className="my-3">
                    <InputForm />
                    <div
                        style={{ float: "left", clear: "both" }}
                        ref={(el) => {
                            pageend = el;
                        }}
                    ></div>
                </Container>
            )}
        </>
    );
}

export default NewTransfer;
