#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
# appsettings.json: MaxFailures 3, WindowMinutes 15, Mode file.
# The environment beats the file, the command line beats both.
export Detection__MaxFailures=5
export Detection__Mode=environment
dotnet run --project m15-l07-sources.csproj --verbosity quiet -- --Detection:Mode=command-line
