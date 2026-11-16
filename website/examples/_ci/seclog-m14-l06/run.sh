#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
input=seclog-input.txt
tmp="$(mktemp -d)"
trap 'rm -rf "$tmp"' EXIT

# Release build: the full program output.
dotnet run --project src/SecLog.Cli -c Release --verbosity quiet <"$input" >"$tmp/release.txt"
cat "$tmp/release.txt"

# Publish as a self-contained single file for linux-x64 (properties in SecLog.Cli.csproj).
dotnet publish src/SecLog.Cli -c Release -o "$tmp/publish" --verbosity quiet >"$tmp/publish.log" 2>&1 \
  || { cat "$tmp/publish.log" >&2; exit 1; }
echo
echo "== Files in the publish folder =="
ls "$tmp/publish"

# Start the single file: its output must equal the output of the Release build.
echo
echo "== Single file (SecLog.Cli): lines that differ from the Release build =="
"$tmp/publish/SecLog.Cli" <"$input" >"$tmp/single.txt"
diff "$tmp/release.txt" "$tmp/single.txt" | grep '^[<>]' || echo "(none)"
