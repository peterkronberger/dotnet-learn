#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
bash ../../../../tools/run-tests.sh Counter.Tests/Counter.Tests.csproj
