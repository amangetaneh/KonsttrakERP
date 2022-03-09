import { useEffect, useRef, useState, useMemo } from "react";
import { Form, Button, Container, Spinner } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import axios from "axios";
import Item from "../../../models/Item";
import TransferRequestConsumableItem from "./fragments/TransferRequestConsumableItem";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";

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

    useEffect(() => {
        //get sites
        axios.get("https://localhost:7067/api/site").then((res) => {
            setAllSites(res.data);
        });
        //get stores and select first ones
        axios.get("https://localhost:7067/api/store").then((res) => {
            setAllStores(res.data);
            setSendSite(res.data[0].id);
            setReceiveSite(res.data[0].id);
        });
        //get items
        axios.get("https://localhost:7067/api/item/consumable").then((res) => {
            setAllItems(res.data);

            setIsLoading(false);
        });
    }, []);

    useEffect(() => {
        //update send stores when send site changes
        setSendSiteStores(
            allStores.filter((store) => store.siteId === sendSite)
        );
    }, [sendSite]);

    useEffect(() => {
        //select first store when send stores change
        if (sendSiteStores.length > 0) setSendStore(sendSiteStores[0].id);
    }, [sendSiteStores]);

    useEffect(() => {
        //update receive stores when receive site changes
        setReceiveSiteStores(
            allStores.filter((store) => store.siteId == receiveSite)
        );
    }, [receiveSite]);

    useEffect(() => {
        //select first store when receive stores change
        if (sendSiteStores.length > 0) setReceiveStore(receiveSiteStores[0].id);
    }, [receiveSiteStores]);

    function requestTransfer(e) {
        e.preventDefault();

        let data = {
            sendStoreId: Number(sendStore),
            receiveStoreId: 1, ////////////////////////////////////////////////////////////////////////////////
            requestedById: 1,
            transferItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.id),
                qtyRequested: Number(item.qtyRequested),
                requestRemark: item.requestRemark,
            };

            data.transferItems.push(tempItem);
        }

        axios
            .post("https://localhost:7067/api/transfer/request", data)
            .then((res) => {
                navigate(`/transfer/${res.data}`);
            });
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
            <Form onSubmit={requestTransfer}>
                <div className="row mx-2">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Sending Site</Form.Label>
                            <Form.Select
                                value={sendSite}
                                onChange={(e) => setSendSite(e.target.value)}
                            >
                                {allSites.map((site) => (
                                    <option key={site.id} value={site.id}>
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
                                    <option key={store.id} value={store.id}>
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
                                    <option key={site.id} value={site.id}>
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
                                    <option key={store.id} value={store.id}>
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
