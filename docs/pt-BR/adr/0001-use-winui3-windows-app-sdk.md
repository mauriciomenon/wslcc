# ADR-0001. Usar WinUI 3 e Windows App SDK na interface

Estado: Aceita

## Contexto

O WSLC precisa de um aplicativo de controle nativo para Windows. As alternativas eram WinUI 3, WPF e Tauri/Electron. O aplicativo deve seguir as diretrizes de design da Microsoft, usar controles padrao sem desenho manual e oferecer acesso aos recursos nativos do wslc.

## Decisao

Usar WinUI 3 (Windows App SDK 2.4.0) e .NET 10.

- A Microsoft recomenda WinUI 3 para interfaces nativas do Windows. O framework segue o Fluent Design e inclui Mica, cantos arredondados e recursos de tema.
- Controles padrao como NavigationView, ListView, InfoBar e ContentDialog atendem ao requisito de evitar componentes desenhados a mao.
- WinUI e Microsoft.WSL.Containers (a API WinRT) fazem parte do ecossistema da Microsoft e facilitam o uso de tipos entre os componentes.
- A distribuicao autocontida dispensa a instalacao separada do runtime.

## Consequencias

A interface segue os padroes visuais e de interacao do Windows 11. A equipe ganha produtividade com um framework mantido pela Microsoft. O suporte apenas ao Windows corresponde ao objetivo do produto. No Windows em chines, uma falha conhecida do compilador XAML pode afetar compilacoes locais com `dotnet build`. Consulte a alternativa na [ADR-0007](0007-msbuild-build-strategy.md).
