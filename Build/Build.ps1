#The ultimate build script to build release version of Terminals
.\InstallPrerequisities.ps1

# Compile the solution - the distributionrelease configuration contains installer, which is not normal configurations
& $msbuild ..\Source\Terminals.sln /m /p:configuration=DistributionRelease /toolsversion:4.0 /t:rebuild

.\PackOutputs.ps1