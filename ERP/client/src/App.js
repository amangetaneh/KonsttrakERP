import React, { Component } from "react";
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import ContextProviders from "./contexts/ContextProviders";
import Sidebar from "./components/layouts/Sidebar";
import MaterialTransfer from "./components/forms/transfer/MaterialTransfer";
import EquipmentTransfer from "./components/forms/transfer/EquipmentTransfer";
import ListTransfers from "./components/forms/transfer/ListTransfers";
import PurchaseNewItem from "./components/forms/purchase/PurchaseNewItem";
import ListPurchases from "./components/forms/purchase/ListPurchases";
import SingleTransfer from "./components/forms/transfer/SingleTransfer";
import ReceiveNewItem from "./components/forms/receive/ReceiveNewItem";
import ListReceives from "./components/forms/receive/ListReceives";

import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import ListIssues from "./components/forms/issue/ListIssues";
import SingleIssue from "./components/forms/issue/SingleIssue";
import NewIssue from "./components/forms/issue/NewIssue";

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <ContextProviders>
                <Router>
                    <div
                        className="row m-0"
                        style={{
                            minHeight: "100vh",
                            maxWidth: "100vw",
                            minWidth: "900px",
                        }}
                    >
                        <div className="col-auto p-0">
                            <Sidebar />
                        </div>
                        <div className="col p-0">
                            <Routes>
                                <Route path="issue" element={<ListIssues />} />
                                <Route path="issue/:id" element={<SingleIssue />} />
                                <Route path="issue/new" element={<NewIssue />} />

                                <Route path="transfer/material" element={<MaterialTransfer />} />
                                <Route path="transfer/equipment" element={<EquipmentTransfer />} />
                                <Route
                                    path="transfer/in"
                                    element={<ListTransfers title="Transfer In" type="in" />}
                                />
                                <Route path="transfer/:id" element={<SingleTransfer />} />

                                <Route path="purchase/new" element={<PurchaseNewItem />} />
                                <Route path="purchase/list" element={<ListPurchases />} />

                                <Route path="receive/new" element={<ReceiveNewItem />} />
                                <Route path="receive/list" element={<ListReceives />} />
                            </Routes>
                        </div>
                    </div>
                </Router>
            </ContextProviders>
        );
    }
}
