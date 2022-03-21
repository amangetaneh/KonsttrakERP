import { useEffect, useRef, useState } from "react";
import { Form, Button, Container } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import Item from "../../../models/Item";
import IssueRequestMaterial from "./fragments/IssueRequestMaterial";
import Header from "../../layouts/Header";
import { useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "react-query";
import { fetchSites } from "../../../api/site";
import { fetchStores } from "../../../api/store";
import { fetchMaterials } from "../../../api/item";
import { requestIssue } from "../../../api/issue";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";

function NewIssue() {
    const navigate = useNavigate();
    const [sendSite, setSendSite] = useState(0);
    const [sendStore, setSendStore] = useState(0);
   const [addedItems, setAddedItems] = useState([new Item()]);

    const [sendSiteStores, setSendSiteStores] = useState([]);
    const [allStores, setAllStores] = useState([]);
    const [allSites, setAllSites] = useState([]);
    const [allItems, setAllItems] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isError, setIsError] = useState(false);

    const sitesQuery = useQuery("sites", fetchSites, {
        onSuccess: (data) => setAllSites(data),
    });
    const storesQuery = useQuery("stores", fetchStores, {
        onSuccess: (data) => {
            setAllStores(data);
            setSendSite(data[0].storeId);
        },
    });
    const itemQuery = useQuery("material", fetchMaterials, {
        onSuccess: (data) => setAllItems(data),
    });

    useEffect(() => {
        setIsLoading(
            sitesQuery.isLoading || storesQuery.isLoading || itemQuery.isLoading
        );
    }, [sitesQuery.isLoading, storesQuery.isLoading, itemQuery.isLoading]);

    useEffect(() => {
        setIsError(
            sitesQuery.isError || storesQuery.isError || itemQuery.isError
        );
    }, [sitesQuery.isError, storesQuery.isError, itemQuery.isError]);

    useEffect(() => {
        //update send stores when send site changes
        setSendSiteStores(
            allStores.filter(
                (store) => store.siteId.toString() === sendSite.toString()
            )
        );
    }, [sendSite, allStores]);

    useEffect(() => {
        //select first store when send stores change
        if (sendSiteStores.length > 0) setSendStore(sendSiteStores[0].storeId);
    }, [sendSiteStores]);

    
    const {
        isError: isSubmitError,
        isLoading: isSubmitLoading,
        mutate: submitIssueRequest,
    } = useMutation(requestIssue, {
        onSuccess: (res) => navigate(`/issue/${res.data}`),
    });

    function submit(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        var data = {
            storeId: Number(sendStore),
            requestedById: 1, ////////////////////////////////////////////////////////////////////////////////
            issueItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                qtyRequested: Number(item.qtyRequested),
                requestRemark: item.requestRemark,
            };

            data.issueItems.push(tempItem);
        }

        submitIssueRequest(data);
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

    if (isError || isSubmitError) return <ConnectionError />;

    return (
        <>
            <Header title="New Issue Request" />

            <Container className="my-3">
                <Form onSubmit={submit}>
                    <div className="row mx-2">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Issue Site</Form.Label>
                                <Form.Select
                                    value={sendSite}
                                    onChange={(e) =>
                                        setSendSite(e.target.value)
                                    }
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
                                <Form.Label>Issue Store</Form.Label>
                                <Form.Select
                                    value={sendStore}
                                    onChange={(e) =>
                                        setSendStore(e.target.value)
                                    }
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


                    {addedItems.map((item, index) => (
                        <IssueRequestMaterial
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
                                Request Issue
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

export default NewIssue;
