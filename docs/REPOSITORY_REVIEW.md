# Repository preparation review

Reviewed on 2026-11-25. The initial inventory contained 680 tracked files, including 350 C# files, 240 JavaScript/JSX files, 68 JPEGs, a PDF, and a Word document. Automated content inspection covered every tracked text file and every unique blob reachable from `main`. Compiler, ESLint, formatter, and production-bundler checks cover source files; generated migrations were retained and scanned rather than rewritten. Media review included a contact sheet, document text, metadata, and PDF page render comparison. This is a repository preparation review, not an exhaustive proof of every business workflow.

## Completed changes

- Added the MIT license and matching .NET/npm manifest metadata.
- Removed the signing secret from current configuration and publishable Git history. Required configuration is supplied by environment variables, with validated values and a safe root `.env.example`.
- Expanded ignores for local configuration, credentials, private keys, databases, uploads, tooling, and build output.
- Removed the experimental `AAController`, unused client modules, template documentation, fake inventory loader, and console error interception.
- Replaced Create React App with Vite/Vitest, moved JSX-bearing modules to `.jsx`, updated React/routing, and added ESLint/Prettier and NuGet/npm lockfiles.
- Updated the backend from .NET 6 to .NET 10 and pinned compatible dependencies and the local EF tool.
- Added `make install`, `run`, `run-api`, `run-client`, `db-update`, `build`, `test`, `lint`, `format`, and `check`, plus a GitHub Actions verification workflow.
- Replaced anonymous employee-list initialization detection with a boolean status endpoint. Protected API routes require an approved employee role by default; public registration catalogs and blank templates remain explicitly accessible.
- Prevented password hashes/salts from being serialized, removed them from registration/setup responses, and added constant-time password-hash comparison while preserving stored-password compatibility.
- Made the initial admin receive admin/report permissions, required an explicit password, and repaired login initialization error handling.
- Fixed the API client to read the latest stored bearer token per request and remove stale authorization after logout. Corrupt/expired browser sessions are cleared safely.
- Prevented negative inventory adjustments, cleared notifications when stock reaches its minimum, repaired the fallback asset prefix alphabet, validated asset-number requests, and corrected employee-site lookup.
- Restricted file access to plain filenames, created missing upload directories, used asynchronous I/O and unique upload names, and protected private files from anonymous requests.
- Replaced the old spreadsheet parser with `.xlsx` input validation for headers, blank rows, missing/duplicate asset numbers, and optional serial numbers.
- Removed historic workflow screenshots containing business/user records, while retaining the written help. Removed document author metadata without changing the blank forms' contents; all 12 PDF pages rendered identically after cleanup.
- Replaced unverified stock dashboard images with repository-created SVGs and removed the old images from publishable history.

## Verification and practical limits

The root README documents exact installation, configuration, database migration, run, test, lint, and build commands. Tests use synthetic fixtures and isolated in-memory stores, with no live credentials. The default test suite and build/format/lint checks are run before delivery. SQL Server integration tests are a separate opt-in suite and CI job. npm and NuGet advisory scans are also run against the resolved dependency trees.

Verified results:

| Check | Result |
| --- | --- |
| `make install` | Passed using locked NuGet and npm dependencies |
| `make check` | Passed: API/client builds, tests, C# format, ESLint, Prettier |
| Backend tests | 93 passed; none skipped |
| Client tests | 16 passed; none skipped |
| Cleanup-script security tests | 8 passed |
| npm audit | Zero reported vulnerabilities |
| NuGet audit (including transitive dependencies) | Zero reported vulnerable packages |
| Current-file credential scan | Zero findings; includes newly added source and documentation |
| Historical credential scan | 2,022 reachable blobs; zero findings |
| EF migration SQL generation | Passed; generated approximately 100 KB of SQL |
| EF pending-model check | No model changes since the last migration |
| Git integrity and whitespace | Passed |

The existing SQL Server migration chain has not been applied to a live SQL Server on this ARM Mac. The separate `ERP.SqlServer.Tests` project compiles successfully and verifies migrations, persisted stock, relational constraints, rollback, and optional-date SQL queries when run on a supported server. `make test-sql` and the GitHub Actions `sql-server` job run it on x64 Docker or a supplied disposable server. This execution remains pending; in-memory test results are not represented as relational verification.

A full rebuild now completes with zero warnings and zero errors. Missing report dates are labeled N/A, unselected summary sections are explicitly nullable, missing employee sites and unapproved quantities have explicit checks, missing notifications return Not Found, and purchase notification destinations are validated before records change. Read-only return reporting no longer clears tracked employee roles. Regression tests cover the report groupings and related cleanup behavior. Warnings are treated as errors; they are not globally disabled.

Existing per-action permission enforcement still needs a systematic server-side audit. First-run admin setup should only be exposed in a trusted environment. Upload size limits, file scanning, adaptive password hashing, and concurrent transaction guarantees remain planned work, rather than claims made by these repository checks.

## Local history recovery

The 123-commit merge structure, commit messages, dates, and author/committer attribution to Amanuel Getaneh were preserved during sanitization. Commit hashes changed again because historical content changed. Recovery bundles, patches, and commit maps were moved to `~/ERP-private-recovery/2026-11-25/`, outside this repository. Their checksums were verified and permissions restricted. They must remain private. Push `main` explicitly; do not push internal refs or use a mirror push. A subsequent local-only cleanup removed the configured remote, remote-tracking branch, upstream, fetched-remote cache, and repository-specific GitHub credential helper. It normalized 18 URL-containing merge labels to `Merge branch 'main'`, renamed two remote/account-specific descriptions, and verified all 123 commits retain their trees, identities, dates, and parent structure. The pre-rewrite bundle and commit map are stored privately outside the project under `~/ERP-private-recovery/2026-11-25/local-message-rewrite-67b3cffd/`. No remote push was performed.

## Submission preparation follow-up

- The source-only workspace exporter refuses existing/nested output folders, excludes local configuration and data files even when tracked accidentally, rejects symlinks and suspected credentials, and starts with a fresh Git object database. Its security behavior has eight Python regression checks shared with the credential scanner.
- The supported sidebar release preserves navigation and logout behavior under React 18; obsolete peer dependencies were removed. ESLint was updated to a supported release, removing the legacy argparse dependency.
- Unused JsonModelBinder was removed and the third-party MIME mapper replaced with the framework implementation. The patched JSON dependency is limited to the test projects.
- Scoped ISC-licensed cache/filesystem dependency versions are pinned. The remaining exact license-name differences and Microsoft native terms are disclosed in `THIRD_PARTY_NOTICES.md` and `docs/dependency-licenses.json`; no unsupported claim of program approval is made.
- Tool-specific access settings have not been invented for an unidentified program CLI. `AGENTS.md` contains tool-independent instructions, and the source-only export provides physical separation from private Git history and local settings.
