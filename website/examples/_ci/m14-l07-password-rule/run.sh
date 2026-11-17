#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# One test fails on purpose (bug in PasswordRule), so the lesson can show the failure.
bash ../../../../tools/run-tests.sh --allow-failures PasswordRule.Tests.csproj
