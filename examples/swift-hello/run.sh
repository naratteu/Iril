#!/usr/bin/env bash
#
# End-to-end Swift "Hello, world!" through Iril.
#
#   Swift source ──swiftc──▶ LLVM IR ──Iril──▶ .NET assembly ──▶ prints output
#
# The generated IR only *declares* the Swift standard library (print, String,
# Array, ...). Rather than ship libswiftCore, we compile a tiny mock runtime
# (swift_shim.ll) ALONGSIDE the program. Because Iril compiles both in one
# module, the multi-value Swift returns ({i64,ptr}, ...) share Iril's generated
# anonymous struct types, so the mock links with zero ABI glue.
#
# Requires: swiftc, and the Iril CLI (built from ../../Cli).
set -euo pipefail

here="$(cd "$(dirname "$0")" && pwd)"
root="$(cd "$here/../.." && pwd)"
out="${TMPDIR:-/tmp}/iril-swift-hello"
mkdir -p "$out"

echo "1/4  swiftc -emit-ir"
swiftc -emit-ir "$here/hello.swift" -o "$out/hello.ll"

echo "2/4  strip loader metadata"
python3 "$here/prepare.py" "$out/hello.ll" "$out/hello.poc.ll"

echo "3/4  iril (program + mock runtime)"
dotnet run --project "$root/Cli/Cli.csproj" -c Debug -- \
    "$out/hello.poc.ll" "$here/swift_shim.ll" -o "$out/hello.dll"

echo "4/4  run"
( cd "$out" && dotnet exec hello.dll )
