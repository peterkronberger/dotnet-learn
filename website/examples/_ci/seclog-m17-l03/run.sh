#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1

# The program output (Release build, as in the step before).
# The environment variables beat appsettings.json: MaxFailures 3 becomes 5, WindowMinutes 15 becomes 10.
export Detection__MaxFailures=5
export Detection__WindowMinutes=10
# The user name to search for comes from outside: it beats Search:User of appsettings.json.
export Search__User=bob
# The database is a SQLite file in a fresh temporary directory, so every run starts empty.
tmp="$(mktemp -d)"
trap 'rm -rf "$tmp"' EXIT
export ConnectionStrings__SecLog="Data Source=$tmp/seclog.db"
dotnet run --project src/SecLog.Cli -c Release --verbosity quiet <seclog-input.txt

# The test results of the whole solution.
echo
echo "== Tests =="
bash ../../../../tools/run-tests.sh SecLog.slnx
