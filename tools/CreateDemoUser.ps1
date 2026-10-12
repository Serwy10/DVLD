param(
    [string]$ConnectionString = 'Server=.;Database=DVLD;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;',
    [string]$UserName = 'demo',
    [string]$AssemblyPath = "$PSScriptRoot\..\DVLDDataAccess\bin\Release\DVLDDataAccess.dll"
)
$ErrorActionPreference = 'Stop'
if (!(Test-Path $AssemblyPath)) { throw 'Build the solution in Release mode before creating the demo account.' }
if ([string]::IsNullOrWhiteSpace($UserName) -or $UserName.Length -gt 50) { throw 'Use a username between 1 and 50 characters.' }
Add-Type -Path (Resolve-Path $AssemblyPath)
$secure = Read-Host 'Choose a demo password (at least 8 characters)' -AsSecureString
$pointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secure)
try {
    $plain = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($pointer)
    if ($plain.Length -lt 8) { throw 'Use at least 8 characters.' }
    $hash = [DVLDDataAccess.PasswordHasher]::Hash($plain)
} finally {
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($pointer)
    $plain = $null
}
$connection = New-Object System.Data.SqlClient.SqlConnection $ConnectionString
$connection.Open()
$transaction = $connection.BeginTransaction()
try {
    $command = $connection.CreateCommand()
    $command.Transaction = $transaction
    $command.CommandText = @'
IF EXISTS (SELECT 1 FROM dbo.Users WHERE UserName=@name) THROW 51001,'Username already exists.',1;
DECLARE @country int=(SELECT CountryID FROM dbo.Countries WHERE CountryName=N'Egypt');
IF @country IS NULL THROW 51002,'Run the reference-data script first.',1;
INSERT dbo.People(NationalNo,FirstName,SecondName,LastName,DateOfBirth,Gendor,Address,Phone,NationalityCountryID)
VALUES(@national,N'Demo',N'Portfolio',N'User','19950101',0,N'Synthetic demo address',N'0000000000',@country);
DECLARE @person int=CONVERT(int,SCOPE_IDENTITY());
INSERT dbo.Users(PersonID,UserName,Password,IsActive) VALUES(@person,@name,@hash,1);
'@
    [void]$command.Parameters.AddWithValue('@national', 'DEMO-' + [Guid]::NewGuid().ToString('N'))
    [void]$command.Parameters.AddWithValue('@name', $UserName)
    [void]$command.Parameters.AddWithValue('@hash', $hash)
    [void]$command.ExecuteNonQuery()
    $transaction.Commit()
    Write-Host "Demo account '$UserName' created."
} finally {
    if ($command) { $command.Dispose() }
    $transaction.Dispose()
    $connection.Dispose()
}
