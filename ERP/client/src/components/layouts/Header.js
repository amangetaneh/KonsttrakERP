import { Navbar, Container, Nav, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import PropTypes from "prop-types";
import { FaArrowLeft } from "react-icons/fa";

function Header({ title }) {
    const navigate = useNavigate();

    function goBack() {
        navigate(-1);
    }

    return (
        <Navbar className="navbar-dark bg-teal" expand="sm">
            <Container>
                <Button
                    className="text-white"
                    variant="transparent"
                    onClick={goBack}
                >
                    <FaArrowLeft />
                </Button>
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
