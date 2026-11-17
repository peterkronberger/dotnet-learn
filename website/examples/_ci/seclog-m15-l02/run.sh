#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1

# The program output (Release build, as in the step before).
dotnet run --project src/SecLog.Cli -c Release --verbosity quiet <seclog-input.txt

# The test results of the whole solution.
echo
echo "== Tests =="
bash ../../../../tools/run-tests.sh SecLog.slnx
