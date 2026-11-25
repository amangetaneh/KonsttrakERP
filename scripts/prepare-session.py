#!/usr/bin/env python3
"""Create a source-only working copy without secrets, caches, or old Git objects."""
import argparse
import importlib.util
import shutil
import subprocess
from pathlib import Path

SCRIPT_DIR = Path(__file__).resolve().parent
spec = importlib.util.spec_from_file_location("secret_check", SCRIPT_DIR / "check-secrets.py")
secret_check = importlib.util.module_from_spec(spec)
spec.loader.exec_module(secret_check)


def protected(name):
    path = Path(name)
    if any(part in {".git", ".aws", ".ssh", "node_modules", "bin", "obj", "dist", "__pycache__"} for part in path.parts):
        return True
    if path.name.startswith(".env") and path.name != ".env.example":
        return True
    if path.suffix.lower() in {".pem", ".key", ".pfx", ".p12", ".db", ".sqlite", ".sqlite3", ".mdf", ".ldf", ".log", ".bundle", ".patch"}:
        return True
    if path.name.lower().startswith("credentials") or path.name.lower().endswith(".local.json"):
        return True
    if path.suffix.lower() in {".pdf", ".docx", ".xlsx", ".xls", ".csv"} and name not in {"ERP/UploadedFiles/blank_forms.pdf", "ERP/UploadedFiles/blank_forms.docx"}:
        return True
    return "UploadedFiles" in path.parts and path.name not in {".gitignore", "blank_forms.pdf", "blank_forms.docx"}


def export_session(source, destination):
    source, destination = source.resolve(), destination.resolve()
    if destination == source or source in destination.parents:
        raise ValueError("Choose a new folder outside the source repository.")
    if destination.exists():
        raise ValueError("The destination must not exist; existing files are never overwritten.")
    names = subprocess.check_output(["git", "-C", str(source), "ls-files", "--cached", "--others", "--exclude-standard", "-z"]).decode().split("\0")
    selected = []
    for name in sorted(set(names) - {""}):
        path = source / name
        if protected(name) or not path.exists():
            continue
        if path.is_symlink() or source not in path.resolve().parents:
            raise ValueError(f"Refusing to export a symlink or external file: {name}")
        if not path.is_file():
            continue
        if secret_check.findings(name, path.read_bytes()):
            raise ValueError(f"Potential credential in {name}; export stopped without copying files.")
        selected.append((name, path))
    destination.mkdir(parents=True, mode=0o700)
    for name, path in selected:
        target = destination / name
        target.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(path, target)
    # A fresh object database cannot expose removed material through reflogs/internal refs.
    subprocess.run(["git", "init", "--quiet", "--initial-branch=main", str(destination)], check=True)
    return len(selected)


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--output", type=Path, required=True, help="new directory outside this repository")
    args = parser.parse_args()
    try:
        count = export_session(SCRIPT_DIR.parent, args.output)
    except (ValueError, subprocess.CalledProcessError) as error:
        parser.exit(1, str(error) + "\n")
    print(f"Exported {count} source files to {args.output.resolve()}. No history, credentials, or installed dependencies were copied.")
    print("This is a separate working copy. Keep the canonical repository for submission and contribution history.")


if __name__ == "__main__":
    main()
