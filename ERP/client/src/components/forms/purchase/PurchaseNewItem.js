import { useEffect, useRef, useState, useMemo } from "react";
import { Form, Button, Container } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import AllItems from "../../../models/AllItems";
import Item from "../../../models/Item";
import PurchaseRequestItem from "./PurchaseRequestItem";
import Header from "../../layouts/Header";
function PurchaseNewItem() {
    const [date, setDate] = useState("");
    const [requestNo, setRequestNo] = useState("");
    // const [requestSite, setrequestSite] = useState("");
    const [sendStore, setSendStore] = useState("");
    const [addedItems, setAddedItems] = useState([new Item()]);

    const allItems = useMemo(() => AllItems(), []);

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
            <Header title="New Purchase Request" />

            <Container className="my-3">
                <Form>
                    <div className="row mx-2">
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
                                <Form.Label>Request Number</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={requestNo}
                                    onChange={(e) => {
                                        setRequestNo(e.target.value);
                                    }}
                                />
                            </Form.Group>
                        </div>
                    </div>
                    <div className="row mx-2">

                        {/* <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Request Site</Form.Label>
                                <Form.Select
                                    value={requestSite}
                                    onChange={(e) =>
                                        setrequestSite(e.target.value)
                                    }
                                >
                                    <option> Select Site </option>
                                    <option value="1">Site 1</option>
                                </Form.Select>
                            </Form.Group>
                        </div> */}
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Request Store</Form.Label>
                                <Form.Select
                                    value={sendStore}
                                    onChange={(e) =>
                                        setSendStore(e.target.value)
                                    }
                                >
                                    <option> Select Store </option>
                                    <option value="1">Store 1</option>
                                </Form.Select>
                            </Form.Group>
                        </div>
                    </div>
                   

                    {addedItems.map((item, index) => (
                        <PurchaseRequestItem
                            key={index}
                            index={index}
                            allItems={allItems}
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                        />
                    ))}

                    <div className="row">
                        <div className="col-9 d-grid">
                            <Button className="bg-teal">
                                Request Purchase
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

export default PurchaseNewItem;
