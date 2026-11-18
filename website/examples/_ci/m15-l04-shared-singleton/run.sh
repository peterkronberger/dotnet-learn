#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m15-l04-shared-singleton.csproj --verbosity quiet
