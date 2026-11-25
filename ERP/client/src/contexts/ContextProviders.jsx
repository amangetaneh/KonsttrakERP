import { AuthProvider } from "./AuthContext";

export default function ContextProviders({ children }) {
  return <AuthProvider>{children}</AuthProvider>;
}
