import axios from "axios";

export const url = (
  import.meta.env.VITE_API_URL || "http://localhost:5067/api/"
).replace(/\/?$/, "/");
const api = axios.create({ baseURL: url });

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  else delete config.headers.Authorization;
  return config;
});

export default api;
