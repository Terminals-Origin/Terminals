$url = 'https://github.com/Terminals-Origin/Terminals/releases/download/$Version/TerminalsSetup_$Version.msi';
$checksum = '';

$ErrorActionPreference = 'Stop';
$packageName= 'terminals';
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)";

$packageArgs = @{
  packageName   = $packageName
  unzipLocation = $toolsDir
  fileType      = 'msi'
  url           = $url

  softwareName  = 'terminals*'

  checksum      = $checksum
  checksumType  = 'sha256'

  silentArgs    = "/qn /norestart"
  validExitCodes= @(0, 3010, 1641)
};

Install-ChocolateyPackage @packageArgs;