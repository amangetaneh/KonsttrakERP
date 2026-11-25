import { clearSession, sessionExpiresAt } from "./auth/session";
import { useEffect, lazy, Suspense } from "react";

import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";
import "./AppPrint.css";
import Sidebar from "./components/layouts/Sidebar";

import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";

import { useAuth, useAuthUpdate } from "./contexts/AuthContext";

import ConnectionError from "./components/fragments/ConnectionError";

const MaterialTransfer = lazy(
  () => import("./components/forms/transfer/MaterialTransfer"),
);
const MaterialPurchase = lazy(
  () => import("./components/forms/purchase/MaterialPurchase"),
);
const EquipmentPurchase = lazy(
  () => import("./components/forms/purchase/EquipmentPurchase"),
);
const MaterialItem = lazy(() => import("./components/forms/item/MaterialItem"));
const ListMySiteReceives = lazy(
  () => import("./components/forms/receive/ListMySiteReceives"),
);
const MaterialBuy = lazy(() => import("./components/forms/buy/MaterialBuy"));
const EquipmentTransfer = lazy(
  () => import("./components/forms/transfer/EquipmentTransfer"),
);
const ListTransfers = lazy(
  () => import("./components/forms/transfer/ListTransfers"),
);
const ListBulkPurchases = lazy(
  () => import("./components/forms/bulkPurchase/ListBulkPurchases"),
);
const ListPurchases = lazy(
  () => import("./components/forms/purchase/ListPurchases"),
);
const ListBuys = lazy(() => import("./components/forms/buy/ListBuys"));
const SingleTransfer = lazy(
  () => import("./components/forms/transfer/SingleTransfer"),
);
const SingleBulkPurchase = lazy(
  () => import("./components/forms/bulkPurchase/SingleBulkPurchase"),
);
const SinglePurchase = lazy(
  () => import("./components/forms/purchase/SinglePurchase"),
);
const SingleBuy = lazy(() => import("./components/forms/buy/SingleBuy"));
const SingleReceive = lazy(
  () => import("./components/forms/receive/SingleReceive"),
);
const ListReceives = lazy(
  () => import("./components/forms/receive/ListReceives"),
);
const ListIssues = lazy(() => import("./components/forms/issue/ListIssues"));
const SingleIssue = lazy(() => import("./components/forms/issue/SingleIssue"));
const NewIssue = lazy(() => import("./components/forms/issue/NewIssue"));
const ListBorrows = lazy(() => import("./components/forms/borrow/ListBorrows"));
const SingleBorrow = lazy(
  () => import("./components/forms/borrow/SingleBorrow"),
);
const NewBorrow = lazy(() => import("./components/forms/borrow/NewBorrow"));
const ListMaintenance = lazy(
  () => import("./components/forms/maintenance/ListMaintenance"),
);
const SingleMaintenance = lazy(
  () => import("./components/forms/maintenance/SingleMaintenance"),
);
const NewMaintenance = lazy(
  () => import("./components/forms/maintenance/NewMaintenance"),
);
const ListReturns = lazy(() => import("./components/forms/return/ListReturns"));
const NewReturn = lazy(() => import("./components/forms/return/NewReturn"));
const SingleReturn = lazy(
  () => import("./components/forms/return/SingleReturn"),
);
const SignUp = lazy(() => import("./components/forms/authorization/Register"));
const Login = lazy(() => import("./components/forms/authorization/Login"));
const EditMaterial = lazy(() => import("./components/forms/item/EditMaterial"));
const EditEquipment = lazy(
  () => import("./components/forms/item/EditEquipment"),
);
const EditEquipmentModel = lazy(
  () => import("./components/forms/item/EditEquipmentModel"),
);
const ListMaterial = lazy(() => import("./components/forms/item/ListMaterial"));
const ListEquipment = lazy(
  () => import("./components/forms/item/ListEquipment"),
);
const ListEquipmentModel = lazy(
  () => import("./components/forms/item/ListEquipmentModel"),
);
const EquipmentItem = lazy(
  () => import("./components/forms/item/EquipmentItem"),
);
const EquipmentModel = lazy(
  () => import("./components/forms/item/EquipmentModel"),
);
const EquipmentCategory = lazy(
  () => import("./components/forms/item/EquipmentCategory"),
);
const ManageUser = lazy(() => import("./components/forms/user/ManageUser"));
const NewRole = lazy(() => import("./components/forms/role/NewRole"));
const Dashboard = lazy(() => import("./components/forms/Dashboard"));
const Report = lazy(() => import("./components/forms/report/Report"));
const ListEmployee = lazy(
  () => import("./components/forms/employee/ListEmployee"),
);
const ListRole = lazy(() => import("./components/forms/role/ListRole"));
const ListSite = lazy(() => import("./components/forms/site/ListSite"));
const EditSite = lazy(() => import("./components/forms/site/EditSite"));
const EditRole = lazy(() => import("./components/forms/role/EditRole"));
const SingleEmployee = lazy(
  () => import("./components/forms/employee/SingleEmployee"),
);
const NewSite = lazy(() => import("./components/forms/site/NewSite"));
const NewRent = lazy(() => import("./components/forms/rent/NewRent"));
const AdminNotification = lazy(
  () => import("./components/forms/AdminNotification"),
);
const MinimumStock = lazy(() => import("./components/forms/item/MinimumStock"));
const ComanyNamePrefix = lazy(
  () => import("./components/forms/misc/ComanyNamePrefix"),
);
const AssetNumberImport = lazy(
  () => import("./components/forms/item/AssetNumberImport"),
);
const UserGuide = lazy(() => import("./components/forms/help/UserGuide"));
const NewGarage = lazy(() => import("./components/forms/garage/NewGarage"));
const NewProduction = lazy(
  () => import("./components/forms/production/NewProduction"),
);
const FormBank = lazy(() => import("./components/forms/help/FormBank"));
const ManageDamages = lazy(
  () => import("./components/forms/item/ManageDamages"),
);
const StockReport = lazy(() => import("./components/forms/report/StockReport"));

