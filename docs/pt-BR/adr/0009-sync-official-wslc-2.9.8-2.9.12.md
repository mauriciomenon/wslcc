# ADR-0009. Adicionar os recursos do wslc das versoes 2.9.8 a 2.9.12

Estado: Aceita

## Contexto

As versoes preliminares oficiais do WSL de 2.9.8 a 2.9.12 acrescentaram recursos ao wslc e ampliaram a compatibilidade com Docker. A API WSL Containers ainda estava em fase preliminar, com disponibilidade geral planejada para o outono de 2026 no hemisferio norte. O projeto tinha dois problemas:

1. `WslcInstallService` consultava `/releases/latest`, que retorna a ultima versao estavel (na epoca, 2.7). As versoes 2.9.x com wslc eram preliminares, entao o usuario nunca via as atualizacoes 2.9.10 ou posteriores.
2. A atualizacao em um clique usava `winget install --id Microsoft.Windows.WSL`, que instala a versao estavel. O comando oficial para acessar o canal preliminar 2.9.x era `wsl --update --pre-release`.

O gerenciador tambem nao cobria a opcao global `--session`, os campos de sessao em `settings.yaml`, o gerenciamento de redes, a limpeza de recursos e novos parametros de criacao de containers.

## Decisao

- Consultar a lista de versoes de `microsoft/WSL` com `per_page=40` e selecionar o maior numero de versao entre todas as tags, incluindo as preliminares.
- Executar `wsl --update --pre-release` com privilegios de administrador para atualizar.
- Atualizar `Microsoft.WSL.Containers` de 2.9.9 para 2.9.12. O pacote so e distribuido pelo GitHub Releases e nao esta no nuget.org. A compilacao, a publicacao e a CI usam `scripts/fetch-wslc-sdk.ps1` para preencher a fonte local `nuget-packages/`.
- Adicionar `WslcSettingsFileService` para ler e gravar o arquivo oficial `%LOCALAPPDATA%\wslc\settings.yaml` preservando os comentarios e o formato. Ele trata `cpuCount`, `memorySize`, `maxStorageSize`, `storagePath`, `defaultBindingAddress`, `hostLoopback`, `idleTimeout` e `credentialStore`. Permitir que `WslcRunner` passe a opcao global `--session`.
- Validar o nome da sessao ao iniciar e ao salvar as configuracoes com `system session list`. Nos testes, `wslc --session <nome-inexistente>` fazia todas as operacoes retornarem `WSLC_E_SESSION_NOT_FOUND`. Se o nome nao existir, registrar o problema e usar a sessao padrao do wslc.
- Adicionar list/create/rm/connect/disconnect/prune/inspect para redes, prune para containers/imagens/volumes/redes, `container cp`, 19 novos parametros de criacao, `inspect -s`, `list --size`, `image ls --all` e as opcoes de logs `-n`, `--timestamps`, `--details`, `--since` e `--until`.

## Consequencias

A deteccao e a atualizacao seguem o canal preliminar oficial. As configuracoes de sessao passam a valer por meio do arquivo oficial. O gerenciamento de redes, a limpeza e os principais parametros de criacao ficam disponiveis na interface.

A gravacao em YAML usa os comentarios oficiais para localizar as chaves. Se o texto mudar, acrescenta a chave e o valor ao fim da secao correspondente, mantendo um YAML valido e funcional. A limpeza exibe a saida original da CLI porque os formatos variam entre os tipos de recurso e nao fornecem contagens uniformes.
