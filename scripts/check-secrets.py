#!/usr/bin/env python3
"""Check publishable files or main's history without printing sensitive values."""
import argparse
import re
import subprocess
import sys
from pathlib import Path

RULES = {
    "private key": re.compile(rb"-----BEGIN (?:RSA |EC |DSA |OPENSSH )?PRIVATE KEY-----"),
    "provider credential": re.compile(rb"\b(?:AKIA|ASIA)[A-Z0-9]{16}\b|\bgh[pousr]_[A-Za-z0-9]{30,}|\bsk_live_[A-Za-z0-9]{16,}"),
    "literal credential": re.compile(rb'''(?i)["']?(?:password|pwd|secret|api[_-]?key|token)["']?\s*[:=]\s*["']([^"'\r\n]+)["']'''),
    "connection password": re.compile(rb"(?i)(?:password|pwd)\s*=\s*([^;\s'\"]+)"),
}
SAFE = re.compile(rb"(?i)^(?:CHANGE_ME|synthetic|incorrect|short$|demo|placeholder|\{token\})")


def findings(path, data):
    # Test files are scanned too. Only explicitly annotated scanner fixtures are exempt.
    fixture = path.startswith("tests/") or ".test." in path
    result = []
    for rule, pattern in RULES.items():
        for match in pattern.finditer(data):
            line_data = data[data.rfind(b"\n", 0, match.start()) + 1:data.find(b"\n", match.end()) if b"\n" in data[match.end():] else len(data)]
            if rule in ("literal credential", "connection password"):
                if fixture and b"# synthetic scanner fixture" in line_data:
                    continue
                value = match.group(1)
                if rule == "connection password" and path.endswith(".sh") and re.fullmatch(rb"\$(?:[A-Za-z_][A-Za-z0-9_]*|\{[A-Za-z_][A-Za-z0-9_]*\})", value):
                    continue
                if SAFE.match(value) or value.strip() in (b"", b"bearer ", b"Bearer "):
                    continue
            if rule == "connection password":
                if not re.search(rb"(?i)(?:server|data source|database)\s*=", line_data):
                    continue
            line = data[:match.start()].count(b"\n") + 1
            result.append((path, line, rule))
    return result


def worktree():
    files = subprocess.check_output(["git", "ls-files", "--cached", "--others", "--exclude-standard", "-z"])
    for name in sorted(set(files.decode().split("\0")) - {""}):
        path = Path(name)
        if path.is_file():
            yield name, path.read_bytes()


def history():
    objects = subprocess.check_output(["git", "rev-list", "--objects", "main"]).decode().splitlines()
    process = subprocess.Popen(["git", "cat-file", "--batch"], stdin=subprocess.PIPE, stdout=subprocess.PIPE)
    try:
        for entry in objects:
            oid, _, name = entry.partition(" ")
            if not name:
                continue
            process.stdin.write((oid + "\n").encode())
            process.stdin.flush()
            header = process.stdout.readline().split()
            data = process.stdout.read(int(header[2]))
            process.stdout.read(1)
            if header[1] == b"blob":
                yield name, data
    finally:
        process.stdin.close()
        process.stdout.close()
        process.wait()


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--history", action="store_true", help="check all blobs reachable from main")
    args = parser.parse_args()
    problems = set()
    count = 0
    for name, data in history() if args.history else worktree():
        count += 1
        problems.update(findings(name, data))
    for name, line, rule in sorted(problems):
        print(f"{name}:{line}: potential {rule} (value redacted)")
    print(f"Scanned {count} {'historical blobs' if args.history else 'files'}; {len(problems)} potential secrets.")
    return bool(problems)


if __name__ == "__main__":
    sys.exit(main())
