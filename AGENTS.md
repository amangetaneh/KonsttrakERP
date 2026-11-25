# CLI contributor guide

- Read README.md before changing setup, configuration, or run commands.
- Backend: .NET 10, ASP.NET Core controllers, Entity Framework Core with SQL Server.
- Client: React 18, JavaScript/JSX, Vite, Vitest, ESLint, and Prettier.
- Use `make install`, `make test`, `make lint`, and `make build` from the root. C# warnings fail the build.
- Run `make test-sql` on an x64 Docker host for SQL Server integration tests; this suite uses a new disposable database, never the application database.
- Add focused synthetic-data tests for business logic and security changes.
- Preserve existing migration history. Review schema changes explicitly and add migrations with the local EF tool.
- Keep secrets in environment variables and maintain `.env.example` with placeholders only.
- Never commit runtime uploads, local databases, credentials, or history backup bundles.
- Use `make format` after source edits. Keep license metadata MIT.

## Private local material

- Do not open real `.env` files, private local settings, database dumps, runtime uploads, or recovery bundles. `.env.example` is safe to read.
- Do not dump the process environment or print connection strings, passwords, or signing keys.
- `scripts/prepare-session.py --output /path/outside/repository` creates a source-only copy with a fresh Git database. Install dependencies in that copy before coding.
- Regenerate and review `docs/dependency-licenses.json` with `make licenses` after dependency changes. Do not relabel third-party packages as MIT or claim program approval for unlisted license names.
