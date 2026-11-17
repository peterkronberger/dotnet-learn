#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# The environment variable beats appsettings.json, the command line beats both.
export Seclog__Threshold=7
export Seclog__Mode=environment
dotnet run --project config-hosting.csproj --verbosity quiet -- --Seclog:Mode=command-line
