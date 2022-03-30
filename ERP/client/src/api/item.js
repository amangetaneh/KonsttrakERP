import api from "./api";

export const fetchMaterials = () => api.get("/item/material").then((res) => res.data);

export const fetchEquipments = () => api.get("/item/equipment").then((res) => res.data);

export const fetchItem = (itemId) => api.get(`/item/${itemId}`).then((res) => res.data);

export const fetchEquipmentModel = (modelId) =>
    api.get(`/item/equipmentmodel/${modelId}`).then((res) => res.data);


    
    