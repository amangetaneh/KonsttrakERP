import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import Table from "../../fragments/Table";
import Header from "../../layouts/Header";
import { Container } from "react-bootstrap";
import axios from "axios";
import { statusBadge } from "../../../Helpers";
import PropTypes from "prop-types";

function ListTransfers({ title, type }) {
    const [transfers, setTransfers] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        axios.get("https://localhost:7067/api/transfer").then((res) => {
            var data = res.data;

            Object.keys(data).map((key, index) => {
                //fromat date
                data[key].date = new Date(data[key].date).toLocaleDateString();

                //add status badge
                data[key].status = statusBadge(data[key].status);

                //add links to each row
                data[key].open = (
                    <>
                        <Link
                            className="btn btn-teal me-2"
                            to={`/transfer/${data[key].id}`}
                        >
                            Open
                        </Link>
                    </>
                );

                return null;
            });

            setTransfers(data);
        });

        setIsLoading(false);
    }, []);

    const columns = React.useMemo(
        () => [
            {
                Header: "Transfer No.",
                accessor: "id", // accessor is the "key" in the data
            },
            {
                Header: "Date",
                accessor: "date", // accessor is the "key" in the data
            },
            {
                Header: "Sending Site",
                accessor: "sendStore.site.name", // accessor is the "key" in the data
            },
            {
                Header: "Receiving Site",
                accessor: "receiveStore.site.name", // accessor is the "key" in the data
            },
            {
                Header: "Status",
                accessor: "status", // accessor is the "key" in the data
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

            <Container className="my-3">
                <Table columns={columns} data={transfers} />
            </Container>
        </>
    );
}

ListTransfers.propTypes = {
    title: PropTypes.string.isRequired,
    type: PropTypes.string.isRequired,
};

export default ListTransfers;
