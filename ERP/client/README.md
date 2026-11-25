# ERP client

React inventory and equipment management UI, built with Vite. Node.js 24 is required.

Follow the [root README](../../README.md) for SQL Server setup, the root `.env.example`, and the two-terminal development workflow. From the repository root:

```sh
make install
make run-client
npm --prefix ERP/client test
npm --prefix ERP/client run lint
npm --prefix ERP/client run format:check
npm --prefix ERP/client run build
```

The production bundle is written to `dist/`. `VITE_API_URL` is public browser configuration; it must never contain credentials. Spreadsheet imports accept `.xlsx` files with unique header names and at least one data row; legacy `.xls` files must first be saved as `.xlsx`.

License: **MIT**. See the [root LICENSE](../../LICENSE).
