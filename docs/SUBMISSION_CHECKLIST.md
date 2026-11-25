# Submission readiness

Completed repository work:

- [x] MIT license and matching project metadata.
- [x] Safe environment example, ignored local secrets/data, and passing credential scans.
- [x] Reproducible terminal install/build/test/lint commands and dependency lockfiles.
- [x] Zero C# build warnings; warnings fail future builds.
- [x] Passing synthetic backend, browser-component, and cleanup-script tests.
- [x] Updated dependency/license inventory with notices and source evidence.
- [x] Private recovery material outside the repository, with verified checksums.
- [x] Source-only session export that excludes private material and old Git objects.
- [x] Separate SQL Server integration test suite and CI job.

Still requiring an external step or review:

- [ ] Run the SQL Server CI job or `make test-sql` on a supported x64 host. Compilation alone is not proof that migrations and relational queries pass.
- [ ] Review the exact license-name flags in `THIRD_PARTY_NOTICES.md` for your submission's requirements. The application is MIT; third-party dependencies keep their own terms.
- [ ] Publish the reviewed branch to your chosen GitHub repository and use its actual URL and license in the submission. The repository is currently local-only; configure your chosen remote explicitly when you decide to publish. No changes have been pushed by this cleanup.

`make check` verifies the local software checks. This checklist records technical preparation; it does not sign, submit, or certify an agreement on your behalf.
