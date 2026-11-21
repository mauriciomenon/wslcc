# ADR-0012. Adotar o runtime de containers do WSL 3.0.1 em disponibilidade geral

Estado: Aceita

## Contexto

O WSL 3.0.1 foi lancado em 2026-09-29. Os containers do WSL (wslc) e a API WSL Containers passaram a disponibilidade geral. O anuncio removeu as indicacoes de versao preliminar (microsoft/WSL#41704) e documentou `wsl --update` como caminho de instalacao. As versoes 2.9.x anteriores no repositorio estavam marcadas como preliminares.

Mudancas desde a versao 2.9.13:

1. `wsl --update` instala ou atualiza a versao estavel. `container.exe` e um alias integrado de `wslc.exe`, com os mesmos comandos.
2. Os novos comandos incluem `wslc system info` (alias de `info` que exibe as versoes do cliente e servidor e as sessoes ativas), `wslc container restart` (com `-s/--signal` e `-t/--timeout`) e `wslc settings reset`.
3. Os parametros de create/run incluem `--mount`, `--env-file`, `--domainname`, `--network-alias`, `--ip`, `-P/--publish-all`, `--no-healthcheck`, `--dns-option`, `--dns-search`, `--cidfile`, `-i/--interactive` e `-t/--tty`. A maioria ja estava em `ArgumentDefinitions.h` da versao 2.9.13, mas ainda nao aparecia no gerenciador.
4. A criacao de rede aceita opcoes do driver com `-o/--opt` e rotulos com `-l/--label`. O gerenciador ja oferecia suporte na v1.2.0.
5. O Intune acrescenta controles para permitir containers WSL e definir os registros de imagens autorizados. O plugin do Defender for Endpoint passa a abranger atividades dos containers.
6. O acesso do Linux a arquivos do Windows fica ate duas vezes mais rapido. A versao acrescenta o modo de rede `consomme`.

## Decisao

- Atualizar `Microsoft.WSL.Containers` de 2.9.13 para 3.0.1 e ajustar o valor padrao no script que obtem o SDK.
- Alterar `WslcInstallService.RunInstallOrUpgradeAsync` de `wsl --update --pre-release` para `wsl --update`. Informar que o comando atualiza os componentes estaveis de containers.
- Atualizar o README, as orientacoes nas configuracoes, a mensagem de indisponibilidade da pagina Eventos e a mensagem de executavel ausente do `WslcRunner` para indicar a versao estavel e `wsl --update`. Preservar as ADR-0009 e ADR-0011 como registros historicos e atualizar a revisao da ADR-0003.
- Acrescentar ao formulario de criacao e a `ContainerCreateOptions` as opcoes `--mount`, `--env-file`, `--domainname`, `--network-alias`, `-P/--publish-all` e `--no-healthcheck`. Reutilizar Expander com controles TextBox/CheckBox para opcoes avancadas.
- `WslcInstallService.GetStatusAsync` ja seleciona a maior versao entre as tags, portanto detecta a versao estavel (3.0.1 > 2.9.x).
- `WslcCapabilities` mantem 2.9.13 como minimo para os recursos existentes. A versao 3.0.1 atende ao requisito.

## Consequencias

A distribuicao e as atualizacoes usam o canal estavel oficial. O formulario de criacao cobre os principais parametros e o aplicativo orienta os usuarios a usar o comando de atualizacao estavel.

Intune, MDE, listas de registros autorizados e `consomme` pertencem a plataforma ou a administracao. O gerenciador pode repassar `network create -o` quando necessario. Ele continua usando `wslc.exe` para manter a compatibilidade com versoes anteriores.

O projeto de origem planeja oferecer `wslc compose` nativo. Hoje o gerenciador interpreta YAML e executa `wslc run` para cada container. Uma nova ADR avaliara o comando nativo quando estiver disponivel.
