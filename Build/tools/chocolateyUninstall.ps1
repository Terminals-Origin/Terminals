$packageName = 'terminals' # arbitrary name for the package, used in messages
$installerType = 'msi' #only one of these: exe, msi, msu
$url = 'http://terminals.codeplex.com/downloads/get/' # download url
$silentArgs = '/qn /norestart' # "/s /S /q /Q /quiet /silent /SILENT /VERYSILENT" # try any of these to get the silent installer #msi is always /quiet
$validExitCodes = @(0) #please insert other valid exit codes here, exit codes for ms http://msdn.microsoft.com/en-us/library/aa368542(VS.85).aspx



try {
    $chocTempDir = Join-Path $env:TEMP "chocolatey"
	$tempDir = Join-Path $chocTempDir "$packageName"
	if (![System.IO.Directory]::Exists($tempDir)) {[System.IO.Directory]::CreateDirectory($tempDir) | Out-Null}
	$file = Join-Path $tempDir "$($packageName)Install.msi"
	Write-Debug "Getting uninstall-package for $packageName :`'$file`', args: `'$silentArgs`' ";
	Get-ChocolateyWebFile $packageName $file $url
	$msiArgs = "/x `"$file`" $silentArgs"
	Start-ChocolateyProcessAsAdmin "$msiArgs" 'msiexec' -validExitCodes $validExitCodes
	write-host "$packageName has been uninstalled."
  } catch {
    Write-ChocolateyFailure $packageName $($_.Exception.Message)
    throw
}