#!/usr/bin/env bash
# Publishes m14-publish in every mode of lesson m14-l04 and shows the files and the program output.
#
#   bash run.sh           file names + program output per mode (snapshot: expected.out.txt)
#   bash run.sh --sizes   total size per mode in MB; changes between SDK versions, so it is
#                         not part of the snapshot (not used by tools/check-examples.sh)
#
# File list: the app files (names that start with "m14-publish") are listed by name. Other
# files are .NET runtime files: up to 10 are listed by name, more are summarized in one line
# without a count, because the count can change between SDK versions.
set -euo pipefail
cd "$(dirname "$0")"

sizes=false
[[ "${1:-}" == "--sizes" ]] && sizes=true

export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1

app=m14-publish
tmp="$(mktemp -d)"
trap 'rm -rf "$tmp"' EXIT
n=0

publish() { # <mode> <dotnet publish arguments...>; sets $out to the publish folder
  local mode="$1"
  shift
  n=$((n + 1))
  out="$tmp/out$n"
  # Own artifacts folder per mode, so no mode reuses the intermediate files of another.
  if ! dotnet publish "$app.csproj" -c Release --artifacts-path "$tmp/artifacts$n" \
      -o "$out" -v q "$@" >"$tmp/log$n" 2>&1; then
    echo "publish failed: $mode" >&2
    cat "$tmp/log$n" >&2
    exit 1
  fi
}

size_mb() { # <file or folder>
  du -sb "$1" | awk '{ printf "%.1f", $1 / 1048576 }'
}

list_files() { # <folder>
  local others
  (cd "$1" && find . -maxdepth 1 -type f -name "$app*" -printf '%f\n' | LC_ALL=C sort)
  others="$(cd "$1" && find . -type f -not -name "$app*" -printf '%P\n' | LC_ALL=C sort)"
  [[ -z "$others" ]] && return
  if [[ $(wc -l <<<"$others") -le 10 ]]; then
    printf '%s\n' "$others"
  else
    echo "+ .NET runtime files (not listed)"
  fi
}

run() { # <folder> <command...>
  local dir="$1"
  shift
  echo "\$ $*"
  (cd "$dir" && "$@")
}

section() { # <mode> <start commands as "cmd|cmd|...">
  local mode="$1" starts="$2" cmd
  if $sizes; then
    echo "$mode: $(size_mb "$out") MB"
    return
  fi
  echo "== $mode =="
  list_files "$out"
  IFS='|' read -ra cmds <<<"$starts"
  for cmd in "${cmds[@]}"; do
    read -ra words <<<"$cmd"
    run "$out" "${words[@]}"
  done
  echo
}

# 1. Framework-dependent (default): needs an installed .NET runtime.
publish framework-dependent -r linux-x64
section framework-dependent "./$app|dotnet $app.dll"

# 2. Framework-dependent without apphost: a cross-platform DLL, so no runtime identifier.
publish no-apphost -p:UseAppHost=false
section "framework-dependent, no apphost" "dotnet $app.dll"

# 3. Self-contained: the .NET runtime is copied next to the app.
publish self-contained -r linux-x64 --self-contained
section self-contained "./$app"

# 4. Self-contained single file.
publish single-file -r linux-x64 --self-contained -p:PublishSingleFile=true
section "self-contained, single file" "./$app"

# 5. ReadyToRun: the app is compiled ahead of time in addition to the IL code.
publish ready-to-run -r linux-x64 --self-contained -p:PublishReadyToRun=true
section "self-contained, ReadyToRun" "./$app"

# 6. Trimmed: unused code of the framework is removed.
publish trimmed -r linux-x64 --self-contained -p:PublishTrimmed=true
section "self-contained, trimmed" "./$app"

# 7. Native AOT: one native file, no JIT at run time. Needs clang on Linux.
publish native-aot -r linux-x64 -p:PublishAot=true
section "Native AOT" "./$app"

# 8. Container image as an archive: no container runtime needed, the image is not started.
publish container -r linux-x64 -t:PublishContainer \
  -p:ContainerArchiveOutputPath="$tmp/container/"
if $sizes; then
  echo "container image: $(size_mb "$tmp/container") MB"
else
  echo "== container image =="
  (cd "$tmp/container" && find . -type f -printf '%f\n' | LC_ALL=C sort)
fi
