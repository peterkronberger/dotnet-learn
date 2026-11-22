#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m17-l10-document.csproj --verbosity quiet
