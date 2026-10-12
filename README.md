# DVLD — Driving License Management

A C# Windows Forms application for managing people, user accounts, driving-license applications, tests, drivers, and licenses. Built with a three-layer architecture and SQL Server using ADO.NET.

## Features

- People and user account management, search, and account activation.
- Local driving-license applications and three test stages: vision, written, and street.
- First license issuance after all three test types are passed.
- License renewal and lost/damaged replacement.
- International licenses for eligible Class 3 drivers.
- License detention, release, and history.
- Editable application and test fees.

## Stack and structure

| Folder | Responsibility |
| --- | --- |
| `DVLD/` | Windows Forms screens, controls, input validation, and local images |
| `DVLDBusinessLogic2/` | Business objects and application workflow methods |
| `DVLDDataAccess/` | Parameterized SQL, connection settings, password hashing, and transactions |
| `database/` | Clean demo schema, reference data, and existing-database password migration |
| `tests/` | Regression checks and isolated SQL Server integration tests |
| `tools/` | Demo account creation |

Targets **.NET Framework 4.7.2**. This is a desktop application; it does not expose a REST API.

## Run on Windows

1. Install Visual Studio with the **.NET desktop development** workload and .NET Framework **4.7.2 targeting pack**.
2. Install SQL Server Express/Developer and SQL Server Management Studio. SQL Server 2016 or later is sufficient for the included demo scripts.
3. On a SQL Server instance that has **no existing `DVLD` database**, execute `database/01-schema.sql`, followed by `database/02-reference-data.sql`. The schema script refuses to overwrite an existing database. When using `sqlcmd`, include `-b` so an error stops execution:

   ```powershell
   sqlcmd -S . -E -b -i database/01-schema.sql
   sqlcmd -S . -E -b -i database/02-reference-data.sql
   ```

4. Set the `DVLD` connection string in `DVLD/App.config`. The default uses the local SQL Server instance and Windows authentication. For Express, use `Server=.\SQLEXPRESS`; for LocalDB, use `Server=(localdb)\MSSQLLocalDB`.
5. Open `DVLD.sln`, select **Release**, and build. Alternatively, use a Visual Studio Developer PowerShell:

   ```powershell
   msbuild DVLD.sln /m /p:Configuration=Release /p:Platform="Any CPU"
   ```

6. Create a synthetic demo account in **Windows PowerShell 5.1**. The script prompts for a password and stores only its hash:

   ```powershell
   .\tools\CreateDemoUser.ps1
   ```

   Pass `-ConnectionString` if your server differs from the default. Pass `-UserName` to choose a username other than `demo`.
7. Start `DVLD/bin/Release/DVLD.exe` and sign in with your chosen credentials.

The reference data includes five countries, seven license classes, seven application types, and three test types. Fees, ages, and validity periods are **demo business rules**, not official licensing requirements. Add more countries through SQL if needed.

## Connection settings

`DVLD_CONNECTION_STRING` takes precedence over `App.config`, so SQL credentials can stay outside source control. For a local SQL login:

```powershell
$env:DVLD_CONNECTION_STRING = 'Server=YOUR_SERVER;Database=DVLD;User ID=YOUR_APP_LOGIN;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True;'
```

Use a dedicated application login with the database permissions it needs. The included certificate-trust setting is for a local demo; configure a trusted server certificate and `TrustServerCertificate=False` for a real deployment.

## Existing database

Back up your database and execute **only** `database/03-password-storage-migration.sql` to widen `Users.Password` for hashes. The clean schema is a reconstruction of the model used by the source code; it is not a backup of the original database.

Existing plaintext passwords are upgraded on the next successful active-account login. Accounts that never log in still contain their old plaintext value until their password is changed. New accounts and password changes use salted **PBKDF2-HMAC-SHA256** with 600,000 iterations. An update with a blank password keeps the current hash.

The old SQL password was removed from the source. Rotate that SQL login password on your server and delete the old `loginInfo.txt` on your PC. The application now remembers only the username.

Photos and local state are stored under `%LOCALAPPDATA%\DVLD`. Old photo paths continue to work while the files exist on your machine; they are not migrated automatically.

## Tests

After building:

```powershell
.\tests\bin\Release\DVLD.Tests.exe $PWD.Path
```

For SQL integration tests, point the test server setting at a server where your Windows account can create and drop **temporary test databases**:

```powershell
$env:DVLD_TEST_SERVER_CONNECTION = 'Server=(localdb)\MSSQLLocalDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;'
.\tests\bin\Release\DVLD.Tests.exe $PWD.Path
```

Tests create a uniquely named `DVLD_Test_*` database, exercise the schema and workflows, and drop that database afterwards. They never use your normal `DVLD` database. The GitHub Actions workflow builds on Windows and runs these checks against a separate LocalDB instance.

See [the review](docs/REVIEW.md) for the changes and validation limits and [manual checks](docs/MANUAL_TESTS.md) for the Windows walkthrough.

## Project scope

This is an educational portfolio project. It uses synchronous SQL calls and a desktop login flow. It does not yet implement role-based authorization, an API, or automated GUI tests. Icons and images were supplied with the original project; no third-party asset license was included in the upload.
