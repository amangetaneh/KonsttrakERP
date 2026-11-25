import { useState } from "react";
import {
  Sidebar as SidebarContainer,
  Menu,
  MenuItem,
  SubMenu,
} from "react-pro-sidebar";
import { GoGear } from "react-icons/go";
import { FaHome, FaList, FaBars, FaUser, FaChartBar } from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom";
import { useAuth, useAuthUpdate } from "../../contexts/AuthContext";

function Sidebar() {
  const navigate = useNavigate();
  const updateAuthContext = useAuthUpdate();

  const { data } = useAuth();

  const [collapsed, setCollapsed] = useState(false);

  return (
    <>
      <SidebarContainer
        className="text-light sidebar-decrease-padding"
        collapsed={collapsed}
        width="280px"
        rootStyles={{
          ".ps-sidebar-container": {
            display: "flex",
            flexDirection: "column",
            backgroundColor: "#1d1d1d",
          },
          ".ps-submenu-content": { backgroundColor: "#1d1d1d" },
          ".ps-menu-button:hover": { backgroundColor: "#374343" },
        }}
        style={{ height: "100vh", position: "sticky", top: 0 }}
      >
        <div className="p-1 text-nowrap pro-sidebar-header">
          <button
            type="button"
            aria-label="Toggle navigation"
            aria-expanded={!collapsed}
            className="btn text-light ms-3 me-2"
            onClick={() => setCollapsed(!collapsed)}
          >
            <FaBars className="fs-5" />
          </button>
          {!collapsed && "ERP"}
        </div>
        <div style={{ flex: 1, overflowY: "auto" }}>
          <Menu>
            <MenuItem
              className=""
              icon={<FaHome className="fs-5" />}
              component={<Link to="/" />}
            >
              Home
            </MenuItem>

            <SubMenu label="Setting" icon={<GoGear className="fs-5" />}>
              {data.userRole.canEditUser && (
                <SubMenu label="Employee">
                  <MenuItem component={<Link to="/employee" />}>
                    Manage Employees
                  </MenuItem>
                </SubMenu>
              )}
              <SubMenu label="Role">
                <MenuItem component={<Link to="/role/new" />}>
                  Add a new Role
                </MenuItem>
                <MenuItem component={<Link to="/role" />}>Edit Role</MenuItem>
              </SubMenu>
              <SubMenu label="Site">
                <MenuItem component={<Link to="/site/new" />}>
                  Add a new Site
                </MenuItem>
                <MenuItem component={<Link to="/site" />}>Edit Site</MenuItem>
              </SubMenu>
              <SubMenu label="Stock">
                <MenuItem component={<Link to="/item/material/new" />}>
                  Add a new Material
                </MenuItem>
                <MenuItem component={<Link to="/item/material" />}>
                  Edit Material
                </MenuItem>
                <SubMenu label="Equipment">
                  <MenuItem component={<Link to="/item/equipment/new" />}>
                    Add a new Equipment
                  </MenuItem>
                  <MenuItem component={<Link to="/item/equipment/model/new" />}>
                    Add a new Model
                  </MenuItem>
                  <MenuItem
                    component={<Link to="/item/equipment/category/new" />}
                  >
                    Add a new Category{" "}
                  </MenuItem>
                  <MenuItem component={<Link to="/item/equipment" />}>
                    Edit Equipment
                  </MenuItem>
                  <MenuItem component={<Link to="/item/equipment/model" />}>
                    Edit Model
                  </MenuItem>
                </SubMenu>

                <MenuItem component={<Link to="/item/minimum-stock" />}>
                  Minimum Stock
                </MenuItem>
                <MenuItem component={<Link to="/item/damage" />}>
                  Manage Damages
                </MenuItem>
                <MenuItem component={<Link to="/item/import-asset-numbers" />}>
                  Import Asset Numbers
                </MenuItem>
              </SubMenu>

              <MenuItem component={<Link to="/notification" />}>
                Notifications
              </MenuItem>
              <MenuItem component={<Link to="/misc/company" />}>
                Company Name / Prefix
              </MenuItem>
            </SubMenu>

            <SubMenu
              label="Property Management"
              icon={<FaList className="fs-5" />}
              defaultOpen={true}
            >
              <SubMenu label="Buy">
                {data.userRole?.canRequestBuy && (
                  <MenuItem component={<Link to="/buy/item" />}>
                    Buy an Item
                  </MenuItem>
                )}
                <MenuItem component={<Link to="/buy" />}>Buy Requests</MenuItem>
              </SubMenu>

              {data.userRole.canViewPurchase && (
                <SubMenu label="Purchase">
                  {data.userRole?.canRequestPurchase && (
                    <MenuItem component={<Link to="/purchase/material" />}>
                      Purchase Material
                    </MenuItem>
                  )}
                  {data.userRole?.canRequestPurchase && (
                    <MenuItem component={<Link to="/purchase/equipment" />}>
                      Purchase Equipment
                    </MenuItem>
                  )}

                  <MenuItem component={<Link to="/purchase" />}>
                    List Purchase Requests
                  </MenuItem>
                  {data.userRole?.canRequestBulkPurchase && (
                    <MenuItem component={<Link to="/bulkPurchase" />}>
                      Bulk Purchase
                    </MenuItem>
                  )}
                </SubMenu>
              )}

              {data.userRole.canViewReceive && (
                <SubMenu label="Receive">
                  {data.userRole?.canReceive && (
                    <MenuItem component={<Link to="/receive/item" />}>
                      Receive an Item
                    </MenuItem>
                  )}

                  <MenuItem component={<Link to="/receive" />}>
                    List Received Items
                  </MenuItem>
                </SubMenu>
              )}

              {data.userRole.canViewTransfer && (
                <SubMenu label="Transfer">
                  {data.userRole?.canRequestTransfer && (
                    <>
                      <MenuItem component={<Link to="/transfer/material" />}>
                        New Material Transfer
                      </MenuItem>
                      <MenuItem component={<Link to="/transfer/equipment" />}>
                        New Equipment Transfer
                      </MenuItem>
                    </>
                  )}
                  <MenuItem component={<Link to="/transfer/in" />}>
                    Transfer In
                  </MenuItem>
                  <MenuItem component={<Link to="/transfer/out" />}>
                    Transfer Out
                  </MenuItem>
                </SubMenu>
              )}

              {data.userRole.canViewIssue && (
                <SubMenu label="Issue">
                  {data.userRole.canRequestIssue && (
                    <MenuItem component={<Link to="/issue/new" />}>
                      New Issue Request
                    </MenuItem>
                  )}
                  <MenuItem component={<Link to="/issue" />}>
                    Issue Requests
                  </MenuItem>
                </SubMenu>
              )}

              {data.userRole.canViewBorrow && (
                <SubMenu label="Borrow">
                  {data.userRole.canRequestBorrow && (
                    <MenuItem component={<Link to="/borrow/new" />}>
                      New Borrow Request
                    </MenuItem>
                  )}
                  <MenuItem component={<Link to="/borrow" />}>
                    Borrow Requests
                  </MenuItem>

                  {data.userRole.canReturnBorrow && (
                    <MenuItem component={<Link to="/return/new" />}>
                      Return Assets
                    </MenuItem>
                  )}

                  <MenuItem component={<Link to="/return" />}>
                    Returned Assets
                  </MenuItem>
                </SubMenu>
              )}

              {data.userRole.canViewMaintenance && (
                <SubMenu label="Maintenance">
                  {data.userRole.canRequestMaintenance && (
                    <MenuItem component={<Link to="/maintenance/new" />}>
                      New Maintenance Request
                    </MenuItem>
                  )}
                  <MenuItem component={<Link to="/maintenance" />}>
                    Maintenance Requests
                  </MenuItem>
                </SubMenu>
              )}
              {data.userRole.isAdmin && (
                <>
                  <SubMenu label="Rent">
                    <MenuItem component={<Link to="/rent/new" />}>
                      New Rent Request
                    </MenuItem>
                  </SubMenu>

                  <SubMenu label="Garage">
                    <MenuItem component={<Link to="/garage/new" />}>
                      New Maintenance Request
                    </MenuItem>
                  </SubMenu>
                  <SubMenu label="Equipment Production">
                    <MenuItem component={<Link to="/production/new" />}>
                      New Production Request
                    </MenuItem>
                  </SubMenu>
                </>
              )}

              {!data.userRole.isAdmin && data.userRole.canGetStockReport && (
                <MenuItem component={<Link to="/stock-report" />}>
                  Stock Report
                </MenuItem>
              )}
            </SubMenu>

            {(data.userRole.isAdmin || data.userRole.isFinance) && (
              <MenuItem
                icon={<FaChartBar className="fs-5" />}
                component={<Link to="/report" />}
              >
                Reports
              </MenuItem>
            )}
          </Menu>
        </div>
        <div>
          <Menu>
            <SubMenu label={data.username} icon={<FaUser className="fs-5" />}>
              <MenuItem
                onClick={() => {
                  localStorage.removeItem("token");
                  localStorage.removeItem("data");
                  updateAuthContext({ token: "", data: "", username: "" });
                  navigate("/login");
                }}
              >
                Logout
              </MenuItem>
            </SubMenu>
          </Menu>
        </div>
      </SidebarContainer>
    </>
  );
}

export default Sidebar;
