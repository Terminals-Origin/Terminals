$url = 'https://terminals.codeplex.com/downloads/get/';
$checksum = '';

$ErrorActionPreference = 'Stop';
$packageName= 'terminals';
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)";

$packageArgs = @{
  packageName   = $packageName
  unzipLocation = $toolsDir
  fileType      = 'msi'
  url           = $url
  url64bit      = ''

  softwareName  = 'terminals*'

  checksum      = $checksum
  checksumType  = 'sha256'
  checksum64    = ''
  checksumType64= 'sha256'

  silentArgs    = "/qn /norestart"
  validExitCodes= @(0, 3010, 1641)
};

Install-ChocolateyPackage @packageArgs;