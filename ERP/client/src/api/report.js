import api from "./api";

export const fetchTransferReport = (data) =>
    api.post("/report/transfer", data).then((res) => res.data);
