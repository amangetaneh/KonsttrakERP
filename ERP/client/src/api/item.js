import api from "./api";

export const fetchMaterials = () => api.get("/item/material").then((res) => res.data);
export const fetchEquipments = () => api.get("/item/equipment").then((res) => res.data);

