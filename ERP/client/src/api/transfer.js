import api from "./api";

export const fetchTransfers = () =>
    api.get("/transfer").then((res) => res.data);

export const requestTransfer = (data) => api.post("/transfer/request", data);
