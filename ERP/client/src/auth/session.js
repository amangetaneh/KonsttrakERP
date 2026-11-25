import jwtDecode from "jwt-decode";

export function clearSession() {
  localStorage.removeItem("token");
  localStorage.removeItem("data");
}

export function readSession() {
  try {
    const token = localStorage.getItem("token");
    if (!token) return { token: "", data: null, username: "" };
    const { exp } = jwtDecode(token);
    if (!Number.isFinite(exp) || exp * 1000 <= Date.now())
      throw new Error("Expired session");
    return {
      token,
      data: JSON.parse(localStorage.getItem("data")),
      username: "",
    };
  } catch {
    clearSession();
    return { token: "", data: null, username: "" };
  }
}

export function sessionExpiresAt(token) {
  try {
    const { exp } = jwtDecode(token);
    return Number.isFinite(exp) ? exp * 1000 : 0;
  } catch {
    return 0;
  }
}
