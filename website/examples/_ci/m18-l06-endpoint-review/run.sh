#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m18-l06-endpoint-review.csproj --verbosity quiet
