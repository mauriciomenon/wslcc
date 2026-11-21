# Registros de decisoes de arquitetura

Este diretorio registra as principais decisoes de arquitetura do WSLCC, o gerenciador de containers do WSLC. Cada registro apresenta estado, contexto, decisao e consequencias.

| ADR | Assunto | Estado |
|---|---|---|
| 0001 | [Usar WinUI 3 e Windows App SDK na interface](0001-use-winui3-windows-app-sdk.md) | Aceita |
| 0002 | [Implementar MVVM com CommunityToolkit.Mvvm e propriedades parciais](0002-mvvm-communitytoolkit-partial-property.md) | Aceita |
| 0003 | [Usar a CLI wslc como backend principal e manter a API nativa](0003-dual-channel-backend.md) | Aceita |
| 0004 | [Armazenar os dados locais em SQLite com Microsoft.Data.Sqlite](0004-sqlite-local-storage.md) | Aceita |
| 0005 | [Gerenciar uma sessao por processo com inicio e termino explicitos](0005-session-lifecycle.md) | Aceita |
| 0006 | [Aceitar nomes alternativos de campos ao analisar o JSON da CLI](0006-cli-json-parsing-strategy.md) | Aceita |
| 0007 | [Compilar com a CLI dotnet e evitar a falha do XamlCompiler no Windows em chines](0007-msbuild-build-strategy.md) | Aceita |
| 0008 | [Distribuir com WindowsAppSDKSelfContained e WindowsPackageType=None](0008-self-contained-deployment.md) | Aceita |
| 0009 | [Adicionar os recursos do wslc das versoes 2.9.8 a 2.9.12](0009-sync-official-wslc-2.9.8-2.9.12.md) | Aceita |
| 0010 | [Adicionar um minipainel de containers junto a bandeja do sistema](0010-mini-container-panel.md) | Aceita |
| 0011 | [Adicionar digests, all-tags, follow-link e eventos do wslc 2.9.13](0011-sync-official-wslc-2.9.13.md) | Aceita |
| 0012 | [Adotar o runtime de containers do WSL 3.0.1 em disponibilidade geral](0012-sync-official-wslc-3.0.1-ga.md) | Aceita |
| 0013 | [Distribuir edicoes autocontida e dependente de framework](0013-dual-release-editions.md) | Aceita |
| 0014 | [Diagnosticar 0x800701c0 quando a pasta de dados do wslc e um ponto de nova analise](0014-wslc-reparse-point-redirection-guard.md) | Aceita |
