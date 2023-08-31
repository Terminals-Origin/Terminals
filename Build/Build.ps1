#The ultimate build script to build release version of Terminals
#.\InstallPrerequisities.ps1;

$logFile = "Output\build.log";

if(Test-Path .\Output) {
    Remove-Item .\Output\* -Recurse -ErrorAction Stop | Tee-Object $logFile;
} else {
	mkdir .\Output
}

# Visual Studio 2022
$msbuild = "c:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe";

# github old build agent 2019, because the lastest doesnt contain dotnet 4 tool
if (-Not $(Test-Path $msbuild)) {
  $msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe";
}

$solutionFile = "..\Source\Terminals.sln";
dotnet restore $solutionFile  # already expecting new dotnet installed with Visual studio
# Compile the solution - the distributionrelease configuration contains installer, which is not normal configurations
& "$msbuild" $solutionFile /m /p:configuration=DistributionRelease /p:Platform='Mixed Platforms' /t:rebuild -restore /verbosity:diag | Tee-Object $logFile -Append;
 
.\PackOutput.ps1 | Tee-Object $logFile -Append;


exit $LastExitCode;