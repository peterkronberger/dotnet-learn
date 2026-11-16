#!/usr/bin/env bash
# Builds the snippets of lesson m14-l06 and prints the compiler diagnostics.
# The path of the source file is cut off, because it differs between machines.
set -uo pipefail
cd "$(dirname "$0")"
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
tmp="$(mktemp -d)"
trap 'rm -rf "$tmp"' EXIT

build() { # <snippet file> <Program.cs name shown>
  local dir="$tmp/${1%.snippet.cs}"
  mkdir -p "$dir"
  cat >"$dir/App.csproj" <<'XML'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net11.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
XML
  cp "$1" "$dir/Program.cs"
  echo "== $1"
  dotnet build "$dir" --nologo -v q 2>&1 \
    | grep -E '(warning|error) CS' \
    | sed -E 's|^.*/Program\.cs|Program.cs|; s| \[[^]]*\.csproj\]$||' \
    | awk '!seen[$0]++'
}

build obsolete-warning.snippet.cs
build obsolete-error.snippet.cs
build wrong-target.snippet.cs
