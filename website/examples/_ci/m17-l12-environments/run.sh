#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# No environment is set: the app must fall back to its default.
unset ASPNETCORE_ENVIRONMENT DOTNET_ENVIRONMENT
dotnet run --project m17-l12-environments.csproj --verbosity quiet
