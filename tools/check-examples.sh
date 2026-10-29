#!/usr/bin/env bash
# Verifies all examples under website/examples/ (spike S2).
#
#   tools/check-examples.sh            compare outputs with the snapshots (<name>.out.txt)
#   tools/check-examples.sh --update   rewrite the snapshots from the actual run
#
# Kinds of examples:
#   *.cs            file-based app (`dotnet run file.cs`), output must match <name>.out.txt.
#                   Console input comes from <name>.in.txt if it exists, otherwise from /dev/null.
#   *.snippet.cs    fragment, skipped (cannot run on its own)
#   _ci/<name>/     regular project, run by _ci/<name>/run.sh, output must match _ci/<name>/expected.out.txt
set -euo pipefail

root="$(cd "$(dirname "$0")/.." && pwd)"
examples="$root/website/examples"
update=false
[[ "${1:-}" == "--update" ]] && update=true

# Same culture behavior as the playground (InvariantGlobalization).
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1

failed=0
checked=0

check() { # <label> <snapshot> <command...>
  local label="$1" snapshot="$2"
  shift 2
  local actual
  if ! actual="$("$@" 2>&1)"; then
    echo "FAIL  $label: run failed"
    echo "$actual" | sed 's/^/      /'
    failed=$((failed + 1))
    return
  fi
  checked=$((checked + 1))
  if $update; then
    printf '%s\n' "$actual" >"$snapshot"
    echo "WROTE $label"
  elif [[ ! -f "$snapshot" ]]; then
    echo "FAIL  $label: snapshot missing (run: tools/check-examples.sh --update)"
    failed=$((failed + 1))
  elif ! diff -u "$snapshot" <(printf '%s\n' "$actual") >/tmp/example-diff.txt; then
    echo "FAIL  $label: output differs from snapshot"
    sed 's/^/      /' /tmp/example-diff.txt
    failed=$((failed + 1))
  else
    echo "OK    $label"
  fi
}

# 1. Single-file examples
while IFS= read -r -d '' file; do
  rel="${file#"$examples"/}"
  case "$rel" in
    _ci/*) continue ;;
    *.snippet.cs) echo "SKIP  $rel (snippet)"; continue ;;
  esac
  input="${file%.cs}.in.txt"
  [[ -f "$input" ]] || input=/dev/null
  check "$rel" "${file%.cs}.out.txt" dotnet run "$file" <"$input"
done < <(find "$examples" -name '*.cs' -not -path '*/bin/*' -not -path '*/obj/*' -print0 | sort -z)

# 2. CI projects (EF Core, Minimal API, xUnit)
for dir in "$examples"/_ci/*/; do
  [[ -f "$dir/run.sh" ]] || continue
  name="_ci/$(basename "$dir")"
  check "$name" "$dir/expected.out.txt" bash "$dir/run.sh"
done

echo "Checked: $checked, failed: $failed"
[[ $failed -eq 0 ]]
