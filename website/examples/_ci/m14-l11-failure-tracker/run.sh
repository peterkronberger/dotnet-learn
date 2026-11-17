#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
bash ../../../../tools/run-tests.sh Tracker.Tests.csproj
