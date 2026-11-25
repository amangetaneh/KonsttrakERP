#!/usr/bin/env sh
set -eu
repo_dir=$(CDPATH= cd -- "$(dirname -- "$0")/.." && pwd)
if [ ! -f "$repo_dir/.env" ]; then
  echo "Missing .env. Copy .env.example to .env and configure it first." >&2
  exit 1
fi
set -a
. "$repo_dir/.env"
set +a
cd "$repo_dir"
exec "$@"
