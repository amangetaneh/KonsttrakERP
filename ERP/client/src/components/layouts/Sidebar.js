import { useEffect, useState } from "react";
import {
    ProSidebar,
    Menu,
    MenuItem,
    SubMenu,
    SidebarHeader,
    SidebarFooter,
    SidebarContent,
} from "react-pro-sidebar";
import "react-pro-sidebar/dist/css/styles.css";
import { FaHome, FaList, FaBars, FaUser } from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom";
import { useAuth, useAuthUpdate } from "../../contexts/AuthContext";
import { useQueryClient } from "react-query";

function Sidebar() {
    const navigate = useNavigate();
    // const dispatch = useDispatch()
    // const contextData=useAuth()
    const updateAuthContext = useAuthUpdate();
    const queryClient = useQueryClient();

    const { data } = useAuth();

    const [collapsed, setCollapsed] = useState(false);

    return (
        <>
            <ProSidebar
                className="text-light sidebar-decrease-padding"
                collapsed={collapsed}
                width="255px"
                style={{ height: "100vh", position: "sticky", top: 0 }}
            >
                <SidebarHeader className="p-1 text-nowrap">
                    <MenuItem
                        className="ms-4 me-3"
                        icon={<FaBars className="fs-5" />}
                        style={{ display: "inline-flex" }}
                        onClick={() => setCollapsed(!collapsed)}
                    />
                    {!collapsed && "System Name"}
                </SidebarHeader>
                <SidebarContent>
                    <Menu>
                        <MenuItem className="" icon={<FaHome className="fs-5" />}>
                            Home
                        </MenuItem>

                        <SubMenu
                            title="Inventory"
                            icon={<FaList className="fs-5" />}
                            defaultOpen={true}
                        >
                            <SubMenu title="Stock">
                                <MenuItem>
                                    Add a new Material <Link to="/receive/purchase" />
                                </MenuItem>
                                <SubMenu title="Equipment">
                                    <MenuItem>
                                        Add a new Equipment <Link to="/receive" />
                                    </MenuItem>
                                    <MenuItem>
                                        Add a new Model <Link to="/receive" />
                                    </MenuItem>
                                    <MenuItem>
                                        Add a new Category <Link to="/receive" />
                                    </MenuItem>
                                </SubMenu>
                            </SubMenu>
                            <SubMenu title="Buy">
                                <MenuItem>
                                    Buy an Item <Link to="/purchase/material" />
                                </MenuItem>
                                {data.userRole?.canRequestPurchase === 1 && (
                                    <MenuItem>
                                        Buy Requests <Link to="/purchase" />
                                    </MenuItem>
                                )}
                            </SubMenu>
                            <SubMenu title="Purchase">
                                <MenuItem>
                                    Purchase an Item <Link to="/purchase/material" />
                                </MenuItem>
                                {data.userRole?.canRequestPurchase === 1 && (
                                    <MenuItem>
                                        Purchase Requests <Link to="/purchase" />
                                    </MenuItem>
                                )}
                            </SubMenu>
                            <SubMenu title="Receive">
                                <MenuItem>
                                    Receive an Item <Link to="/receive/purchase" />
                                </MenuItem>
                                <MenuItem>
                                    List Received Items <Link to="/receive" />
                                </MenuItem>
                            </SubMenu>
                            <SubMenu title="Transfer">
                                <MenuItem>
                                    New Material Transfer
                                    <Link to="/transfer/material" />
                                </MenuItem>
                                <MenuItem>
                                    New Equipment Transfer
                                    <Link to="/transfer/equipment" />
                                </MenuItem>
                                <MenuItem>
                                    Transfer In <Link to="/transfer/in" />
                                </MenuItem>
                                <MenuItem>
                                    Transfer Out
                                    <Link to="/transfer/out" />
                                </MenuItem>
                            </SubMenu>
                            <SubMenu title="Issue">
                                <MenuItem>
                                    New Issue Request
                                    <Link to="/issue/new" />
                                </MenuItem>
                                <MenuItem>
                                    Issue Requests
                                    <Link to="/issue" />
                                </MenuItem>
                            </SubMenu>
                            <SubMenu title="Borrow">
                                <MenuItem>
                                    New Borrow Request <Link to="/borrow/new" />
                                </MenuItem>
                                <MenuItem>
                                    Borrow Requests <Link to="/borrow" />
                                </MenuItem>
                                <MenuItem>
                                    Retrun Assets <Link to="/return/new" />
                                </MenuItem>
                                <MenuItem>
                                    Returned Assets <Link to="/return" />
                                </MenuItem>
                            </SubMenu>

                            <SubMenu title="Maintenance">
                                <MenuItem>
                                    New Maintenance Request <Link to="/maintenance/new" />
                                </MenuItem>
                                <MenuItem>
                                    Maintenance Requests <Link to="/maintenance" />
                                </MenuItem>
                            </SubMenu>
                            <SubMenu title="Settings">
                                <MenuItem>
                                    Report <Link to="/report" />
                                </MenuItem>
                            </SubMenu>
                        </SubMenu>
                    </Menu>
                </SidebarContent>
                <SidebarFooter>
                    <Menu subMenuBullets={true}>
                        <SubMenu title={data.username} icon={<FaUser className="fs-5" />}>
                            <MenuItem>Manage Account</MenuItem>
                            <MenuItem
                                onClick={() => {
                                    localStorage.removeItem("token");
                                    localStorage.removeItem("data");
                                    updateAuthContext({ token: "", data: "", username: "" });
                                    queryClient.invalidateQueries();
                                    navigate("/login");
                                }}
                            >
                                Logout
                            </MenuItem>
                        </SubMenu>
                    </Menu>
                </SidebarFooter>
            </ProSidebar>
        </>
    );
}

export default Sidebar;
