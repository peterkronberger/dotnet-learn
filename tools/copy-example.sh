#!/usr/bin/env bash
# Copies one example folder under website/examples/_ci/ to a new folder, without build output.
#
#   bash tools/copy-example.sh <source-name> <new-name>
#
# Example (start of a SecLog step, see concept/project-seclog.md):
#   bash tools/copy-example.sh seclog-m15-l09 seclog-m16-l01
#
# Why this script exists: the implement workflow (claude-implement.yml) cannot run `cp -r`.
# Claude Code asks for manual approval for `cp` with flags, even if the allowed tools list
# contains a rule for it. `bash tools/...` is allowed, so the copy runs through this script.
#
# What it does:
#   - <source-name> and <new-name> are folder names (not paths) under website/examples/_ci/.
#     Only letters, digits, ".", "_" and "-" are allowed, so the script cannot leave that folder.
#   - The source folder must exist. The new folder must NOT exist (nothing is overwritten).
#   - It copies everything except the build output: bin/, obj/ and TestResults/ (at any depth).
#     These are ignored by git anyway, but a copy of a folder that was built would carry them.
#   - It prints the number of copied files.
#
# Next steps for a SecLog step: change only what the new requirement needs, run
# `bash tools/check-examples.sh --update`, review the snapshot diff, and show
# `diff -r -x bin -x obj -x TestResults <source> <new>` in the PR.
set -euo pipefail

if [[ $# -ne 2 ]]; then
  echo "Usage: bash tools/copy-example.sh <source-name> <new-name>" >&2
  exit 2
fi

src_name="$1"
dst_name="$2"
base="website/examples/_ci"

for name in "$src_name" "$dst_name"; do
  if [[ ! "$name" =~ ^[A-Za-z0-9][A-Za-z0-9._-]*$ || "$name" == *..* ]]; then
    echo "Invalid folder name: '$name' (allowed: letters, digits, '.', '_', '-')" >&2
    exit 2
  fi
done

# Run from the repository root, whatever the current directory is.
cd "$(dirname "${BASH_SOURCE[0]}")/.."

src="$base/$src_name"
dst="$base/$dst_name"

if [[ ! -d "$src" ]]; then
  echo "Source folder does not exist: $src" >&2
  exit 1
fi
if [[ -e "$dst" ]]; then
  echo "New folder already exists: $dst (not overwriting)" >&2
  exit 1
fi

mkdir -p "$dst"
tar -C "$src" \
    --exclude='bin' --exclude='obj' --exclude='TestResults' \
    -cf - . | tar -C "$dst" -xf -

count=$(find "$dst" -type f | wc -l)
echo "Copied $src -> $dst ($count files, without bin/obj/TestResults)"
