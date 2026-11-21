#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m16-l04-translation.csproj --verbosity quiet
