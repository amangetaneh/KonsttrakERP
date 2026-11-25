import { readSession, sessionExpiresAt } from "./session";

function token(exp) {
  return `e30.${btoa(JSON.stringify({ exp }))}.synthetic-signature`;
}

it("restores an unexpired synthetic session", () => {
  const value = token(Math.floor(Date.now() / 1000) + 3600);
  localStorage.setItem("token", value);
  localStorage.setItem("data", JSON.stringify({ username: "demo-user" }));
  expect(readSession()).toMatchObject({
    token: value,
    data: { username: "demo-user" },
  });
  expect(sessionExpiresAt(value)).toBeGreaterThan(Date.now());
});

it.each(["invalid-token", token(1), token("invalid")])(
  "clears an invalid or expired session",
  (value) => {
    localStorage.setItem("token", value);
    localStorage.setItem("data", "{}");
    expect(readSession().token).toBe("");
    expect(localStorage.getItem("token")).toBeNull();
    expect(localStorage.getItem("data")).toBeNull();
  },
);

it("recovers from malformed saved JSON", () => {
  localStorage.setItem("token", token(Math.floor(Date.now() / 1000) + 3600));
  localStorage.setItem("data", "bad-json");
  expect(readSession().data).toBeNull();
});
