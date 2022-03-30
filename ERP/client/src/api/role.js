import api from "./api";

export const fetchRoles = () => api.get("/UserRole").then((res) => res.data);
