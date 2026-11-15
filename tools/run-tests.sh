#!/usr/bin/env bash
# Runs the tests of one test project or solution and prints a stable, sorted result list.
#
#   bash tools/run-tests.sh [--allow-failures] <path to .csproj or .slnx>
#
# Output (stdout), sorted by test name:
#   Passed <Class>.<Method>[(<arguments>)]
#   Failed <Class>.<Method>[(<arguments>)]
#         <assertion message lines, indented by 6 spaces>
#   Skipped <Class>.<Method>
#
# Exit code: 0 if all tests passed, or if --allow-failures is set and the test run itself
# worked (build ok, tests ran). Otherwise non-zero. On an error, the raw output goes to stderr.
#
# How it runs: `dotnet test` with Microsoft.Testing.Platform (MTP; global.json sets
# "test": { "runner": "Microsoft.Testing.Platform" }) and `--output Detailed`, which prints one
# block per test: a result line ("passed Name (12ms)"), a "from <dll>" line, and for failed
# tests the message and the stack trace. We chose `dotnet test` over starting the test
# executable directly because it builds first, accepts .csproj and .slnx alike, and its
# MTP output format is the same for every test framework.
# The script removes everything that is not stable: build lines, durations, "from <dll>"
# lines (paths), stack frames, skip reasons and the summary with times. The assertion message
# text itself comes unchanged from xUnit.
set -uo pipefail

allow_failures=false
if [[ "${1:-}" == "--allow-failures" ]]; then
  allow_failures=true
  shift
fi
if [[ $# -ne 1 ]]; then
  echo "Usage: bash tools/run-tests.sh [--allow-failures] <path to .csproj or .slnx>" >&2
  exit 64
fi
target="$1"

case "$target" in
  *.slnx | *.sln) kind=--solution ;;
  *) kind=--project ;;
esac

export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1

raw="$(dotnet test "$kind" "$target" --output Detailed --no-progress --no-ansi 2>&1)"
status=$?

# MTP exit codes (https://aka.ms/testingplatform/exitcodes): 0 = all passed, 2 = at least one
# test failed. Everything else (build error = 1, no tests ran = 8, ...) is a broken run.
ran=true
grep -qE '^Test run summary: ' <<<"$raw" || ran=false
if [[ $status -ne 0 ]] && ! { $allow_failures && [[ $status -eq 2 ]] && $ran; }; then
  printf '%s\n' "$raw" >&2
  echo "run-tests.sh: dotnet test failed with exit code $status" >&2
  exit "$status"
fi
if ! $ran; then
  printf '%s\n' "$raw" >&2
  echo "run-tests.sh: no test summary found" >&2
  exit 1
fi

# One record per test: "<name>\t<line>\x1f<line>...", then sort by name (C locale) and expand.
printf '%s\n' "$raw" | awk '
  function flush() { if (name != "") printf "%s\t%s\n", name, block; name = "" }
  /^(passed|failed|skipped) / {
    flush()
    result = $1
    line = substr($0, length(result) + 2)
    sub(/ \(([0-9]+(ms|s|m|h) ?)+\)$/, "", line)   # duration, for example (13ms) or (1s 20ms)
    name = line
    block = toupper(substr(result, 1, 1)) substr(result, 2) " " line
    keep = (result == "failed")                     # only failed tests show details
    next
  }
  /^[^ ]/ { flush(); next }                         # build lines, summary, end of a block
  name == "" { next }
  !keep { next }
  /^  from / { next }                               # path of the test assembly
  /^ +at / { next }                                 # stack frame
  /^ +--- End of / { next }                         # stack trace separator
  /^ *$/ { next }
  {
    line = substr($0, 3)                            # MTP indents details by 2 spaces
    sub(/^Xunit\.MicrosoftTestingPlatform\.XunitException: /, "", line)
    block = block "\037      " line
  }
  END { flush() }
' | LC_ALL=C sort -t $'\t' -k1,1 -s | cut -f2- | tr '\037' '\n'

exit 0
