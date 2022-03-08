import { Badge } from "react-bootstrap";

export function transferStatusBadge(stat) {
    const color = ["info", "success", "primary", "secondary"];
    const text = ["Requested", "Approved", "Sent", "Recieved"];

    return (
        <Badge pill bg={color[stat]} style={{ width: "90px" }}>
            {text[stat]}
        </Badge>
    );
}
