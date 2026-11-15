#!/usr/bin/env bash
# Builds m14-l03-build in both configurations and shows what lesson m14-l03 teaches:
# the build output, what the configurations change, and the ways to start the program.
set -euo pipefail
cd "$(dirname "$0")"

export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1

app=m14-l03-build
rm -rf bin obj

build() { # <dotnet build arguments...>; the build log is only shown on an error
  local log
  log="$(mktemp)"
  if ! dotnet build -v q "$@" >"$log" 2>&1; then
    cat "$log" >&2
    rm -f "$log"
    exit 1
  fi
  rm -f "$log"
}

run() { # <folder> <command...>
  local dir="$1"
  shift
  echo "\$ $*"
  (cd "$dir" && "$@")
}

echo "== dotnet build (no -c: the default configuration) =="
build
ls bin/Debug/net11.0
echo

echo "== dotnet build -c Release =="
build -c Release
ls bin/Release/net11.0
echo

echo "== What the configuration changes =="
for config in Debug Release; do
  optimize="$(dotnet msbuild -getProperty:Optimize -p:Configuration="$config")"
  debugtype="$(dotnet msbuild -getProperty:DebugType -p:Configuration="$config")"
  symbols="$(dotnet msbuild -getProperty:DefineConstants -p:Configuration="$config")"
  debug_defined=no
  tr ';' '\n' <<<"$symbols" | grep -x DEBUG >/dev/null && debug_defined=yes
  echo "$config: Optimize=$optimize, DebugType=$debugtype, DEBUG defined=$debug_defined"
done
echo

echo "== Ways to start the Release build =="
echo "\$ dotnet run -c Release"
dotnet run -c Release -v q
run bin/Release/net11.0 ./$app
run bin/Release/net11.0 dotnet $app.dll
echo

echo "== $app.runtimeconfig.json (framework part) =="
sed -n '/"tfm"/,/^    },/p' bin/Release/net11.0/$app.runtimeconfig.json
