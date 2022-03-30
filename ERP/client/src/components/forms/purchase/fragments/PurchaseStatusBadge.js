import { Badge } from "react-bootstrap";
import { PURCHASESTATUS } from "../../../../Constants";

function PurchaseStatusBadge(status) {
    const color = ["danger", "secondary", "primary", "success", "info", "warning"];

    return (
        <Badge
            className="mt-1"
            bg={color[status]}
            style={{ width: "90px", fontSize: "14px" }}
        >
            {PURCHASESTATUS[status]}
        </Badge>
    );
}

export default PurchaseStatusBadge;
