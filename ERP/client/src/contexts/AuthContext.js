import React, { useContext, useState } from "react";

const AuthContext = React.createContext();
const AuthtUpdateContext = React.createContext();

export function useAuth() {
    return useContext(AuthContext);
}

export function useAuthUpdate(){
    return useContext(AuthtUpdateContext);
}

export function AuthProvider({ children }) {
    const [state, setState] = useState({});

    return (
        <AuthContext.Provider value={state}>
            <AuthtUpdateContext.Provider value={setState}>
                {children}
            </AuthtUpdateContext.Provider>
        </AuthContext.Provider>
    );
}
