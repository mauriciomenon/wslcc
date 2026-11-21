# Requisitos funcionais do backend (WSLCC.Core / WSLCC.Data)

## 1. Camadas

```
WSLCC.Core            Servicos de dominio e infraestrutura, independentes da interface
  Models/             DTOs imutaveis para ambiente, imagens, conteineres, progresso, configuracoes e auditoria
  Cli/                Executor de processos wslc.exe e mapeamento de erros
  Services/           Servicos de ambiente, imagens, conteineres, sistema, logs, configuracoes, auditoria e historico
  WslcHost.cs         Fachada de servicos com instancia unica

WSLCC.Data            Camada de acesso a dados
  WslcDatabase.cs     Conexao com SQLite e inicializacao do esquema
  Repositories/       Configuracoes, auditoria, historico de downloads, instantaneos de conteineres e estado das sessoes
```

WSLCC.Core depende de WSLCC.Data. WSLCC.App depende de WSLCC.Core e nao tem dependencia direta de Data.

## 2. Canais de dados

- O canal principal e a CLI do `wslc` (`wslc.exe` com `--format json`). Foi a unica opcao estavel no ambiente de teste, no qual a API nativa retornou E_UNEXPECTED. A CLI tambem oferece a cobertura mais ampla de comandos, incluindo list, logs, stats, save e load.
- A alternativa e a API nativa Microsoft.WSL.Containers, exposta por `WslcApiHost`. Em ambientes compativeis, ela permite acompanhar o progresso por eventos e usar chamadas com seguranca de tipos.
- Durante a execucao, o servico de ambiente informa `ApiAvailable`. Os servicos de imagens e conteineres usam a CLI. `WslcApiHost` expoe as operacoes nativas para uma futura troca de canal.

## 3. Servicos e responsabilidades

| Servico | Operacoes principais | Comportamento |
|---|---|---|
| WslcEnvironmentService | GetEnvironmentAsync | Combina `wslc info --format json` e GetMissingComponents/GetVersion da API |
| WslcImageService | List / Pull / Delete | `image ls --format json`, `pull` com saida continua, `rmi` |
| WslcContainerService | List / Start / Stop / Kill / Restart / Remove / Run | `container list --format json`, `start/stop/kill/restart/rm`, `run -d --rm --name -p -e -v` |
| WslcLogService | GetLogsAsync | Texto monoespacado de `logs <name>` |
| WslcInspectService | InspectContainerAsync | Texto JSON estruturado de `inspect <name> --format json` |
| WslcSystemService | GetInfoAsync / ListSessionsAsync / TerminateSessionsAsync | `info --format json`, `system session list`, `system session terminate` |
| WslcSettingsService | Get/Set das configuracoes de sessao | Le e grava em AppSettings o nome da sessao, caminho de armazenamento, CPU e memoria |
| WslcAuditService | Record / Query / Clear | Audita categoria, acao, detalhes, resultado e duracao |
| WslcHistoryService | RecordPull / QueryPulls | Armazena e consulta o historico de downloads de imagens |
| WslcSnapshotService | RecordContainer / QueryLatest | Mantem em cache os estados observados dos conteineres |
| WslcApiHost | Acesso a API nativa | Instancia unica de Session, progresso de PullImageAsync, GetImagesAsync, DeleteImageAsync, TerminateSessionAsync |

## 4. Execucao da CLI e tratamento de erros

- Encaminhe todas as chamadas da CLI por `WslcRunner`.
  - Inicie `wslc.exe <args>` sem janela e redirecione a saida em UTF-8.
  - Em caso de sucesso, retorne stdout. Analise `--format json` como um objeto JSON por linha (estilo Docker) e aceite nomes alternativos para os campos.
  - Em caso de falha, combine stdout e stderr. Extraia o codigo de `错误代码： X` ou `Error code: X` e lance `WslcCliException` com WslcErrorCode.
  - Em comandos demorados, como pull e run, repasse cada linha da saida por `IProgress<string>`.
- Exiba as mensagens de excecao na InfoBar da interface. Inclua o codigo, como `(E_UNEXPECTED)`, para que o usuario possa consulta-lo na documentacao do wslc e nas issues do GitHub.

## 5. Recursos adicionais

Os recursos a seguir usam a CLI do wslc. Se um comando estiver indisponivel, informe o erro com clareza e mantenha o aplicativo em execucao.

1. Leia logs de conteineres ao vivo e historicos com `logs <name>`.
2. Exiba detalhes estruturados do conteiner com `inspect <name> --format json`.
3. Gerencie sessoes residuais de maquina virtual com `system session list` e `terminate`. Nos testes, sessoes residuais causaram E_UNEXPECTED em operacoes posteriores. O gerenciamento de sessoes permite recuperar o ambiente.
4. Reserve os comandos save/load para futura exportacao e importacao de imagens.
5. Use WslcAuditService para registrar escritas no banco e mantenha o historico de downloads de imagens.
6. Leia e grave as configuracoes de sessao pela pagina Configuracoes. Use esses valores como padrao ao criar conteineres ou sessoes.
7. Salve o estado dos conteineres no SQLite a cada atualizacao da lista. O aplicativo pode mostrar o ultimo instantaneo quando estiver offline ou apos um reinicio rapido.

## 6. Concorrencia e threads

- Os servicos nao mantem estado e usam uma instancia unica por processo (WslcHost.Default).
- Aguarde metodos assincronos no contexto da interface. Deixe o uso de ConfigureAwait(false) a criterio do chamador. Operacoes demoradas dos servicos devem manter a interface responsiva.
- WslcRunner inicia um processo separado por chamada e executa em sequencia os comandos dessa chamada. Serialize as escritas no banco com SemaphoreSlim na conexao SQLite compartilhada.
