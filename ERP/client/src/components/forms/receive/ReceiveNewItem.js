import { useEffect, useRef, useState, useMemo } from "react";
import { Form, Button, Container } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import axios from "axios";
import AllItems from "../../../models/AllItems";
import Item from "../../../models/Item";
import ReceiveRequestItem from "./RecieveItem";
import Header from "../../layouts/Header";
function ReceiveNewItem() {
    const [date, setDate] = useState("");
    const [receivingNo, setReceivingNo] = useState("");
    const [receivingStore, setreceivingStore] = useState("");
    const [deliveredFrom, setdeliveredFrom] = useState("");
    const [addedItems, setAddedItems] = useState([new Item()]);

    const [allItems, setAllItems] = useState([]);

    useEffect(() => {
        axios.get("https://localhost:7067/api/item").then((res) => {
            console.log(res.data);

            setAllItems(res.data);
        });
    },[]);
    
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
            <Header title="New Receive Request" />

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
                                <Form.Label>Receiving Number</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={receivingNo}
                                    onChange={(e) => {
                                        setReceivingNo(e.target.value);
                                    }}
                                />
                            </Form.Group>
                        </div>
                    </div>
                    <div className="row mx-2">

                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Receiving Store</Form.Label>
                                <Form.Select
                                    value={receivingStore}
                                    onChange={(e) =>
                                        setreceivingStore(e.target.value)
                                    }
                                >
                                    <option> Select Store </option>
                                    <option value="1">Store 1</option>
                                </Form.Select>
                            </Form.Group>
                        </div>
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Delivered From</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={deliveredFrom}
                                    onChange={(e) =>
                                        setdeliveredFrom(e.target.value)
                                    }
                                /> 
                                </Form.Group>
                        </div>
                    </div>
                   

                    {addedItems.map((item, index) => (
                        <ReceiveRequestItem
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
                                Request Receive
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

export default ReceiveNewItem;
