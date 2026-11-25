export function rowsToRecords(rows) {
  if (!Array.isArray(rows) || rows.length < 2)
    throw new Error("The sheet is empty.");
  const headers = rows[0].map((value) => String(value ?? "").trim());
  if (
    headers.some((value) => !value) ||
    new Set(headers).size !== headers.length
  ) {
    throw new Error("Column headers must be nonempty and unique.");
  }
  const records = rows
    .slice(1)
    .filter((row) => row.some((cell) => cell !== null && cell !== ""))
    .map((row) =>
      Object.fromEntries(
        headers.map((header, index) => [header, row[index] ?? ""]),
      ),
    );
  if (!records.length) throw new Error("The sheet has no data rows.");
  return records;
}

export function mapImportedAssets(rows, assetColumn, serialColumn) {
  const assets = rows.map((row) => ({
    assetNo: String(row[assetColumn] ?? "").trim(),
    serialNo: serialColumn ? String(row[serialColumn] ?? "").trim() : "",
  }));
  if (!assets.length || assets.some((asset) => !asset.assetNo))
    throw new Error("Every row needs an asset number.");
  if (new Set(assets.map((asset) => asset.assetNo)).size !== assets.length)
    throw new Error("Asset numbers must be unique within the import.");
  return assets;
}
