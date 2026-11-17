#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# One test fails on purpose: the rule has a bug that the first test cannot see.
bash ../../../../tools/run-tests.sh --allow-failures Coverage.Tests.csproj
