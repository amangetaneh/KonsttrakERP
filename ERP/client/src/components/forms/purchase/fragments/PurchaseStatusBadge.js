import { Badge } from "react-bootstrap";
import { PurchaseStatus } from "../../../../Constants";

function PurchaseStatusBadge(status) {
    const color = ["danger", "info", "success", "primary"];

    return (
        <Badge
            className="mt-1"
            bg={color[status]}
            style={{ width: "90px", fontSize: "14px" }}
        >
            {PurchaseStatus[status]}
        </Badge>
    );
}

export default PurchaseStatusBadge;
