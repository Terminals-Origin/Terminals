# Define variables
[String[]]$packageFiles = .\PackageFiles.ps1

$outputDir = ".\Output\";
$binOutput = "$outputDir\Release\net4.8\";
$commonAssembly = "..\Source\Terminals\Properties\Common.AssemblyInfo.cs";
$setupPath = "$outputDir\Release\TerminalsSetup.msi";

# Extract the variable from the Common.AssemblyInfo.cs
$versionLine = (Select-String -path $commonAssembly "Version").Line
$versionStart = $versionLine.IndexOf("(""");
$versionEnd = $versionLine.IndexOf(".*");
$version = $versionLine.SubString($versionStart+2, $versionEnd - $versionStart -2);

#Define output paths
$installerName = "TerminalsSetup_$version.msi";
$zipName = "Terminals_v$version.zip";
$zipPath = Join-Path $outputDir $zipName;
$installerTargetPath = Join-Path $outputDir $installerName;
$toZip = Join-Path $outputDir "toZip";

# Collect the outputs
if(!(Test-Path $outputDir)) {
    New-Item $outputDir -itemtype directory;
}

if(!(Test-Path $toZip)) {
    New-Item $toZip -itemtype directory;
}

Remove-Item $toZip\*.* -Recurse -Force;
Move-Item $setupPath $installerTargetPath;

#zip the deployable bits
for($index=0; $index -lt $packageFiles.Length;$index++) {
   $toCopy = Join-Path $binOutput $packageFiles[$index];
   $targetPath = Join-Path $toZip $packageFiles[$index];
   $targetDir = Split-Path -Path $targetPath;
   
   if(!(Test-Path $targetDir)) {
       New-Item $targetDir -itemtype directory;
   }

   Copy-Item $toCopy $targetPath;
}

Compress-Archive -Path "$toZip\*" -DestinationPath $zipPath;
