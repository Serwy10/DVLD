-- New demo database only. Refuses to overwrite an existing DVLD database.
USE master;
GO
IF DB_ID(N'DVLD') IS NOT NULL
    THROW 51000, 'DVLD already exists. Use a separate SQL Server instance for the clean demo setup.', 1;
GO
CREATE DATABASE DVLD;
GO
USE DVLD;
GO
CREATE TABLE dbo.Countries (
    CountryID int IDENTITY PRIMARY KEY,
    CountryName nvarchar(100) NOT NULL UNIQUE
);
CREATE TABLE dbo.People (
    PersonID int IDENTITY PRIMARY KEY,
    NationalNo nvarchar(50) NOT NULL UNIQUE,
    FirstName nvarchar(50) NOT NULL,
    SecondName nvarchar(50) NOT NULL,
    ThirdName nvarchar(50) NULL,
    LastName nvarchar(50) NOT NULL,
    DateOfBirth datetime NOT NULL,
    Gendor tinyint NOT NULL CHECK (Gendor IN (0,1)),
    Address nvarchar(500) NOT NULL,
    Phone nvarchar(50) NOT NULL,
    Email nvarchar(254) NULL,
    NationalityCountryID int NOT NULL REFERENCES dbo.Countries(CountryID),
    ImagePath nvarchar(1024) NULL
);
CREATE TABLE dbo.Users (
    UserID int IDENTITY PRIMARY KEY,
    PersonID int NOT NULL UNIQUE REFERENCES dbo.People(PersonID),
    UserName nvarchar(50) NOT NULL UNIQUE,
    Password nvarchar(256) NOT NULL,
    IsActive bit NOT NULL
);
CREATE TABLE dbo.ApplicationTypes (
    ApplicationTypeID int IDENTITY PRIMARY KEY,
    ApplicationTypeTitle nvarchar(150) NOT NULL,
    ApplicationFees decimal(10,2) NOT NULL CHECK (ApplicationFees>=0)
);
CREATE TABLE dbo.LicenseClasses (
    LicenseClassID int IDENTITY PRIMARY KEY,
    ClassName nvarchar(150) NOT NULL UNIQUE,
    ClassDescription nvarchar(500) NOT NULL,
    MinimumAllowedAge tinyint NOT NULL,
    DefaultValidityLength tinyint NOT NULL CHECK (DefaultValidityLength>0),
    ClassFees decimal(10,2) NOT NULL CHECK (ClassFees>=0)
);
CREATE TABLE dbo.TestTypes (
    TestTypeID int IDENTITY PRIMARY KEY,
    TestTypeTitle nvarchar(100) NOT NULL,
    TestTypeDescription nvarchar(500) NOT NULL,
    TestTypeFees decimal(10,2) NOT NULL CHECK (TestTypeFees>=0)
);
CREATE TABLE dbo.Applications (
    ApplicationID int IDENTITY PRIMARY KEY,
    ApplicantPersonID int NOT NULL REFERENCES dbo.People(PersonID),
    ApplicationDate datetime NOT NULL,
    ApplicationTypeID int NOT NULL REFERENCES dbo.ApplicationTypes(ApplicationTypeID),
    ApplicationStatus tinyint NOT NULL CHECK (ApplicationStatus IN(1,2,3)),
    LastStatusDate datetime NOT NULL,
    PaidFees decimal(10,2) NOT NULL CHECK (PaidFees>=0),
    CreatedByUserID int NOT NULL REFERENCES dbo.Users(UserID)
);
CREATE TABLE dbo.LocalDrivingLicenseApplications (
    LocalDrivingLicenseApplicationID int IDENTITY PRIMARY KEY,
    ApplicationID int NOT NULL UNIQUE REFERENCES dbo.Applications(ApplicationID),
    LicenseClassID int NOT NULL REFERENCES dbo.LicenseClasses(LicenseClassID)
);
CREATE TABLE dbo.TestAppointments (
    TestAppointmentID int IDENTITY PRIMARY KEY,
    TestTypeID int NOT NULL REFERENCES dbo.TestTypes(TestTypeID),
    LocalDrivingLicenseApplicationID int NOT NULL REFERENCES dbo.LocalDrivingLicenseApplications(LocalDrivingLicenseApplicationID),
    AppointmentDate datetime NOT NULL,
    PaidFees decimal(10,2) NOT NULL CHECK (PaidFees>=0),
    CreatedByUserID int NOT NULL REFERENCES dbo.Users(UserID),
    IsLocked bit NOT NULL
);
CREATE TABLE dbo.Tests (
    TestID int IDENTITY PRIMARY KEY,
    TestAppointmentID int NOT NULL UNIQUE REFERENCES dbo.TestAppointments(TestAppointmentID),
    TestResult bit NOT NULL,
    Notes nvarchar(1000) NULL,
    CreatedByUserID int NOT NULL REFERENCES dbo.Users(UserID)
);
CREATE TABLE dbo.Drivers (
    DriverID int IDENTITY PRIMARY KEY,
    PersonID int NOT NULL UNIQUE REFERENCES dbo.People(PersonID),
    CreatedByUserID int NOT NULL REFERENCES dbo.Users(UserID),
    CreatedDate datetime NOT NULL
);
CREATE TABLE dbo.Licenses (
    LicenseID int IDENTITY PRIMARY KEY,
    ApplicationID int NOT NULL UNIQUE REFERENCES dbo.Applications(ApplicationID),
    DriverID int NOT NULL REFERENCES dbo.Drivers(DriverID),
    LicenseClass int NOT NULL REFERENCES dbo.LicenseClasses(LicenseClassID),
    IssueDate datetime NOT NULL,
    ExpirationDate datetime NOT NULL,
    Notes nvarchar(1000) NULL,
    PaidFees decimal(10,2) NOT NULL CHECK (PaidFees>=0),
    IsActive bit NOT NULL,
    IssueReason tinyint NOT NULL CHECK (IssueReason IN(1,2,3,4)),
    CreatedByUserID int NOT NULL REFERENCES dbo.Users(UserID),
    CHECK (ExpirationDate>IssueDate)
);
CREATE TABLE dbo.InternationalLicenses (
    InternationalLicenseID int IDENTITY PRIMARY KEY,
    ApplicationID int NOT NULL UNIQUE REFERENCES dbo.Applications(ApplicationID),
    DriverID int NOT NULL REFERENCES dbo.Drivers(DriverID),
    IssuedUsingLocalLicenseID int NOT NULL REFERENCES dbo.Licenses(LicenseID),
    IssueDate datetime NOT NULL,
    ExpirationDate datetime NOT NULL,
    IsActive bit NOT NULL,
    CreatedByUserID int NOT NULL REFERENCES dbo.Users(UserID),
    CHECK (ExpirationDate>IssueDate)
);
CREATE TABLE dbo.DetainedLicenses (
    DetainID int IDENTITY PRIMARY KEY,
    LicenseID int NOT NULL REFERENCES dbo.Licenses(LicenseID),
    DetainDate datetime NOT NULL,
    FineFees decimal(10,2) NOT NULL CHECK (FineFees>=0),
    CreatedByUserID int NOT NULL REFERENCES dbo.Users(UserID),
    IsReleased bit NOT NULL,
    ReleaseDate datetime NULL,
    ReleasedByUserID int NULL REFERENCES dbo.Users(UserID),
    ReleaseApplicationID int NULL REFERENCES dbo.Applications(ApplicationID),
    CHECK ((IsReleased=0 AND ReleaseDate IS NULL AND ReleasedByUserID IS NULL AND ReleaseApplicationID IS NULL)
        OR (IsReleased=1 AND ReleaseDate IS NOT NULL AND ReleasedByUserID IS NOT NULL AND ReleaseApplicationID IS NOT NULL))
);
CREATE UNIQUE INDEX UX_Licenses_ActiveClass ON dbo.Licenses(DriverID,LicenseClass) WHERE IsActive=1;
CREATE UNIQUE INDEX UX_TestAppointments_Open ON dbo.TestAppointments(LocalDrivingLicenseApplicationID,TestTypeID) WHERE IsLocked=0;
CREATE UNIQUE INDEX UX_DetainedLicenses_Open ON dbo.DetainedLicenses(LicenseID) WHERE IsReleased=0;
CREATE INDEX IX_Applications_PersonStatus ON dbo.Applications(ApplicantPersonID,ApplicationStatus);
CREATE INDEX IX_InternationalLicenses_Driver ON dbo.InternationalLicenses(DriverID,IsActive,ExpirationDate);
GO
CREATE VIEW dbo.LocalApplicationsView AS
SELECT l.LocalDrivingLicenseApplicationID AS LDLAppID,c.ClassName,p.NationalNo,
    CONCAT(p.FirstName,N' ',p.SecondName,N' ',p.LastName) AS FullName,a.ApplicationDate,
    (SELECT COUNT(DISTINCT t.TestTypeID) FROM dbo.TestAppointments t JOIN dbo.Tests r ON r.TestAppointmentID=t.TestAppointmentID
     WHERE t.LocalDrivingLicenseApplicationID=l.LocalDrivingLicenseApplicationID AND r.TestResult=1) AS PassedTestCount,
    CASE a.ApplicationStatus WHEN 1 THEN N'New' WHEN 2 THEN N'Cancelled' WHEN 3 THEN N'Completed' END AS AppStatus
