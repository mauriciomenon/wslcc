#Requires -Version 5.1
$language = [Globalization.CultureInfo]::CurrentUICulture.TwoLetterISOLanguageName
$messages = Get-Content -LiteralPath (Join-Path $PSScriptRoot 'build-messages.json') -Encoding UTF8 -Raw | ConvertFrom-Json
$buildText = if ($language -in 'zh', 'pt') { $messages.$language } else { $messages.en }

function Remove-UnusedLanguageResource {
    [CmdletBinding(SupportsShouldProcess)]
    param([Parameter(Mandatory)][string]$OutputDirectory)

    $outputRoot = (Resolve-Path -LiteralPath $OutputDirectory -ErrorAction Stop).ProviderPath
    $supported = 'en', 'en-US', 'en-GB', 'pt', 'pt-BR', 'zh', 'zh-CN', 'zh-Hans', 'zh-Hant', 'zh-TW'
    $cultures = [Globalization.CultureInfo]::GetCultures('AllCultures').Name
    foreach ($directory in Get-ChildItem -LiteralPath $outputRoot -Directory) {
        if ($directory.Name -in $cultures -and $directory.Name -notin $supported) {
            if ($directory.Parent.FullName -ne $outputRoot -or ($directory.Attributes -band [IO.FileAttributes]::ReparsePoint)) {
                throw ($buildText.UnsafeDirectory -f $directory.FullName)
            }
            if ($PSCmdlet.ShouldProcess($directory.FullName, 'Remove unused language resources')) {
                Remove-Item -LiteralPath $directory.FullName -Recurse -Force -ErrorAction Stop
            }
        }
    }
    Write-Information ($buildText.Languages -f ($supported -join ', ')) -InformationAction Continue
}
