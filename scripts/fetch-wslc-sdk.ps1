#Requires -Version 5.1
# Microsoft distributes this SDK through the WSL releases on GitHub.
param(
    [string]$Version = "3.0.1"
)

$ErrorActionPreference = "Stop"
. (Join-Path $PSScriptRoot 'build-common.ps1')

$root = $PSScriptRoot | Split-Path -Parent
$feed = Join-Path $root "nuget-packages"
$target = Join-Path $feed "Microsoft.WSL.Containers.$Version.nupkg"

if (Test-Path $target) {
    Write-Information ($buildText.Cached -f $target) -InformationAction Continue
    exit 0
}

New-Item -ItemType Directory -Path $feed -Force | Out-Null

$url = "https://github.com/microsoft/WSL/releases/download/$Version/Microsoft.WSL.Containers.$Version.nupkg"
Write-Information ($buildText.Downloading -f $Version) -InformationAction Continue
Invoke-WebRequest -Uri $url -OutFile $target -TimeoutSec 180
Write-Information ($buildText.Downloaded -f $target) -InformationAction Continue
