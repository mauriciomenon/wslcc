# ADR-0013. Distribuir edicoes autocontida e dependente de framework

Estado: Aceita

## Contexto

A [ADR-0008](0008-self-contained-deployment.md) definiu `WindowsAppSDKSelfContained=true` e `WindowsPackageType=None` para distribuir o aplicativo pronto para uso em uma pasta. O resultado ocupa cerca de 100 MB, principalmente pelos runtimes .NET e Windows App SDK incluidos. Usuarios que ja instalaram os runtimes querem um download menor. Quem ainda nao os tem precisa de uma edicao padrao pronta para uso.

## Decisao

Compilar as duas edicoes em cada versao e diferencia-las pelos nomes dos arquivos.

| Edicao | Requisitos | ZIP | Instalador |
|---|---|---|---|
| Autocontida | Inclui o runtime .NET e funciona de imediato | `WSLCC-win-x64.zip` | `WSLCC-Setup.exe` |
| Dependente de framework | Requer .NET 10 Desktop Runtime e Windows App Runtime 2.4 instalados | `WSLCC-win-x64-framework.zip` | `WSLCC-Setup-framework.exe` |

- A edicao autocontida preserva os nomes originais sem sufixo para manter os links de download e as atualizacoes automaticas. A dependente de framework usa `-framework`.
- A propriedade de projeto `WSLCCFrameworkDependent` tem valor padrao `false`. Quando verdadeira, definir `WindowsAppSDKSelfContained=false` e a constante `WSLCC_FRAMEWORK_DEPENDENT`.
- A atualizacao automatica usa a constante de compilacao para escolher o instalador da edicao instalada.
- O instalador dependente de framework verifica os dois runtimes antes da instalacao. Se faltar algum, exibe um aviso e abre a pagina de download.
- `publish.ps1` acrescenta a opcao `-Framework`. A CI compila as duas edicoes e envia quatro arquivos.

## Consequencias

O usuario pode escolher entre tamanho do download e runtimes incluidos. A edicao autocontida continua como padrao. O tempo de compilacao e envio na CI dobra, e a equipe precisa manter os nomes e os mapeamentos de atualizacao das duas edicoes. A edicao dependente de framework nao inicia sem os runtimes, por isso o instalador verifica os requisitos e orienta o usuario.
