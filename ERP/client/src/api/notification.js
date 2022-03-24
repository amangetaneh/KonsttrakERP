import api from "./api";

export const fetchNotifications = (data) => api.get("/notification").then((res) => res.data);

export const clearNotification = (notificationId) => api.get(`/notification/clear/${notificationId}`);