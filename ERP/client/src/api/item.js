import api from "./api";

export const fetchConsumableItems = () =>
    api.get("/item/consumable").then((res) => res.data);
