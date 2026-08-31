# Swift "Hello, world!" via Iril

This example runs a Swift program on .NET by transpiling its LLVM IR with Iril
and linking it against a **mock `libswiftCore`** — demonstrating that Iril
faithfully transpiles Swift-emitted IR (issue #7).

```
Swift source ──swiftc──▶ LLVM IR ──Iril──▶ .NET assembly ──▶ "Hello, world!"
```

## Run

```sh
./run.sh
```

Requires `swiftc` and the Iril CLI (built from `../../Cli`).

## How it works

`swiftc -emit-ir` produces IR that only **declares** the Swift standard
library (`print`, `String`, `Array<Any>`, the runtime allocators, ...). Their
bodies live in `libswiftCore`, which Iril's StdLib does not provide.

Instead of shipping the real runtime, `swift_shim.ll` implements just the
handful of runtime entry points this program touches, with a small, internally
consistent ABI:

| Swift runtime symbol | mock behaviour |
| --- | --- |
| `…builtinStringLiteral…` | return `{ length, bytesPtr }` |
| `…allocateUninitializedArray…` | `malloc` a header + element slots |
| `…finalizeUninitializedArray…` | *(defined by Swift itself)* |
| `$sSaMa` (array metadata) | return zero |
| `…print…` | read element 0's String, `write()` it + the terminator |
| `swift_bridgeObjectRelease` | no-op |

The key trick: the shim is compiled **in the same Iril invocation** as the
program, so Swift's multi-value returns (`{ i64, ptr }`, `{ ptr, ptr }`) map to
the *same* generated anonymous struct types on both sides — the mock links with
no ABI shims. The mock only reproduces the data flow (`print` re-reads the exact
bytes `main` stored into the array), which is what makes the transpilation
observable end-to-end.

`prepare.py` drops three loader-only metadata globals (`@"\01l_entry_point"`,
`@llvm.used`, `@__swift_reflection_version`) that are never executed, and turns
the two type-metadata tags the program references (`$sSSN`, `$sypN`) into local
zeroed globals so they resolve without a real runtime.

## Scope

This is a transpilation demonstration, not a Swift runtime. The mock covers
only what `print("…")` exercises. Real Swift programs would need a real
`libswiftCore`.
