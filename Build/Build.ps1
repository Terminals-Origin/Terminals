#The ultimate build script to build release version of Terminals
#.\InstallPrerequisities.ps1;

$logFile = "Output\build.log";

if(Test-Path .\Output) {
    Remove-Item .\Output\* -Recurse -ErrorAction Stop | Tee-Object $logFile;
}

# Visual Studio 2022
$msbuild = "c:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe";
# Compile the solution - the distributionrelease configuration contains installer, which is not normal configurations
& "$msbuild" ..\Source\Terminals.sln /m /p:configuration=DistributionRelease /p:Platform='Any CPU' /t:rebuild | Tee-Object $logFile -Append;
 
# .\PackOutput.ps1 | Tee-Object $logFile -Append;


exit $LastExitCode;