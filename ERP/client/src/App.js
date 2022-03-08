import React, { Component } from "react"
import "./App.css"
import "bootstrap/dist/css/bootstrap.min.css"
import Sidebar from "./components/layouts/Sidebar"
import NewTransfer from "./components/forms/transfer/NewTransfer"
import ListTransfers from "./components/forms/transfer/ListTransfers"
import PurchaseNewItem from "./components/forms/purchase/PurchaseNewItem"
import ListPurchases from "./components/forms/purchase/ListPurchases"
import SingleTransfer from "./components/forms/transfer/SingleTransfer"
import ReceiveNewItem from "./components/forms/receive/ReceiveNewItem"

import { BrowserRouter as Router, Routes, Route } from "react-router-dom"

export default class App extends Component {
  static displayName = App.name

  render() {
    return (
      <Router>
        <div
          className="row m-0"
          style={{ minHeight: "100vh", maxWidth: "100vw" }}
        >
          <div className="col-auto p-0">
            <Sidebar />
          </div>
          <div className="col p-0">
            <Routes>
              <Route path="transfer/new" element={<NewTransfer />} />
              <Route path="transfer/in" element={<ListTransfers title="Transfer In Requests" type="in" />} />
              <Route path="transfer/:id" element={<SingleTransfer />} />

              <Route path="purchase/new" element={<PurchaseNewItem />} />
              <Route path="purchase/list" element={<ListPurchases />} />

              <Route path="receive/new" element={<ReceiveNewItem />} />
            </Routes>
          </div>    
        </div>
      </Router>
    )
  }
}
