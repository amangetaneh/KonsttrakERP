# Third-party dependencies

The ERP source changes are licensed under MIT. Dependencies retain their own licenses and copyright notices; the root LICENSE does not relicense them.

The versioned inventory is [docs/dependency-licenses.json](docs/dependency-licenses.json). It covers npm lockfile entries and the application, unit-test, and SQL-test NuGet lockfiles, including transitive dependencies. Each record identifies the version, declared/resolved license, application or development scope, and package/license source. `make licenses` regenerates it after intentional dependency changes; `make lint` checks that it is current.

The unused JsonModelBinder dependency was removed. MimeTypeMapOfficial was replaced with ASP.NET Core's built-in MIME provider. Both previously required a manual license metadata review. The legacy xunit.abstractions metadata is resolved to Apache-2.0 using its upstream license URL, with evidence recorded in `docs/license-overrides.json`.

The SQL client dependency tree includes Microsoft native components under Microsoft Software License Terms. Their supplied texts are retained in [docs/third-party-license-texts](docs/third-party-license-texts). These are not labeled MIT. Retain dependency-supplied license/notice files when redistributing packages or application bundles; the inventory is not a substitute for those notices or any applicable source-availability obligations.

## Submission-specific review

The inventory compares license names with the supplied agreement's Schedule 1. `requires-clarification` means the exact name is not explicitly listed; it does not mean that a component is automatically prohibited or approved. The outstanding names include development-tool licenses (BlueOak-1.0.0 and MIT-0) and Microsoft native-component terms. Program acceptance of these dependencies must be determined by the program, not inferred from the application's MIT license.

Run `python3 scripts/check-licenses.py --strict-policy` to turn any such declaration into a failing check. Normal build checks disclose and track these declarations without claiming contractual eligibility.
