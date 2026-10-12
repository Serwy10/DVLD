-- Existing database only. Back it up before running this migration.
USE DVLD;
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;
ALTER TABLE dbo.Users ALTER COLUMN Password nvarchar(256) NOT NULL;
COMMIT;
GO
-- Existing plaintext passwords are upgraded to PBKDF2 on the next valid active-account login.
-- This does not remove plaintext passwords from accounts that have not logged in yet.
-- Rotate the old SQL login password and delete the old loginInfo.txt file on your PC.
