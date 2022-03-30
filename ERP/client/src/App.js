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
import ListBorrows from "./components/forms/borrow/ListBorrows";
import SingleBorrow from "./components/forms/borrow/SingleBorrow";
import NewBorrow from "./components/forms/borrow/NewBorrow";
import ListMaintenance from "./components/forms/maintenance/ListMaintenance";
import SingleMaintenance from "./components/forms/maintenance/SingleMaintenance";
import NewMaintenance from "./components/forms/maintenance/NewMaintenance";
import ListReturns from "./components/forms/return/ListReturns";
import NewReturn from "./components/forms/return/NewReturn";
import SingleReturn from "./components/forms/return/SingleReturn";
import TransferReport from "./components/forms/report/TransferReport";

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <ContextProviders>
                <Router>
                    <div
                        className="row m-0 flex-nowrap"
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
                                <Route path="report/transfer" element={<TransferReport />} />

                                <Route path="issue" element={<ListIssues />} />
                                <Route path="issue/:id" element={<SingleIssue />} />
                                <Route path="issue/new" element={<NewIssue />} />

                                <Route path="borrow" element={<ListBorrows />} />
                                <Route path="borrow/:id" element={<SingleBorrow />} />
                                <Route path="borrow/new" element={<NewBorrow />} />

                                <Route path="return" element={<ListReturns />} />
                                <Route path="return/:id" element={<SingleReturn />} />
                                <Route path="return/new" element={<NewReturn />} />

                                <Route path="maintenance" element={<ListMaintenance />} />
                                <Route path="maintenance/:id" element={<SingleMaintenance />} />
                                <Route path="maintenance/new" element={<NewMaintenance />} />

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
