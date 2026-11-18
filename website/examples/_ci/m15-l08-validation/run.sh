#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet run --project m15-l08-validation.csproj --verbosity quiet
