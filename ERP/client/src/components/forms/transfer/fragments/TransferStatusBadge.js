import { Badge } from "react-bootstrap";
import { TransferStatus } from "../../../../Constants";

function TransferStatusBadge(status) {
    const color = ["danger", "info", "success", "primary", "secondary"];

    return (
        <Badge
            className="mt-1"
            bg={color[status]}
            style={{ width: "90px", fontSize: "14px" }}
        >
            {TransferStatus[status]}
        </Badge>
    );
}

export default TransferStatusBadge;
