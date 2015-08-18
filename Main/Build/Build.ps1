#The ultimate build script to build release version of Terminals

# Include files in the zip packages - this should reflect the wix components
[String[]]$packageFiles = ("AWSSDK.dll",
"AxInterop.MSTSCLib.dll",
"AxInterop.VMRCClientControlLib.dll",
"AxWFICALib.dll",
"EntityFramework.dll",
"EntityFramework.xml",
"FlickrNet.dll",
"Granados.dll",
"HexEditor.dll",
"ICSharpCode.SharpZipLib.dll",
"Interop.VMRCClientControlLib.dll",
"log4net.dll",
"log4net.xml",
"Metro.dll",
"MSTSCLib.dll",
"SharpPcap.dll",
"SqlScriptRunner.dll",
"SSHClient.dll",
"TabControl.dll",
"TerminalEmulator.dll",
"Terminals.exe",
"Terminals.exe.config",
"Terminals.log4net.config",
"terminalsicon.ico",
"ToolStrip.settings.config",
"TransPort2006.Design.dll",
"TransPort2006.dll",
"VncSharp.dll",
"WFICALib.dll",
"wpcap.dll",
"ZedGraph.dll",
"ZedGraph.xml",
"zlib.net.dll",
"Terminals.External.dll");


# Define variables
$version = "3.6.1"  # Extract the variable from the Common.AssemblyInfo.cs
$outputDir = ".\Output\";
$binOutput = "..\Source\Terminals\bin\Release\";
$commonAssembly = "..\Source\Terminals\Properties\Common.AssemblyInfo.cs";
$setupPath = "..\Source\TerminalsSetup\bin\Release\TerminalsSetup.msi";
$installerName = "TerminalsSetup_$version.msi";
$zipName = "Terminals_v$version.zip";
$zipPath = Join-Path $outputDir $zipName;
$installerTargetPath = Join-Path $outputDir $installerName;
$msbuild = "c:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe";

# Install prerequisities
# consider chocolatey install and then install Wix or use Nuget directly, if Wix is not installed
..\Source\.nuget\NuGet.exe install chocolatey -Version 0.9.9.8 -OutputDirectory ..\Source\packages
..\Source\packages\chocolatey.0.9.9.8\tools\chocolateyInstall.ps1
# powershell community extensions to get the write-zip command let
choco install pscx -y;
Import-Module "c:\Program Files (x86)\PowerShell Community Extensions\Pscx3\Pscx\pscx";

# Compile the solution - the distributionrelease configuration contains installer, which is not normal configurations
& $msbuild ..\Source\Terminals.sln /m /p:configuration=DistributionRelease /toolsversion:4.0 /t:clean /t:build
 
# Collect the outputs
if(!(Test-Path $outputDir))
{
    New-Item $outputDir -itemtype directory;
}

Remove-Item $outputDir\*.* -Recurse -Force;
Move-Item $setupPath $installerTargetPath;
#zip the deployable bits
for($index=0; $index -lt $packageFiles.Length;$index++)
{
   $packageFiles[$index] = Join-Path $binOutput $packageFiles[$index];
}

Write-Zip -Path $packageFiles $zipPath;




