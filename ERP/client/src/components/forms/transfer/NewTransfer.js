import { useEffect, useRef, useState, useMemo } from "react";
import { Form, Button, Container } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import axios from "axios";
import Item from "../../../models/Item";
import TransferRequestConsumableItem from "../../fragments/TransferRequestConsumableItem";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";
function NewTransfer() {
    const navigate = useNavigate();
    const [date, setDate] = useState("");
    const [transferNo, setTransferNo] = useState("");
    const [sendSite, setSendSite] = useState(0);
    const [sendStore, setSendStore] = useState(0);
    const [addedItems, setAddedItems] = useState([new Item()]);

    const [siteStores, setSiteStores] = useState([]);
    const [allStores, setAllStores] = useState([]);
    const [allSites, setAllSites] = useState([]);
    const [allItems, setAllItems] = useState([]);

    useEffect(() => {
        axios.get("https://localhost:7067/api/site").then((res) => {
            setAllSites(res.data);
        });

        axios.get("https://localhost:7067/api/store").then((res) => {
            setAllStores(res.data);
            setSendSite(res.data[0].id);
        });

        axios.get("https://localhost:7067/api/item").then((res) => {
            setAllItems(res.data);
        });
    }, []);

    useEffect(() => {
        setSiteStores(allStores.filter((store) => store.siteId == sendSite));
    }, [sendSite]);

    useEffect(() => {
        if (siteStores.length > 0) setSendStore(siteStores[0].id);
    }, [siteStores]);

    function requestTransfer(e) {
        e.preventDefault();

        let data = {
            sendStoreId: Number(sendStore),
            receiveStoreId: 1, ////////////////////////////////////////////////////////////////////////////////
            requestedById: 1,
            transferItems: [],
        };

        for (let item of addedItems) {
            let temp = {
                itemId: Number(item.id),
                qtyRequested: Number(item.qtyRequested),
                requestRemark: item.requestRemark,
            };

            data.transferItems.push(temp);
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

    return (
        <>
            <Header title="New Transfer Request" />

            <Container className="my-3">
                <Form onSubmit={requestTransfer}>
                    <div className="row mx-2 border-danger border">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Date</Form.Label>
                                <Form.Control
                                    type="date"
                                    value={date}
                                    onChange={(e) => {
                                        setDate(e.target.value);
                                    }}
                                />
                            </Form.Group>
                        </div>
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Transfer Number</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={transferNo}
                                    onChange={(e) => {
                                        setTransferNo(e.target.value);
                                    }}
                                />
                            </Form.Group>
                        </div>
                    </div>
                    <div className="row mx-2">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Sending Site</Form.Label>
                                <Form.Select
                                    value={sendSite}
                                    onChange={(e) =>
                                        setSendSite(e.target.value)
                                    }
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
                                    onChange={(e) =>
                                        setSendStore(e.target.value)
                                    }
                                >
                                    {siteStores.map((store) => (
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
                            <Button type="submit" className="bg-teal">
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

export default NewTransfer;
