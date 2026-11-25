import { createContext, useContext, useState } from "react";
import { readSession } from "../auth/session";

const AuthContext = createContext();
const AuthUpdateContext = createContext();

export function useAuth() {
  return useContext(AuthContext);
}

export function useAuthUpdate() {
  return useContext(AuthUpdateContext);
}

export function AuthProvider({ children }) {
  const [state, setState] = useState(readSession);
  return (
    <AuthContext.Provider value={state}>
      <AuthUpdateContext.Provider value={setState}>
        {children}
      </AuthUpdateContext.Provider>
    </AuthContext.Provider>
  );
}
