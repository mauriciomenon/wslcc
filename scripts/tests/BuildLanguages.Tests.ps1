#Requires -Version 5.1
Describe 'Build languages' {
    It 'loads matching build messages and falls back to English' {
        $original = [Globalization.CultureInfo]::CurrentUICulture
        try {
            foreach ($culture in 'pt-BR', 'zh-CN', 'en-US', 'de-DE') {
                [Globalization.CultureInfo]::CurrentUICulture = [Globalization.CultureInfo]::GetCultureInfo($culture)
                . (Join-Path $PSScriptRoot '..\build-common.ps1')
                $expected = @{ 'pt-BR' = 'pt'; 'zh-CN' = 'zh'; 'en-US' = 'en'; 'de-DE' = 'en' }[$culture]
                if ($buildText.Built -ne $messages.$expected.Built) { throw "Incorrect language for $culture" }
                if (Compare-Object $messages.en.PSObject.Properties.Name $buildText.PSObject.Properties.Name) {
                    throw "Mismatched message keys for $culture"
                }
            }
        } finally {
            [Globalization.CultureInfo]::CurrentUICulture = $original
        }
    }

    It 'keeps application data and all three supported languages' {
        . (Join-Path $PSScriptRoot '..\build-common.ps1')
        $keep = 'en-US', 'pt', 'pt-BR', 'zh-CN', 'Assets', 'data', 'Microsoft.UI.Xaml'
        foreach ($name in $keep + @('fr', 'nl-NL')) {
            New-Item -ItemType Directory -Path (Join-Path $TestDrive $name) | Out-Null
        }
        Remove-UnusedLanguageResource -OutputDirectory $TestDrive
        $actual = (Get-ChildItem -LiteralPath $TestDrive -Directory).Name
        if (Compare-Object $keep $actual) { throw 'Incorrect language folders in build output' }
    }
}
