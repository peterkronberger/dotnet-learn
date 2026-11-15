#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
input=seclog-input.txt
bin=src/SecLog.Cli/bin/Release/net11.0
tmp="$(mktemp -d)"
trap 'rm -rf "$tmp"' EXIT

# Debug build (the default configuration): the full program output.
dotnet run --project src/SecLog.Cli -c Debug --verbosity quiet <"$input" >"$tmp/debug.txt"
cat "$tmp/debug.txt"

# Release build: only the lines that differ from the Debug output.
dotnet run --project src/SecLog.Cli -c Release --verbosity quiet <"$input" >"$tmp/release.txt"
echo
echo "== Release build (dotnet run -c Release): lines that differ from Debug =="
diff "$tmp/debug.txt" "$tmp/release.txt" | grep '^[<>]' || true

# The Release build started in the two other ways.
echo
echo "== Release build (apphost SecLog.Cli): lines that differ from dotnet run =="
"$bin/SecLog.Cli" <"$input" >"$tmp/apphost.txt"
diff "$tmp/release.txt" "$tmp/apphost.txt" | grep '^[<>]' || echo "(none)"
echo
echo "== Release build (dotnet SecLog.Cli.dll): lines that differ from dotnet run =="
dotnet "$bin/SecLog.Cli.dll" <"$input" >"$tmp/dll.txt"
diff "$tmp/release.txt" "$tmp/dll.txt" | grep '^[<>]' || echo "(none)"
