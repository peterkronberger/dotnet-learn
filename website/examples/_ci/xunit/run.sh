#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# Stable per-test lines from the shared helper (xUnit v3 on Microsoft.Testing.Platform).
bash ../../../../tools/run-tests.sh xunit-demo.csproj
