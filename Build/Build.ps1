#The ultimate build script to build release version of Terminals
#.\InstallPrerequisities.ps1;

$logFile = "Output\build.log";

if(Test-Path .\Output) {
    Remove-Item .\Output\* -Recurse -ErrorAction Stop | Tee-Object $logFile;
}

# Visual Studio 2015
$msbuild = "c:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe";
# Compile the solution - the distributionrelease configuration contains installer, which is not normal configurations
invoke-expression "$msbuild ..\Source\Terminals.sln /m /p:configuration=DistributionRelease /p:Platform='Any CPU' /toolsversion:4.0 /t:rebuild" | Tee-Object $logFile -Append;
 
.\PackOutput.ps1 | Tee-Object $logFile -Append;


exit $LastExitCode;