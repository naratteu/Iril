#!/usr/bin/env sh
set -eu

root=$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)
build="$root/build"
rm -rf "$build"
mkdir "$build"
ponyc --pass=ir --output "$build" "$root"
dotnet run --project "$root/../../Cli/Cli.csproj" -- -o "$build/PonyHello.dll" \
  "$build/pony-hello.ll" "$root/pony_hello_runtime.c"
cp "$root/../../Cli/bin/Debug/net7.0/Cli.runtimeconfig.json" "$build/PonyHello.runtimeconfig.json"
dotnet "$build/PonyHello.dll"
