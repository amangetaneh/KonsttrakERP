#!/usr/bin/env python3
"""Generate/check a portable inventory of locked dependency license declarations."""
import argparse
import hashlib
import json
import sys
import xml.etree.ElementTree as ET
from collections import Counter
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
# Exact SPDX equivalents of the names in the supplied Schedule 1; not a legal opinion.
LISTED = {"MIT", "Apache-2.0", "BSD-2-Clause", "BSD-3-Clause", "ISC", "0BSD", "Zlib", "BSL-1.0", "PostgreSQL", "Unlicense", "CC0-1.0", "CC-BY-4.0", "MPL-2.0", "EPL-2.0", "CC-BY-SA-4.0"}


def read_license_file(path, package_name):
    text = path.read_text(encoding="utf-8-sig")
    if "Permission is hereby granted, free of charge" in text and "The above copyright notice" in text:
        return "MIT"
    if "Apache License" in text and "Version 2.0" in text:
        return "Apache-2.0"
    return "LicenseRef-" + package_name.lower().replace(".", "-")


def inventory(args_check=False):
    overrides = json.loads((ROOT / "docs/license-overrides.json").read_text())
    packages = {}
    npm = json.loads((ROOT / "ERP/client/package-lock.json").read_text())
    for location, metadata in npm["packages"].items():
        if not location:
            continue
        name = metadata.get("name") or location.rsplit("node_modules/", 1)[1]
        key = "npm:" + name + "@" + metadata["version"]
        record = {
            "package": key, "license": metadata.get("license", "UNKNOWN"),
            "scope": "development" if metadata.get("dev", False) else "application",
            "source": metadata.get("resolved", "https://www.npmjs.com/package/" + name)
        }
        packages[key] = record
    cache_dirs = set()
    projects = ["ERP", "tests/ERP.Tests", "tests/ERP.SqlServer.Tests"]
    for project in projects:
        assets = json.loads((ROOT / project / "obj/project.assets.json").read_text())
        cache_dirs.update(Path(folder) for folder in assets["packageFolders"])
    for project in projects:
        locked = json.loads((ROOT / project / "packages.lock.json").read_text())
        for dependencies in locked["dependencies"].values():
            for name, metadata in dependencies.items():
                if "resolved" not in metadata:
                    continue
                version = metadata["resolved"]
                key = "nuget:" + name.lower() + "@" + version
                if key in packages:
                    if project == "ERP": packages[key]["scope"] = "application"
                    continue
                folder = next((base / name.lower() / version for base in cache_dirs if (base / name.lower() / version / (name.lower() + ".nuspec")).is_file()), None)
                if folder is None:
                    raise ValueError(f"Restore packages before checking licenses: {name} {version}")
                doc = ET.parse(folder / (name.lower() + ".nuspec"))
                node = next((element for element in doc.iter() if element.tag.split("}")[-1] == "license"), None)
                source = f"https://www.nuget.org/packages/{name}/{version}"
                license_id = "UNKNOWN"
                evidence = "nuspec SPDX expression"
                if node is not None:
                    license_id = node.text.strip()
                    if node.attrib.get("type") == "file":
                        license_path = (folder / license_id).resolve()
                        if folder.resolve() not in license_path.parents: raise ValueError("Invalid package license path")
                        license_id = read_license_file(license_path, name)
                        if license_id.startswith("LicenseRef-"):
                            notices = ROOT / "docs/third-party-license-texts"
                            notices.mkdir(exist_ok=True)
                            notice = notices / (name.lower() + ".txt")
                            if not notice.exists() or notice.read_bytes() != license_path.read_bytes():
                                if args_check:
                                    raise ValueError("License text snapshot is stale: " + name)
                                notice.write_bytes(license_path.read_bytes())
                        evidence = "bundled license file; sha256=" + hashlib.sha256(license_path.read_bytes()).hexdigest()
                if key in overrides:
                    license_id, source = overrides[key]["license"], overrides[key]["source"]
                    evidence = overrides[key]["note"]
                if license_id == "UNKNOWN": raise ValueError(f"Unresolved license: {key}")
                packages[key] = {"package": key, "license": license_id, "scope": "application" if project == "ERP" else "development", "source": source, "evidence": evidence}
    for record in packages.values():
        record["schedule1"] = "listed" if record["license"] in LISTED else "requires-clarification"
    return sorted(packages.values(), key=lambda record: record["package"])


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true", help="verify the committed inventory is current")
    parser.add_argument("--strict-policy", action="store_true", help="also fail for license names not explicitly listed in Schedule 1")
    args = parser.parse_args()
    try:
        records = inventory(args.check)
    except (ValueError, OSError) as error:
        parser.exit(1, str(error) + "\n")
    content = json.dumps({"schemaVersion": 1, "packages": records}, indent=2) + "\n"
    output = ROOT / "docs/dependency-licenses.json"
    if args.check:
        if not output.exists() or output.read_text() != content:
            parser.exit(1, "Dependency license inventory is stale. Run make licenses and review the changes.\n")
    else:
        output.write_text(content)
    pending = [row for row in records if row["schedule1"] != "listed"]
    counts = Counter(row["license"] for row in pending)
    print(f"Reviewed {len(records)} locked package versions; {len(pending)} license declarations need program-specific clarification: {dict(counts)}.")
    print("Unlisted names are disclosed in the inventory; they are not automatically classified as prohibited or approved.")
    return int(bool(pending) and args.strict_policy)


if __name__ == "__main__":
    sys.exit(main())
