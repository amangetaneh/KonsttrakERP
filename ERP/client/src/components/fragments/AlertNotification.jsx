import { Alert } from "react-bootstrap";
import PropTypes from "prop-types";

function AlertNotification({ title, content }) {
    return (
        <Alert variant="warning">
            <Alert.Heading>{title}</Alert.Heading>
            <p>{content}</p>
        </Alert>
    );
}

AlertNotification.propTypes = {
    title: PropTypes.string.isRequired,
    content: PropTypes.string.isRequired,
};

export default AlertNotification;
