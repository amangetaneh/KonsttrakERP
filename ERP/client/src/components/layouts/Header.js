import { Navbar, Container, Nav } from "react-bootstrap";
// import { Link } from "react-router-dom";
import PropTypes from "prop-types";

function Header({ title }) {
    return (
        <Navbar className="navbar-dark bg-teal" expand="sm">
            <Container>
                <Navbar.Brand>{title}</Navbar.Brand>
                <Nav className="me-auto"></Nav>
            </Container>
        </Navbar>
    );
}

Header.propTypes = {
    title: PropTypes.string.isRequired,
};

export default Header;
