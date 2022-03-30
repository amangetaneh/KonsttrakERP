import { Badge } from "react-bootstrap";
import { RECEIVESTATUS } from "../../../../Constants";

function ReceiveStatusBadge(status) {
    const color = ["danger", "info", "success", "primary"];

    return (
        <Badge
            className="mt-1"
            bg={color[status]}
            style={{ width: "90px", fontSize: "14px" }}
        >
            {RECEIVESTATUS[status]}
        </Badge>
    );
}

export default ReceiveStatusBadge;