FROM dbo.LocalDrivingLicenseApplications l JOIN dbo.Applications a ON a.ApplicationID=l.ApplicationID
JOIN dbo.People p ON p.PersonID=a.ApplicantPersonID JOIN dbo.LicenseClasses c ON c.LicenseClassID=l.LicenseClassID;
GO
CREATE VIEW dbo.retrivingApplicationDataView AS
SELECT l.LocalDrivingLicenseApplicationID AS LDLAppID,c.ClassName,
    (SELECT COUNT(DISTINCT t.TestTypeID) FROM dbo.TestAppointments t JOIN dbo.Tests r ON r.TestAppointmentID=t.TestAppointmentID
     WHERE t.LocalDrivingLicenseApplicationID=l.LocalDrivingLicenseApplicationID AND r.TestResult=1) AS PassedTestCount,
    a.ApplicationID, CASE a.ApplicationStatus WHEN 1 THEN N'New' WHEN 2 THEN N'Cancelled' WHEN 3 THEN N'Completed' END AS Status,
    a.PaidFees,at.ApplicationTypeTitle,CONCAT(p.FirstName,N' ',p.SecondName,N' ',p.LastName) AS FullName,
    a.ApplicationDate,a.LastStatusDate,u.UserName,a.ApplicantPersonID
