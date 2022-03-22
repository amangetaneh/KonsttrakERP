import api from "./api";

export const fetchTransfers = () =>
    api.get("/transfer").then((res) => res.data);

export const fetchTransfer = (id) =>
    api.get(`/transfer/${id}`).then((res) => res.data);

export const requestMaterialTransfer = (data) => api.post("/transfer/request/material", data);

export const requestEquipmentTransfer = (data) => api.post("/transfer/request/equipment", data);

export const approveTransfer = (data) => api.post("/transfer/approve", data);

export const declineTransfer = (data) => api.post("/transfer/decline", data);

export const sendTransfer = (data) => api.post("/transfer/send", data);

export const receiveTransfer = (data) => api.post("/transfer/receive", data);
