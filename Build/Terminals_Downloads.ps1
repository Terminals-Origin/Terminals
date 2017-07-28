$releases = Invoke-RestMethod https://api.github.com/repos/Terminals-Origin/Terminals/releases;

foreach ($release in $releases) {
  $release.assets | Select-Object @{n="Version"; e={ $release.tag_name }}, @{n="Name"; e={ $_.name }}, @{n="Downloads"; e={ $_.download_count }};
}