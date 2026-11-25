import { mapImportedAssets, rowsToRecords } from "./assetImport";

it("converts a synthetic worksheet and preserves numeric serials", () => {
  const rows = rowsToRecords([
    ["Asset", "Serial"],
    ["DEMO-001", 42],
    [null, null],
  ]);
  expect(mapImportedAssets(rows, "Asset", "Serial")).toEqual([
    { assetNo: "DEMO-001", serialNo: "42" },
  ]);
});
it("does not invent an undefined serial number when no column is selected", () => {
  expect(
    mapImportedAssets([{ Asset: "DEMO-001" }], "Asset", "")[0].serialNo,
  ).toBe("");
});
it.each([
  [],
  [
    ["Asset", "Asset"],
    [1, 2],
  ],
  [[""], [1]],
  [["Asset"], [null]],
])("rejects unusable worksheets", (rows) => {
  expect(() => rowsToRecords(rows)).toThrow();
});
it("rejects missing and duplicate asset numbers", () => {
  expect(() => mapImportedAssets([{ Asset: "" }], "Asset", "")).toThrow();
  expect(() =>
    mapImportedAssets(
      [{ Asset: "DEMO-001" }, { Asset: "DEMO-001" }],
      "Asset",
      "",
    ),
  ).toThrow();
});
