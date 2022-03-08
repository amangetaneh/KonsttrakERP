import { useState } from "react"
import {
  ProSidebar,
  Menu,
  MenuItem,
  SubMenu,
  SidebarHeader,
  SidebarFooter,
  SidebarContent,
} from "react-pro-sidebar"
import "react-pro-sidebar/dist/css/styles.css"
import { FaHome, FaList, FaBars, FaUser } from "react-icons/fa"
import { Link } from "react-router-dom"

function Sidebar() {
  const [collapsed, setCollapsed] = useState(false)

  return (
    <>
      <ProSidebar
        className="text-light sidebar-decrease-padding"
        collapsed={collapsed}
        width="250px"
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
              
            >
              <SubMenu title="Stock"></SubMenu>
              <SubMenu title="Purchase">
                <MenuItem>
                    Purchase an Item <Link to="/purchase/new" />
                </MenuItem>
                <MenuItem>
                    List Purchase Requests <Link to="/purchase/list" />
                </MenuItem>
              </SubMenu>
              <SubMenu title="Receive">
                <MenuItem>
                    Receive an Item <Link to="/receive/new" />
                </MenuItem>
                <MenuItem>
                    List Received Items <Link to="/receive/new" />
                </MenuItem>
              </SubMenu>
              <SubMenu title="Transfer">
                <MenuItem>
                  New Transfer <Link to="/transfer/new" />
                </MenuItem>
                <MenuItem>
                  Transfer In <Link to="/transfer/in" />
                </MenuItem>
                <MenuItem>
                  Transfer Out
                  <Link to="/transfer/out" />
                </MenuItem>
              </SubMenu>
              <SubMenu title="Issue"></SubMenu>
              <SubMenu title="Maintainance"></SubMenu>
            </SubMenu>
          </Menu>
        </SidebarContent>
        <SidebarFooter>
          <Menu subMenuBullets={true}>
            <SubMenu title="User Name" icon={<FaUser className="fs-5" />}>
              <MenuItem>Manage Account</MenuItem>
              <MenuItem>Logout</MenuItem>
            </SubMenu>
          </Menu>
        </SidebarFooter>
      </ProSidebar>
    </>
  )
}

export default Sidebar
