#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m17-l08-pipeline.csproj --verbosity quiet
