import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import Table from "../../fragments/Table";
import Header from "../../layouts/Header";
import { Container, Spinner } from "react-bootstrap";
import axios from "axios";
import PurchaseStatusBadge from "./fragments/PurchaseStatusBadge";
import PropTypes from "prop-types";

function ListPurchases({ title, type }) {
    const [purchases, setPurchases] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        axios.get("https://localhost:7067/api/purchase").then((res) => {
            var data = res.data;

            Object.keys(data).map((key, index) => {
                //format date
                data[key].requestDate = new Date(
                    data[key].requestDate
                ).toLocaleDateString();
                data[key].approveDate = new Date(
                    data[key].approveDate
                ).toLocaleDateString();

                //add status badge
                data[key].status = PurchaseStatusBadge(data[key].status);

                //add links to each row
                data[key].open = (
                    <>
                        <Link
                            className="btn btn-teal me-2"
                            to={`/purchase/${data[key].id}`}
                        >
                            Open
                        </Link>
                    </>
                );

                return null;
            });

            setPurchases(data);
            setIsLoading(false);
        });
    }, []);

    const columns = React.useMemo(
        () => [
            {
                Header: "Request No.",
                accessor: "id", // accessor is the "key" in the data
            },
            {
                Header: "Date",
                accessor: "requestDate", // accessor is the "key" in the data
            },
            {
                Header: "Request Store",
                accessor: "site.name", // accessor is the "key" in the data
            },
            {
                Header: "Status",
                accessor: "status", // accessor is the "key" in the data
            },
            {
                Header: "Last Status",
                accessor: "approveDate", // accessor is the "key" in the data
            },
            {
                Header: "",
                accessor: "open", // accessor is the "key" in the data
            },
        ],
        []
    );

    return (
        <>
            <Header title={title} />

            {isLoading ? (
                <div className="d-flex justify-content-center align-content-center mt-4 pt-4">
                    <Spinner animation="grow" className="bg-teal" />
                </div>
            ) : (
                <Container className="my-3">
                    <Table columns={columns} data={purchases} />
                </Container>
            )}
        </>
    );
}
/*
ListPurchases.propTypes = {
    title: PropTypes.string.isRequired,
    type: PropTypes.string.isRequired,
};
*/

export default ListPurchases;
