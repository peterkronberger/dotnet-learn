#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# One test fails on purpose: Throws needs the exact exception type.
bash ../../../../tools/run-tests.sh --allow-failures Checks.Tests.csproj
