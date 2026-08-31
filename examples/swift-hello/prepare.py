#!/usr/bin/env python3
"""Rewrite swiftc-emitted IR into a form the mock runtime can drive.

Swift emits a few loader-only metadata globals that are pure runtime
bookkeeping (they live in special __swift5_* / llvm.metadata sections and are
never executed):

  * @"\01l_entry_point"       - relative pointer to main, for the Swift loader
  * @llvm.used                - keep-alive list
  * @__swift_reflection_version

These are dropped. The two type-metadata symbols the program *does* reference
as opaque tags ($sSSN = String, $sypN = Any) are turned into local zeroed
globals so they resolve without a real Swift runtime. Nothing dereferences
them; the mock only uses them as pass-through tokens.

Usage: prepare.py hello.ll hello.poc.ll
"""
import re
import sys


def main() -> None:
    src, dst = sys.argv[1], sys.argv[2]
    out = []
    for line in open(src).read().splitlines():
        if re.match(r'^@"\\01l_entry_point"', line):
            continue
        if re.match(r'^@llvm\.used', line):
            continue
        if re.match(r'^@__swift_reflection_version', line):
            continue
        line = re.sub(r'^(@"\$sSSN" = )external global (%swift\.type).*$',
                      r'\1internal global \2 zeroinitializer, align 8', line)
        line = re.sub(r'^(@"\$sypN" = )external global (%swift\.full_existential_type).*$',
                      r'\1internal global \2 zeroinitializer', line)
        out.append(line)
    open(dst, 'w').write("\n".join(out) + "\n")


if __name__ == "__main__":
    main()
