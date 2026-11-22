#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m17-l01-http-exchange.csproj --verbosity quiet
