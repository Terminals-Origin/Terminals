# Script to automatically generate the the chocolatey nuget package
Param(
	[Parameter(Mandatory=$true)]
	[string]$Version,
	[Parameter(Mandatory=$true)]
	[string]$DownloadId
)

$OutDir = ".\Output";
$OutTools = "$OutDir\tools\";
$nuspecFile = "$OutDir\terminals.nuspec";

md $OutTools -Force | Out-Null;
cp .\terminals.nuspec $OutDir -Force;

function PrepareScript($path) {
    $scriptToUpdate = gi $path;
    $chocoSCript = $(type $scriptToUpdate);
    $chocoSCript[2] = "`$url = 'http://terminals.codeplex.com/downloads/get/$DownLoadId'";
	echo $chocoSCript > $OutTools$($scriptToUpdate.Name);
}

PrepareScript ".\tools\chocolateyInstall.ps1";
PrepareScript ".\tools\chocolateyUninstall.ps1";

echo "creating package version:'$Version'";
choco pack $nuspecFile --version $Version --outputdirectory $OutDir;

rm $OutTools -Recurse -Force;
rm $nuspecFile -Force;

exit 0;