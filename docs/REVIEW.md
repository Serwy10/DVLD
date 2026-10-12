# Review and changes

## Security and portability

- Removed hardcoded SQL login credentials from all eleven data-access classes. Added one connection-string provider with an environment override and Windows-authentication defaults.
- Added salted PBKDF2-HMAC-SHA256 hashing with 600,000 iterations. New accounts and password changes store hashes. Existing plaintext passwords migrate on successful login after the provided column migration.
- Stopped reading passwords into user-edit forms. Blank password edits preserve the existing value. New passwords require at least eight characters in the UI, and password controls are masked.
- Replaced plaintext password remembering with username-only remembering. Missing/unreadable local preference files no longer prevent startup.
- Replaced personal Windows paths with relative bundled assets and `%LOCALAPPDATA%\DVLD`. Replaced references to missing icons with supplied assets.
- Improved person-photo updates: create the destination directory, preserve the original extension, avoid unnecessary copies, and delete a previous managed photo only after a successful database save.

## Correctness and data integrity

- First issuance creates/reuses the driver, inserts the license, and completes the application in one transaction. It requires the correct applicant/class, minimum age, and three distinct passed test types.
- Renewal/replacement creates the application, deactivates the old license, creates its replacement, and completes the application in one transaction. It checks current database state, including detention.
- A lost/damaged replacement preserves the original expiry and avoids charging the license-class fee again. Renewal uses the current class fee and validity period.
- International issuance validates the local license and checks for another current international license, then creates the application/license and completes the application together.
- Release creates its application, updates the current detention, and completes the application together. Repeat releases are rejected.
- Local application creation/update/deletion commits its related records together. Duplicate-class and age checks are enforced during creation/update. Class changes are rejected once tests have been scheduled.
- Test scheduling checks application state, open appointments, earlier passed test types, and stable test-type IDs. Both `writting` and `written` map to the written test rather than searching editable titles.
- Test recording and appointment locking commit together, and duplicate/future results are rejected. Failed result insertion leaves the appointment open.
- New demo schema adds foreign keys, validation constraints, and unique indexes for active license classes, open test appointments, and current detentions. These schema constraints are not automatically installed into an existing database.
- Save methods change from add to update only after successful insertion, preserving retry behavior.
- Fixed old detention selection, empty list schemas, pass/fail display, disabled change-password navigation, nested sign-out windows, and empty-selection context menu failures.
- Fixed searching by national number after an ID search and stale failed-license search state. First issuance now reads its own application ID instead of another open form's static DTO state.
- Escaped apostrophes and wildcard characters in DataView searches and validated numeric/boolean filters.
- Preserved exception stack traces and added a common UI error handler that avoids exposing database credentials.

## Repository preparation

- Removed `.vs`, `bin`, `obj`, and the unused empty business-layer project from the distributed source.
- Added `.gitignore`, `.gitattributes`, `.editorconfig`, a setup README, and a Windows walkthrough.
- Reconstructed a clean schema and the six SQL views used by the application. Added reference-data seeding and a synthetic-account bootstrap script.
- Added a regression-test project and a Windows GitHub Actions workflow with LocalDB integration checks.

## Validation performed

- Compiled all four projects' C# sources against .NET Framework 4.7.2 reference assemblies with Roslyn, C# 7.3, and warnings treated as errors: passed.
- Executed 20 password-hashing and DataView filter assertions on the available .NET runtime: passed.
- Parsed 118 C# files and checked all project/resource references: passed.
- Parsed 148 standalone/embedded SQL statements with Microsoft's T-SQL parser: passed.
- GitHub Actions Windows MSBuild, including resource generation: passed. SQL Server integration tests run in the supplied LocalDB CI job; consult the latest workflow result for their execution status. The LocalDB connection is explicitly configured for local IPC without TLS.
- The Windows GUI walkthrough and demo-account PowerShell script still require manual verification.

## Remaining scope

The application still uses synchronous ADO.NET calls and some static display DTOs. This review preserves the original three-layer WinForms design; it is not a conversion to ASP.NET Core or a production security certification. Filesystem photo writes and SQL changes are not one shared transaction, so a failed database save may leave an unused new photo file. Existing personal records and photos were not supplied or published.
