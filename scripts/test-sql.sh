#!/usr/bin/env sh
set -eu
repo_dir=$(CDPATH= cd -- "$(dirname -- "$0")/.." && pwd)
cd "$repo_dir"
DOTNET=${DOTNET:-dotnet}
container_name=""
cleanup() {
  if [ -n "$container_name" ]; then docker stop "$container_name" >/dev/null 2>&1 || true; fi
}
trap cleanup EXIT
trap 'exit 130' INT
trap 'exit 143' TERM
if [ -z "${ERP_SQL_TEST_CONNECTION:-}" ]; then
  docker_arch=$(docker info --format '{{.Architecture}}')
  case "$docker_arch" in
    x86_64|amd64) ;;
    *) echo "SQL Server containers require an x64 Docker host. Use the GitHub SQL verification job or export ERP_SQL_TEST_CONNECTION for a disposable x64 SQL Server." >&2; exit 2 ;;
  esac
  container_name="erp-sql-test-$(python3 -c 'import uuid; print(uuid.uuid4().hex[:12])')"
  MSSQL_SA_PASSWORD=$(python3 -c 'import secrets; print("Demo!" + secrets.token_hex(24) + "a1")')
  export MSSQL_SA_PASSWORD
  docker run --detach --rm --name "$container_name" \
    --env ACCEPT_EULA=Y --env MSSQL_SA_PASSWORD \
    --publish 127.0.0.1::1433 mcr.microsoft.com/mssql/server:2022-latest >/dev/null
  sql_port=$(docker port "$container_name" 1433/tcp)
  sql_port=${sql_port##*:}
  ERP_SQL_TEST_CONNECTION="Server=127.0.0.1,$sql_port;User Id=sa;Password=$MSSQL_SA_PASSWORD;Encrypt=True;TrustServerCertificate=True"
  export ERP_SQL_TEST_CONNECTION
fi
"$DOTNET" test tests/ERP.SqlServer.Tests/ERP.SqlServer.Tests.csproj --no-restore --disable-build-servers
