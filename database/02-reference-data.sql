USE DVLD;
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF NOT EXISTS (SELECT 1 FROM dbo.Countries)
    INSERT dbo.Countries(CountryName) VALUES(N'Egypt'),(N'Italy'),(N'Jordan'),(N'United Kingdom'),(N'United States');
IF NOT EXISTS (SELECT 1 FROM dbo.ApplicationTypes)
BEGIN
    SET IDENTITY_INSERT dbo.ApplicationTypes ON;
    INSERT dbo.ApplicationTypes(ApplicationTypeID,ApplicationTypeTitle,ApplicationFees) VALUES
    (1,N'New Local Driving License Service',15),(2,N'Renew Driving License Service',7),
    (3,N'Replacement for a Lost Driving License',10),(4,N'Replacement for a Damaged Driving License',5),
    (5,N'Release Detained Driving License',15),(6,N'New International License',51),(7,N'Retake Test',5);
    SET IDENTITY_INSERT dbo.ApplicationTypes OFF;
END;
IF NOT EXISTS (SELECT 1 FROM dbo.LicenseClasses)
BEGIN
    SET IDENTITY_INSERT dbo.LicenseClasses ON;
    INSERT dbo.LicenseClasses(LicenseClassID,ClassName,ClassDescription,MinimumAllowedAge,DefaultValidityLength,ClassFees) VALUES
    (1,N'Class 1 - Small motorcycle',N'Small motorcycle',16,5,15),
    (2,N'Class 2 - Heavy motorcycle license',N'Heavy motorcycle',18,5,30),
    (3,N'Class 3 - Ordinary driving license',N'Passenger car',18,10,20),
    (4,N'Class 4 - Commercial',N'Commercial vehicle',21,10,200),
    (5,N'Class 5 - Agricultural',N'Agricultural vehicle',21,10,50),
    (6,N'Class 6 - Small and medium bus',N'Small and medium bus',21,10,250),
    (7,N'Class 7 - Truck and heavy vehicle',N'Truck and heavy vehicle',21,10,300);
    SET IDENTITY_INSERT dbo.LicenseClasses OFF;
END;
IF NOT EXISTS (SELECT 1 FROM dbo.TestTypes)
BEGIN
    SET IDENTITY_INSERT dbo.TestTypes ON;
    INSERT dbo.TestTypes(TestTypeID,TestTypeTitle,TestTypeDescription,TestTypeFees) VALUES
    (1,N'Vision Test',N'Vision assessment',10),(2,N'Written Test',N'Theoretical driving knowledge',20),(3,N'Street Test',N'Practical driving assessment',30);
    SET IDENTITY_INSERT dbo.TestTypes OFF;
END;
COMMIT;
GO
-- Demo values are application examples, not official fees or legal requirements.