FROM dbo.LocalDrivingLicenseApplications l JOIN dbo.Applications a ON a.ApplicationID=l.ApplicationID
JOIN dbo.LicenseClasses c ON c.LicenseClassID=l.LicenseClassID JOIN dbo.People p ON p.PersonID=a.ApplicantPersonID
JOIN dbo.ApplicationTypes at ON at.ApplicationTypeID=a.ApplicationTypeID JOIN dbo.Users u ON u.UserID=a.CreatedByUserID;
GO
CREATE VIEW dbo.DriverLicense_View AS
SELECT l.LicenseID,c.ClassName,CONCAT(p.FirstName,N' ',p.SecondName,N' ',p.LastName) AS FullName,p.NationalNo,
    CASE p.Gendor WHEN 0 THEN N'Male' ELSE N'Female' END AS Gender,l.IssueDate,l.ExpirationDate,
    CASE l.IssueReason WHEN 1 THEN N'First issue' WHEN 2 THEN N'Renewal' WHEN 3 THEN N'Lost replacement' WHEN 4 THEN N'Damaged replacement' END AS ApplicationTypeTitle,
    l.Notes,CASE l.IsActive WHEN 1 THEN N'Yes' ELSE N'No' END AS IsActive,l.DriverID,p.DateOfBirth,
    CASE WHEN EXISTS(SELECT 1 FROM dbo.DetainedLicenses d WHERE d.LicenseID=l.LicenseID AND d.IsReleased=0) THEN N'Yes' ELSE N'No' END AS IsDetained,p.ImagePath
FROM dbo.Licenses l JOIN dbo.Drivers d ON d.DriverID=l.DriverID JOIN dbo.People p ON p.PersonID=d.PersonID
JOIN dbo.LicenseClasses c ON c.LicenseClassID=l.LicenseClass;
GO
CREATE VIEW dbo.internationalDriverLicenseInfo_view AS
SELECT il.InternationalLicenseID,CONCAT(p.FirstName,N' ',p.SecondName,N' ',p.LastName) AS FullName,
    il.IssuedUsingLocalLicenseID,p.NationalNo,CASE p.Gendor WHEN 0 THEN N'Male' ELSE N'Female' END AS Gender,
    il.IssueDate,il.ExpirationDate,il.ApplicationID,il.DriverID,CASE il.IsActive WHEN 1 THEN N'Yes' ELSE N'No' END AS IsActive,p.DateOfBirth,p.ImagePath
FROM dbo.InternationalLicenses il JOIN dbo.Drivers d ON d.DriverID=il.DriverID JOIN dbo.People p ON p.PersonID=d.PersonID;
GO
CREATE VIEW dbo.DriversTable_View AS
SELECT d.DriverID,d.PersonID,p.NationalNo,CONCAT(p.FirstName,N' ',p.SecondName,N' ',p.LastName) AS FullName,
    d.CreatedDate,(SELECT COUNT(*) FROM dbo.Licenses l WHERE l.DriverID=d.DriverID AND l.IsActive=1) AS NumberOfActiveLicenses
FROM dbo.Drivers d JOIN dbo.People p ON p.PersonID=d.PersonID;
GO
CREATE VIEW dbo.detainedLicense_View AS
SELECT d.DetainID,d.LicenseID,d.DetainDate,d.IsReleased,d.FineFees,d.ReleaseDate,p.NationalNo,
    CONCAT(p.FirstName,N' ',p.SecondName,N' ',p.LastName) AS FullName,d.ReleaseApplicationID
FROM dbo.DetainedLicenses d JOIN dbo.Licenses l ON l.LicenseID=d.LicenseID
JOIN dbo.Drivers r ON r.DriverID=l.DriverID JOIN dbo.People p ON p.PersonID=r.PersonID;
GO
