#Requires -Version 5.1
# .\publish.ps1 [-Configuration Release] [-Runtime win-x64] [-Framework] [-InnoSetup]
# -Framework requires the .NET 10 Desktop Runtime and Windows App Runtime on the target.
param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",
    [ValidateSet("win-x64", "win-arm64")]
    [string]$Runtime = "win-x64",
    [switch]$Framework,
    [switch]$InnoSetup
)

$ErrorActionPreference = "Stop"
. (Join-Path $PSScriptRoot 'scripts\build-common.ps1')

$project = Join-Path $PSScriptRoot "src\WSLCC.App\WSLCC.App.csproj"
$publishDir = Join-Path $PSScriptRoot "dist\publish"
$exe = Join-Path $publishDir "WSLCC.exe"

$selfContained = -not $Framework
$suffix = if ($Framework) { "-framework" } else { "" }
$mode = if ($Framework) { $buildText.Framework } else { $buildText.Standalone }

& (Join-Path $PSScriptRoot "scripts\fetch-wslc-sdk.ps1")

if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }
New-Item -ItemType Directory -Path $publishDir -Force | Out-Null

Write-Information "==> dotnet publish ($Configuration|$Runtime, $mode)" -InformationAction Continue
dotnet publish $project -c $Configuration -r $Runtime --self-contained $selfContained `
    -p:Platform=x64 -p:WSLCCFrameworkDependent=$($Framework.ToString().ToLowerInvariant()) `
    -p:PublishTrimmed=false -p:PublishReadyToRun=false -o $publishDir -v:m
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Remove-UnusedLanguageResource -OutputDirectory $publishDir

$info = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($exe)
$version = if ($info.ProductVersion) { $info.ProductVersion.Split('+')[0] } else { '0.1.0' }

$zip = Join-Path $PSScriptRoot "dist\WSLCC-$version-$Runtime$suffix.zip"
if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path "$publishDir\*" -DestinationPath $zip -Force
Write-Information ($buildText.Archive -f $zip) -InformationAction Continue

if ($InnoSetup) {
    $iscc = @(
        "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe",
        "C:\Program Files (x86)\Inno Setup 6\ISCC.exe",
        "C:\Program Files\Inno Setup 6\ISCC.exe"
    ) | Where-Object { Test-Path $_ } | Select-Object -First 1
    if (-not $iscc) { throw $buildText.MissingCompiler }
    $iss = Join-Path $PSScriptRoot "scripts\installer.iss"
    $dist = Join-Path $PSScriptRoot "dist"
    Write-Information $buildText.Installer -InformationAction Continue
    & $iscc "/DSourceDir=$publishDir" "/DAppVersion=$version" "/DOutputDir=$dist" "/DOutputSuffix=$suffix" "/DFrameworkDependent=$($Framework.ToString().ToLowerInvariant())" $iss
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
