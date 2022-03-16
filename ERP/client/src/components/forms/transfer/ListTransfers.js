import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { FaFolderOpen } from "react-icons/fa";
import Table from "../../fragments/Table";
import Header from "../../layouts/Header";
import { Container, Spinner } from "react-bootstrap";
import TransferStatusBadge from "./fragments/TransferStatusBadge";
import PropTypes from "prop-types";
import { useQuery } from "react-query";
import { fetchTransfers } from "../../../api/transfer";

function ListTransfers({ title, type }) {
    const [transfers, setTransfers] = useState([]);

    var { data, isLoading } = useQuery("transfers", fetchTransfers);

    useEffect(() => {
        if (data === undefined) return;

        Object.keys(data).map((key, index) => {
            //format date
            data[key].requestDate = new Date(
                data[key].requestDate
            ).toLocaleString();

            //add status badge
            data[key].statusBadge = TransferStatusBadge(data[key].status);

            //add links to each row
            data[key].open = (
                <>
                    <Link
                        className="btn btn-teal me-2"
                        to={`/transfer/${data[key].id}`}
                    >
                        <FaFolderOpen className="me-1 mb-1"/>
                        Open
                    </Link>
                </>
            );

            return null;
        });

        setTransfers(data);
    }, [data]);

    const columns = React.useMemo(
        () => [
            {
                Header: "Transfer No.",
                accessor: "transferId", // accessor is the "key" in the data
            },
            {
                Header: "Requested On",
                accessor: "requestDate", // accessor is the "key" in the data
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
                accessor: "statusBadge", // accessor is the "key" in the data
                sortType: (rowA, rowB) =>
                    rowA.original.status > rowB.original.status ? 1 : -1,
            },
            {
                Header: "",
                accessor: "open", // accessor is the "key" in the data
                disableSortBy: true,
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
                    <Table columns={columns} data={transfers} />
                </Container>
            )}
        </>
    );
}

ListTransfers.propTypes = {
    title: PropTypes.string.isRequired,
    type: PropTypes.string.isRequired,
};

export default ListTransfers;
