#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# One data row fails on purpose (bug in PasswordRule), the other rows still run.
bash ../../../../tools/run-tests.sh --allow-failures Theory.Tests.csproj