const App = () => {
  const contextData = useAuth();
  const updateAuthContext = useAuthUpdate();

  useEffect(() => {
    if (!contextData.token) return;
    const timeout = setTimeout(
      () => {
        clearSession();
        updateAuthContext({ token: "", data: null, username: "" });
      },
      Math.max(0, sessionExpiresAt(contextData.token) - Date.now()),
    );
    return () => clearTimeout(timeout);
  }, [contextData.token, updateAuthContext]);

  return (
    <Suspense fallback={<div role="status">Loading ERP…</div>}>
      <Router>
        {contextData?.token ? (
          <div
            className="row m-0 flex-nowrap"
            style={{
              minHeight: "100vh",
              maxWidth: "100vw",
              minWidth: "900px",
            }}
          >
            <div className="col-auto p-0 d-print-none">
              <Sidebar />
            </div>
            <div className="col p-0">
              <Routes>
                <Route path="/login" element={<Navigate to={"/"} />} />
                <Route path="/register" element={<Navigate to={"/"} />} />

                <Route path="/" element={<Dashboard />} />

                <Route path="employee" element={<ListEmployee />} />

                <Route path="employee/:id" element={<SingleEmployee />} />

                <Route path="user/manage" element={<ManageUser />} />

                <Route path="role" element={<ListRole />} />
                <Route path="role/:id" element={<EditRole />} />
                <Route path="role/new" element={<NewRole />} />

                <Route path="site" element={<ListSite />} />
                <Route path="site/:id" element={<EditSite />} />
                <Route path="site/new" element={<NewSite />} />

                <Route path="report" element={<Report />} />
                <Route path="stock-report" element={<StockReport />} />

                <Route
                  path="item/import-asset-numbers"
                  element={<AssetNumberImport />}
                />
                <Route path="item/minimum-stock" element={<MinimumStock />} />
                <Route path="item/damage" element={<ManageDamages />} />
                <Route path="item/material" element={<ListMaterial />} />
                <Route
                  path="item/material/:itemId"
                  element={<EditMaterial />}
                />
                <Route path="item/equipment" element={<ListEquipment />} />
                <Route
                  path="item/equipment/:itemId"
                  element={<EditEquipment />}
                />
                <Route
                  path="item/equipment/model"
                  element={<ListEquipmentModel />}
                />
                <Route
                  path="item/equipment/model/:equipmentModelId"
                  element={<EditEquipmentModel />}
                />

                <Route path="item/material/new" element={<MaterialItem />} />
                <Route path="item/equipment/new" element={<EquipmentItem />} />
                <Route
                  path="item/equipment/model/new"
                  element={<EquipmentModel />}
                />
                <Route
                  path="item/equipment/category/new"
                  element={<EquipmentCategory />}
                />

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

                <Route
                  path="transfer/material"
                  element={<MaterialTransfer />}
                />
                <Route
                  path="transfer/equipment"
                  element={<EquipmentTransfer />}
                />
                <Route
                  path="transfer/in"
                  element={<ListTransfers title="Transfer In" type="in" />}
                />
                <Route
                  path="transfer/out"
                  element={<ListTransfers title="Transfer Out" type="out" />}
                />
                <Route path="transfer/:id" element={<SingleTransfer />} />

                <Route path="buy" element={<ListBuys />} />
                <Route path="buy/:id" element={<SingleBuy />} />
                <Route path="buy/item" element={<MaterialBuy />} />

                <Route path="bulkPurchase" element={<ListBulkPurchases />} />
                <Route
                  path="bulkPurchase/:id"
                  element={<SingleBulkPurchase />}
                />

                <Route path="purchase" element={<ListPurchases />} />
                <Route path="purchase/:id" element={<SinglePurchase />} />
                <Route
                  path="purchase/material"
                  element={<MaterialPurchase />}
                />
                <Route
                  path="purchase/equipment"
                  element={<EquipmentPurchase />}
                />

                <Route path="receive" element={<ListReceives />} />
                <Route path="receive/:id" element={<SingleReceive />} />
                <Route path="receive/item" element={<ListMySiteReceives />} />

                <Route path="notification" element={<AdminNotification />} />
                <Route path="misc/company" element={<ComanyNamePrefix />} />

                <Route path="help" element={<UserGuide />} />
                <Route path="form-bank" element={<FormBank />} />

                <Route path="rent/new" element={<NewRent />} />
                <Route path="garage/new" element={<NewGarage />} />
                <Route path="production/new" element={<NewProduction />} />

                <Route path="*" element={<ConnectionError status={404} />} />
              </Routes>
            </div>
          </div>
        ) : (
          <div
            className="row m-0"
            style={{
              minHeight: "100vh",
              maxWidth: "100vw",
              minWidth: "900px",
            }}
          >
            <div>
              <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/" element={<Navigate to={"/login"} />} />

                <Route path="/registration" element={<SignUp />} />

                <Route path="help" element={<UserGuide />} />

                <Route path="*" element={<ConnectionError status={404} />} />
              </Routes>
            </div>
          </div>
        )}
      </Router>
    </Suspense>
  );
};

export default App;
