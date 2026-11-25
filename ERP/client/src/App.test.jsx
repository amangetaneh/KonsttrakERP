import { render, screen } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "react-query";
import App from "./App";
import ContextProviders from "./contexts/ContextProviders";
import { fetchInitializationStatus } from "./api/employee";

vi.mock("./api/employee", async (importOriginal) => ({
  ...(await importOriginal()),
  fetchInitializationStatus: vi.fn(),
}));

it("renders login and first-run initialization without contacting a database", async () => {
  fetchInitializationStatus.mockResolvedValue({ initialized: false });
  const client = new QueryClient({
    defaultOptions: { queries: { retry: false, cacheTime: 0 } },
  });
  window.history.replaceState({}, "", "/login");
  render(
    <ContextProviders>
      <QueryClientProvider client={client}>
        <App />
      </QueryClientProvider>
    </ContextProviders>,
  );
  expect(
    await screen.findByRole("heading", { name: "Login" }),
  ).toBeInTheDocument();
  expect(
    await screen.findByRole("heading", { name: "Initialize Admin" }),
  ).toBeInTheDocument();
});
