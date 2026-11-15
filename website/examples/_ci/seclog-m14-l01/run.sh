#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project SecLog.csproj --verbosity quiet < seclog-input.txt
