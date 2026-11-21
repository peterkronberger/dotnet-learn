#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m16-l03-one-to-many.csproj --verbosity quiet
