#Requires -Version 5.1
# .\build.ps1 [-Configuration Debug|Release] [-Platform x64|arm64]

param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",
    [ValidateSet("x64", "arm64")]
    [string]$Platform = "x64"
)

$ErrorActionPreference = "Stop"
. (Join-Path $PSScriptRoot 'scripts\build-common.ps1')

$project = Join-Path $PSScriptRoot "src\WSLCC.App\WSLCC.App.csproj"

& (Join-Path $PSScriptRoot "scripts\fetch-wslc-sdk.ps1")

Write-Information "==> dotnet build WSLCC ($Configuration|$Platform)" -InformationAction Continue
dotnet build $project -c $Configuration -p:Platform=$Platform -p:RuntimeIdentifier=win-$Platform -v:m

if ($LASTEXITCODE -ne 0) {
    [Console]::Error.WriteLine(($buildText.BuildFailed -f $LASTEXITCODE))
    exit $LASTEXITCODE
}

$out = Join-Path $PSScriptRoot "src\WSLCC.App\bin\$Platform\$Configuration\net10.0-windows10.0.26100.0\win-$Platform\WSLCC.exe"
if (-not (Test-Path $out)) {
    $out = Join-Path $PSScriptRoot "src\WSLCC.App\bin\$Configuration\net10.0-windows10.0.26100.0\win-$Platform\WSLCC.exe"
}
Write-Information ($buildText.Built -f $out) -InformationAction Continue

Remove-UnusedLanguageResource -OutputDirectory (Split-Path $out -Parent)
