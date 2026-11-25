.PHONY: install run run-api run-client build test lint format db-update check test-sql licenses
DOTNET ?= dotnet
NPM ?= npm

install:
	$(DOTNET) restore ERP.sln --locked-mode
	$(DOTNET) restore tests/ERP.SqlServer.Tests/ERP.SqlServer.Tests.csproj --locked-mode
	$(DOTNET) tool restore
	$(NPM) --prefix ERP/client ci

run: run-api

run-api:
	./scripts/with-env.sh $(DOTNET) run --project ERP --no-launch-profile

run-client:
	./scripts/with-env.sh $(NPM) --prefix ERP/client start

db-update:
	./scripts/with-env.sh $(DOTNET) ef database update --project ERP

build:
	$(DOTNET) build ERP.sln --no-restore
	$(DOTNET) build tests/ERP.SqlServer.Tests/ERP.SqlServer.Tests.csproj --no-restore
	$(NPM) --prefix ERP/client run build

test:
	$(DOTNET) test ERP.sln --no-restore
	$(NPM) --prefix ERP/client test
	python3 -m unittest discover -s tests/scripts

lint:
	$(DOTNET) format ERP.sln --verify-no-changes --no-restore
	$(DOTNET) format tests/ERP.SqlServer.Tests/ERP.SqlServer.Tests.csproj --verify-no-changes --no-restore
	$(NPM) --prefix ERP/client run lint
	$(NPM) --prefix ERP/client run format:check
	python3 scripts/check-secrets.py
	python3 scripts/check-licenses.py --check

format:
	$(DOTNET) format ERP.sln --no-restore
	$(DOTNET) format tests/ERP.SqlServer.Tests/ERP.SqlServer.Tests.csproj --no-restore
	$(NPM) --prefix ERP/client run format

check: build test lint

test-sql:
	DOTNET="$(DOTNET)" ./scripts/test-sql.sh

licenses:
	python3 scripts/check-licenses.py
