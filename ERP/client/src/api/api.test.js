import api from "./api";

it("reads the current token for each request and clears stale authorization", async () => {
  const adapter = vi.fn(async (config) => ({
    data: {},
    status: 200,
    statusText: "OK",
    headers: {},
    config,
  }));
  localStorage.setItem("token", "synthetic-one");
  await api.get("/employee", { adapter });
  expect(adapter.mock.calls[0][0].headers.Authorization).toBe(
    "Bearer synthetic-one",
  );
  localStorage.setItem("token", "synthetic-two");
  await api.get("/employee", { adapter });
  expect(adapter.mock.calls[1][0].headers.Authorization).toBe(
    "Bearer synthetic-two",
  );
  localStorage.removeItem("token");
  await api.get("/employee", { adapter });
  expect(adapter.mock.calls[2][0].headers.Authorization).toBeUndefined();
});
