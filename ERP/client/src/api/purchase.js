import api from "./api";

export const fetchPurchases = () =>
    api.get("/receive").then((res) => res.data);

export const fetchPurchase = (id) =>
    api.get(`/receive/${id}`).then((res) => res.data);

export const requestPurchase = (data) => api.post("/purchase/request", data);

export const approvePurchase = (data) => api.post("/purchase/approve", data);

export const declinePurchase = (data) => api.post("/purchase/decline", data);

export const checkPurchase = (data) => api.post("/purchase/check", data);

export const confirmPurchase = (data) => api.post("/purchase/confirm", data);
