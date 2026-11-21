$ErrorActionPreference = 'Stop'
$src = Join-Path (Split-Path $PSScriptRoot) 'src'
$resources = Join-Path $src 'WSLCC.Core/Localization'

function Read-Catalog($name) {
    $file = Join-Path $resources $name
    [xml]$xml = [IO.File]::ReadAllText($file)
    $catalog = [Collections.Generic.Dictionary[string,string]]::new([StringComparer]::Ordinal)
    foreach ($entry in $xml.SelectNodes('/root/data')) {
        $key = $entry.GetAttribute('name')
        $value = [string]$entry.value
        if ([string]::IsNullOrWhiteSpace($key) -or [string]::IsNullOrWhiteSpace($value)) {
            throw "$name contains an empty key or translation ('$key')."
        }
        $catalog.Add($key, $value)
    }
    if (!$catalog.Count) { throw "$name contains no translations." }
    return ,$catalog
}

function Get-FormatIndices([string]$value) {
    $indices = foreach ($match in [regex]::Matches($value, '\{\{|\{(\d+)(?=[,:}])')) {
        if ($match.Groups[1].Success) { [int]$match.Groups[1].Value }
    }
    return ($indices | Sort-Object -Unique) -join ','
}

$english = Read-Catalog 'Strings.resx'
foreach ($language in 'pt-BR', 'zh-Hans') {
    $catalog = Read-Catalog "Strings.$language.resx"
    foreach ($key in $english.Keys) {
        if (!$catalog.ContainsKey($key)) { throw "$language is missing '$key'." }
        if ((Get-FormatIndices $catalog[$key]) -ne (Get-FormatIndices $english[$key])) {
            throw "$language has different format indices for '$key'."
        }
    }
    foreach ($key in $catalog.Keys) {
        if (!$english.ContainsKey($key)) { throw "$language has an unexpected key '$key'." }
    }
}

$patterns = @{
    '.cs'   = '\bL\.Get(?:Format)?\s*\(\s*"([^"]+)"\s*(?=[,)])'
    '.xaml' = '\{loc:Tr\s+Key\s*=\s*([^\s,}]+)\s*\}'
}
$count = 0
foreach ($file in Get-ChildItem $src -Recurse -File -Include '*.cs', '*.xaml') {
    if ($file.FullName -match '[/\\](bin|obj)[/\\]') { continue }
    $content = [IO.File]::ReadAllText($file.FullName)
    foreach ($match in [regex]::Matches($content, $patterns[$file.Extension])) {
        $key = $match.Groups[1].Value
        if (!$english.ContainsKey($key)) { throw "$($file.Name) references missing key '$key'." }
        $count++
    }
}
Write-Output "Validated $($english.Count) translations per language and $count literal references."
