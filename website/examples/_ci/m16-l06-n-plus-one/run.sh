#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m16-l06-n-plus-one.csproj --verbosity quiet
