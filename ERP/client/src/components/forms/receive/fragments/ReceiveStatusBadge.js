import { Badge } from "react-bootstrap";
import { ReceiveStatus } from "../../../../Constants";

function ReceiveStatusBadge(status) {
    const color = ["danger", "info", "success", "primary"];

    return (
        <Badge
            className="mt-1"
            bg={color[status]}
            style={{ width: "90px", fontSize: "14px" }}
        >
            {ReceiveStatus[status]}
        </Badge>
    );
}

export default ReceiveStatusBadge;
