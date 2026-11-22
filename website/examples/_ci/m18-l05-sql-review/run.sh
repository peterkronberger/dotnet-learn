#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m18-l05-sql-review.csproj --verbosity quiet
