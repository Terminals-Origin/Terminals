# Install prerequisities
# Terminals environment depends on some external components to be able run the build script
# Ensure the execution policy is set to unrestricted

# Install chocolatey using Nuget, which is already part of the solution
iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
..\Source\packages\chocolatey.0.10.5\tools\chocolateyInstall.ps1
choco upgrade chocolatey -y;

# All packages to install development environment:
choco install wixtoolset -y;
choco install checksum -y;