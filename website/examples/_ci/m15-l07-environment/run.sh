#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# Without DOTNET_ENVIRONMENT the environment is Production: only appsettings.json is read.
echo "== no environment variable =="
env -u DOTNET_ENVIRONMENT dotnet run --project m15-l07-environment.csproj --verbosity quiet
# With Staging, appsettings.Staging.json is added on top.
echo "== DOTNET_ENVIRONMENT=Staging =="
DOTNET_ENVIRONMENT=Staging dotnet run --project m15-l07-environment.csproj --verbosity quiet
