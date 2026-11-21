# ADR-0007. Compilar com a CLI dotnet e evitar a falha do XamlCompiler no Windows em chines

Estado: Aceita

## Contexto

No Windows local em chines (.NET 10 SDK e Windows App SDK 2.4.0/WinUI 2.3.6), `dotnet build` chama `XamlCompiler.exe` como subprocesso net472 (`UseXamlCompilerExecutable=true`). Na Pass2, o processamento de XamlTypeInfo gera WMC9999 (NullReference, referencia de objeto sem instancia) e WMC1509 (No LocalAssembly). O caso esta relacionado a microsoft/microsoft-ui-xaml#11157, sobre nomes de recursos do XamlCompiler que divergem em sistemas em outros idiomas. Executar no mesmo processo com `UseXamlCompilerExecutable=false` gera MSB4027 porque MetadataLoadContext ja foi descartado.

O problema foi observado apenas na compilacao local em Windows em chines. O executor windows-latest em ingles do GitHub Actions conclui a Pass2 do XAML com `dotnet build`. O projeto agora usa a CLI dotnet sem exigir a instalacao completa do MSBuild do Visual Studio.

## Decisao

- Usar `dotnet build src/WSLCC.App/WSLCC.App.csproj -c <Configuration> -p:Platform=<Platform> -p:RuntimeIdentifier=win-<Platform>`.
- Os caminhos de compilacao sao `build.ps1` e o fluxo de CI em `.github/workflows/build.yml`. Eles usam `dotnet build` e `dotnet publish`.
- Executar a CI em windows-latest em ingles com o .NET 10 SDK para evitar WMC9999.

## Consequencias

A compilacao dispensa o Visual Studio. A CI passa no executor em ingles e o desenvolvimento local exige apenas o .NET 10 SDK. Se a compilacao direta no Windows em chines gerar WMC9999, compilar em ambiente em ingles ou aguardar uma correcao no projeto de origem.
