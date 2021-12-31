import React,{ useState }  from 'react';
import { Collapse, Container, DropdownItem, DropdownMenu, DropdownToggle, Nav, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown } from 'reactstrap';
import { Link } from 'react-router-dom';
import { useTranslation } from "react-i18next";

const NavMenu = (props) => {
    const { t, i18n } = useTranslation();
    const [collapsed, setCollapsed] = useState(false);
    const [user] = useState(null);

    const logOut = (e) => {
        e.preventDefault();
    }

    return (
        <header>
            <Navbar
                color="dark"
                dark
                light
                className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
            >
                <Container>
                    <NavbarBrand tag={Link} to="/">TestProject</NavbarBrand>
                <NavbarToggler onClick={() => setCollapsed(!collapsed)} className="mr-2" />
                <Collapse isOpen={collapsed} navbar>
                    <Nav className="me-auto" navbar>
                        <UncontrolledDropdown nav inNavbar>
                            <DropdownToggle nav caret>
                                {i18n.language.toUpperCase()}
                            </DropdownToggle>
                            <DropdownMenu right>
                                <DropdownItem onClick={() => i18n.changeLanguage("ua")}>
                                    UA
                                </DropdownItem>
                                <DropdownItem onClick={() => i18n.changeLanguage("en")}>
                                    EN
                                </DropdownItem>
                            </DropdownMenu>
                        </UncontrolledDropdown>
                    </Nav>
                    <Nav className="d-flex" navbar>
                        {user ? (
                            <><NavItem>
                                <NavLink tag={Link} to="/profile">{t("Profile")}</NavLink>
                            </NavItem>
                                {user.role === "Admin" &&
                                    <NavItem>
                                        <NavLink tag={Link} to="/users">{t("Users")}</NavLink>
                                    </NavItem>
                                }
                                <li className="nav-item">
                                    <a href="/login" className="nav-link" onClick={logOut}>
                                        {t("LogOut")}
                                    </a>
                                </li></>
                        ) : (
                            <>
                                <NavItem>
                                    <NavLink tag={Link} to="/login">{t("Login")}</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} to="/register">{t("SignUp")}</NavLink>
                                </NavItem>
                            </>
                        )}
                    </Nav>
                </Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;