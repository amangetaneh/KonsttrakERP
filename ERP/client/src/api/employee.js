import api from "./api";

export const fetchEmployees = () => api.get("/employee").then((res) => res.data);
