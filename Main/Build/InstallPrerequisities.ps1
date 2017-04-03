# Install prerequisities
# Terminals environment depends on some external components to be able run the build script
# Ensure the execution policy is set to unrestricted

# Install chocolatey using Nuget, which is already part of the solution
..\Source\.nuget\NuGet.exe install chocolatey -Version 0.10.5 -OutputDirectory ..\Source\packages
..\Source\packages\chocolatey.0.10.5\tools\chocolateyInstall.ps1
choco upgrade chocolatey -y;

# powershell community extensions to get the write-zip command let
choco install pscx -y;
Import-Module "c:\Program Files (x86)\PowerShell Community Extensions\Pscx3\Pscx\pscx";

# All packages to install development environment:
choco install wixtoolset -y;
choco install checksum -y;