import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import Sidebar from "./Sidebar";

const { updateAuth } = vi.hoisted(() => ({ updateAuth: vi.fn() }));
vi.mock("../../contexts/AuthContext", () => ({
  useAuth: () => ({
    data: { username: "demo-user", userRole: { isAdmin: true } },
  }),
  useAuthUpdate: () => updateAuth,
}));

beforeEach(() => {
  vi.stubGlobal(
    "ResizeObserver",
    class {
      observe() {}
      unobserve() {}
      disconnect() {}
    },
  );
});
afterEach(() => vi.unstubAllGlobals());

function renderSidebar() {
  window.matchMedia = vi.fn(() => ({
    matches: false,
    addEventListener: vi.fn(),
    removeEventListener: vi.fn(),
    addListener: vi.fn(),
    removeListener: vi.fn(),
  }));
  return render(
    <MemoryRouter initialEntries={["/employee"]}>
      <Sidebar />
      <Routes>
        <Route path="/" element={<h1>Demo Home Page</h1>} />
        <Route path="/login" element={<h1>Demo Login Page</h1>} />
        <Route path="*" element={<h1>Demo Employee Page</h1>} />
      </Routes>
    </MemoryRouter>,
  );
}

it("preserves working router links and the collapse control", async () => {
  const user = userEvent.setup();
  renderSidebar();
  expect(screen.getByRole("link", { name: "Home" })).toHaveAttribute(
    "href",
    "/",
  );
  await user.click(screen.getByRole("link", { name: "Home" }));
  expect(
    screen.getByRole("heading", { name: "Demo Home Page" }),
  ).toBeInTheDocument();
  await user.click(screen.getByRole("button", { name: "Toggle navigation" }));
  expect(
    screen.getByRole("button", { name: "Toggle navigation" }),
  ).toBeInTheDocument();
});

it("clears the saved session and returns to login on logout", async () => {
  const user = userEvent.setup();
  localStorage.setItem("token", "synthetic-session");
  localStorage.setItem("data", "{}");
  renderSidebar();
  await user.click(screen.getByText("demo-user"));
  await user.click(await screen.findByText("Logout"));
  expect(localStorage.getItem("token")).toBeNull();
  expect(localStorage.getItem("data")).toBeNull();
  expect(updateAuth).toHaveBeenCalledWith({
    token: "",
    data: "",
    username: "",
  });
  expect(
    screen.getByRole("heading", { name: "Demo Login Page" }),
  ).toBeInTheDocument();
});
