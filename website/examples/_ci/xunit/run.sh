#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# Only the per-test lines; build noise and timings are filtered out so the snapshot is stable.
dotnet test xunit-demo.csproj --verbosity quiet --logger "console;verbosity=normal" 2>&1 \
  | grep -E '^\s+(Passed|Failed|Skipped) ' | sed -E 's/ \[[^]]*\]$//' | sort
