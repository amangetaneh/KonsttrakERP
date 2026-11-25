import React from "react";
import { createRoot } from "react-dom/client";
import App from "./App";
import { QueryClient, QueryClientProvider } from "react-query";

import ContextProviders from "./contexts/ContextProviders";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 2,
    },
  },
});

createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <ContextProviders>
      <QueryClientProvider client={queryClient}>
        <App />
      </QueryClientProvider>
    </ContextProviders>
  </React.StrictMode>,
);
