# ERP

An inventory and equipment management application with a React 18 interface and an ASP.NET Core API. It tracks material stock and individual equipment assets across sites, with employee registration, role permissions, purchases, receiving, issues, borrowing, returns, transfers, maintenance, minimum-stock notifications, and reports.

## Requirements

- .NET **10 SDK** (the repository's `global.json` accepts current 10.0 feature bands).
- Node.js **24** and npm; `nvm use` selects the major version in `.nvmrc`.
- A reachable **SQL Server 2019 or later** instance and credentials permitted to create/update the development database. SQL Server is an external prerequisite; it is not embedded in this application. On Apple Silicon, use a remote SQL Server or a supported x64 host.
- Git, Make, and a POSIX shell (macOS, Linux, or WSL). OpenSSL is used below to generate a signing key.

## Repository structure

```text
ERP.sln                     API and backend test solution
ERP/
  Controllers/              HTTP endpoints
  Services/                 Inventory and transaction business logic
  Models/                   Entity Framework database entities
  DTOs/                     Request and report payloads
  Context/                  SQL Server DbContext and relationships
  Migrations/               Existing database migration history
  Configuration/            Environment configuration validation
  Security/                 Password hashing compatibility helpers
  UploadedFiles/            Blank form templates; runtime uploads are ignored
  client/
    src/api/                API client and endpoint wrappers
    src/auth/               Session restoration and expiry
    src/components/         Forms, reports, layouts, and help guides
    src/contexts/           React context providers
    src/models/             Client form models
    src/utils/              Spreadsheet import validation
    public/                 Static browser assets
scripts/                    Root environment loader and secret scanner
tests/ERP.Tests/            Backend unit and in-process API tests
tests/ERP.SqlServer.Tests/  Real SQL Server integration tests
tests/scripts/             Session-export security tests
docs/                       Repository review and verification notes
```

## Install and configure

Run all commands from the repository root:

```sh
nvm use                     # optional if Node 24 is already on PATH
make install
cp .env.example .env
openssl rand -hex 64
```

Edit `.env` locally: replace `CHANGE_ME_DATABASE_PASSWORD` inside `ConnectionStrings__DefaultConnection` with your development SQL Server password, and replace `AppSettings__Token` with the generated random key. Do not commit `.env`. Keep the connection string single-quoted so its semicolons and spaces are preserved by the shell.

The required application settings are `ConnectionStrings__DefaultConnection`, `AppSettings__Token` (at least 64 UTF-8 bytes), and `Cors__AllowedOrigins` (comma-separated browser origins without trailing slashes). `ASPNETCORE_URLS` controls the API listener; `VITE_API_URL` is the public API URL compiled into the browser bundle. Never store secrets in `VITE_*` settings.

Root Make commands load `.env` through `scripts/with-env.sh`. ASP.NET Core reads exported environment variables natively; it does not load `.env` when `dotnet run` is invoked directly. Vite also reads the root `.env` and only exposes variables prefixed with `VITE_` to the browser.

Create/update your **development** database using the checked-in migrations:

```sh
make db-update
```

Review migrations and back up existing data before applying them to an existing database. The migrations are preserved, rather than replaced by a new initial schema.

## Run

Use two terminals at the repository root:

```sh
# Terminal 1: API at http://localhost:5067; Swagger at /swagger
make run-api              # make run is an alias
```

```sh
# Terminal 2: client at http://localhost:5173
make run-client
```

On an empty database the login screen offers **Initialize Admin**. Choose your own password of at least eight characters; the initial username is `admin`. There is no default password. After initialization, log in and create sites and roles before registering employees. Restrict first-run setup to your trusted local environment. The application rejects a second initialization once an employee exists.

Development uses HTTP on loopback to avoid requiring a local HTTPS certificate. Configure HTTPS at your deployment boundary; production enables HTTPS redirection. CORS permits only the origins you configure.

## Test, lint, and build

```sh
make test                 # backend xUnit + in-process API tests, client Vitest
make lint                 # dotnet format verification + ESLint + Prettier
make build                # compile API and build client into ERP/client/dist
make check                # build, test, lint, credential and license checks
make format               # apply C# and client formatting
python3 scripts/check-secrets.py
make licenses             # refresh dependency license inventory
```

The default tests use synthetic records and isolated in-memory databases; no `.env`, live credentials, or SQL Server is required for tests. They cover password hashing and serialization, configuration validation, admin initialization/login, authorization and CORS, inventory thresholds, asset numbering, safe file access, browser session expiry, API authorization headers, and spreadsheet input validation. In-memory tests do not verify SQL Server query translation or apply migrations to a real server.

The separate SQL Server suite applies the full migration chain to a new `erp_test_<random ID>` database, verifies persisted stock, foreign-key enforcement, transaction rollback, and nullable-date report queries, then drops only that test database. On an x64 Docker host run:

```sh
make test-sql
```

The runner starts and removes its own SQL Server 2022 container and generates an ephemeral password without logging it. It never reads `.env` or uses your application database. On Apple Silicon, use the GitHub Actions `sql-server` job or supply `ERP_SQL_TEST_CONNECTION` for an accessible disposable x64 SQL Server; never print or commit that value. The account needs permission to create/drop test databases. The script intentionally refuses unsupported ARM SQL Server containers.

Individual commands:

```sh
dotnet test ERP.sln
npm --prefix ERP/client test
npm --prefix ERP/client run test:watch
npm --prefix ERP/client run lint
dotnet format ERP.sln --verify-no-changes
```

For CLI configuration overrides, use `make DOTNET=/path/to/dotnet install` or `make NPM=/path/to/npm test`. No Visual Studio, IIS Express, or global `dotnet-ef` installation is required. `dotnet restore ERP.sln --force-evaluate` refreshes NuGet lockfiles after intentional dependency changes. `dotnet tool restore` installs the repository-pinned EF tool.

## GitHub preparation

Review `git diff` and `git status` before committing. Keep `.env`, uploaded business files, local databases, and Git bundles private. Run `python3 scripts/check-secrets.py --history` before the first push. If a real credential was ever shared or committed, rotate it with its provider; rewriting Git history alone does not revoke it.

The original history backups were moved outside this repository to `~/ERP-private-recovery/2026-11-25/`, with private file permissions and verified SHA-256 checksums. They are local recovery material. Push the reviewed `main` branch, never a mirror containing internal or backup refs. This repository is local-only: no Git remote or upstream is configured. Historical remote merge labels have been normalized to local wording. If you later publish it, add your chosen remote explicitly and review the branch before pushing.

## Source-only coding workspace

For a session that must not have access to old Git objects, private local settings, or installed dependencies, create a new source-only copy outside this repository:

```sh
python3 scripts/prepare-session.py --output /path/to/new/erp-session
cd /path/to/new/erp-session
make install
```

The destination must not exist. The export checks candidate files for credential patterns, excludes local secrets/data and non-template documents, rejects symlinks, and initializes an empty Git repository. It copies current source changes without copying Git history or recovery bundles. Review the exported files before use. Keep this canonical repository for submission and contribution history. Tool-specific access settings depend on the coding tool you are actually assigned; no unsupported ignore file is presented as a security boundary.

## Third-party licenses

See [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) and [the locked dependency inventory](docs/dependency-licenses.json). Dependencies keep their own licenses. The inventory explicitly flags names not listed in the supplied agreement, including some development-tool licenses and Microsoft native-component terms; a passing software build is not a certification of program eligibility.

## Planned Work / Roadmap

1. Expand SQL Server integration coverage to complete purchase, transfer, borrow, and return workflows.
2. Make multi-step inventory transactions atomic and test concurrent stock and asset-number updates.
3. Enforce each role permission on the server, with an authorization matrix and approval tests.
4. Migrate legacy HMAC password records to a versioned, adaptive password hasher during login.
5. Add upload size/type limits, malware scanning, retention, and per-record download permissions.
6. Split the large report services into smaller query builders and expand report grouping coverage.
7. Add accessible form labels, keyboard navigation, and complete transaction workflow browser tests.
8. Add pagination and filtering to large inventory, employee, and report datasets.

## License

MIT. See [LICENSE](LICENSE). Copyright 2026 Amanuel Getaneh.
