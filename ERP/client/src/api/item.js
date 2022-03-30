import api from "./api";

export const fetchMaterials = () => api.get("/item/getmaterials").then((res) => res.data);

export const fetchEquipments = () => api.get("/item/getequipments").then((res) => res.data);

export const fetchItem = (itemId) => api.get(`/item/${itemId}`).then((res) => res.data);

export const fetchEquipmentModel = (modelId) =>
    api.get(`/item/equipmentmodel/${modelId}`).then((res) => res.data);


    
    