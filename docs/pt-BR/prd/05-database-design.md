# Projeto do banco de dados (WSLCC.Data / SQLite)

## 1. Escolha da tecnologia

- O aplicativo de desktop funciona em uma unica maquina, armazena pequenos volumes de configuracao, auditoria e historico e tem baixa concorrencia.
- O SQLite dispensa implantacao separada e usa um unico arquivo que pode ser lido em varias plataformas, o que simplifica a manutencao em producao.
- Microsoft.Data.Sqlite e o provedor ADO.NET oficial. Seu uso evita o custo de AOT e migracoes do EF Core em WinUI.

## 2. Local do banco e conexao

- Arquivo: `%LOCALAPPDATA%\WSLCC\wslcc.db`.
- Cada operacao abre uma `SqliteConnection` de curta duracao, executa a consulta ou comando e fecha a conexao. Serialize as escritas com SemaphoreSlim. Ative o modo WAL para leituras concorrentes.
- Na primeira abertura, inicialize o esquema com instrucoes idempotentes CREATE TABLE IF NOT EXISTS.

## 3. Esquema do banco

### 3.1 app_settings (configuracoes do aplicativo)

| Coluna | Tipo | Descricao |
|---|---|---|
| key | TEXT PK | Chave da configuracao |
| value | TEXT NOT NULL | Valor da configuracao em JSON ou texto simples |
| updated_at | TEXT NOT NULL | Horario de atualizacao (ISO8601) |

As chaves convencionadas sao `session.name`, `session.storagePath`, `session.cpuCount`, `session.memoryMb` e `ui.defaultImage`.

### 3.2 audit_log (auditoria de operacoes)

| Coluna | Tipo | Descricao |
|---|---|---|
| id | INTEGER PK AUTOINCREMENT | Chave primaria |
| ts | TEXT NOT NULL | Horario do evento em ISO8601 |
| category | TEXT NOT NULL | Categoria (image/container/session/settings/system) |
| action | TEXT NOT NULL | Acao (pull/run/start/stop/restart/remove/delete/...) |
| detail | TEXT | Detalhes, como nome do objeto ou referencia da imagem |
| result | TEXT NOT NULL | Resultado (success/failure) |
| message | TEXT | Mensagem de erro em caso de falha |
| duration_ms | INTEGER | Duracao em milissegundos |

### 3.3 pull_history (historico de downloads de imagens)

| Coluna | Tipo | Descricao |
|---|---|---|
| id | INTEGER PK AUTOINCREMENT | Chave primaria |
| image_ref | TEXT NOT NULL | Referencia da imagem |
| status | TEXT NOT NULL | Resultado (success/failure) |
| error | TEXT | Motivo da falha |
| started_at | TEXT NOT NULL | Horario de inicio |
| finished_at | TEXT | Horario de conclusao |

### 3.4 container_snapshot (instantaneos de estado dos conteineres)

| Coluna | Tipo | Descricao |
|---|---|---|
| id | INTEGER PK AUTOINCREMENT | Chave primaria |
| name | TEXT NOT NULL | Nome do conteiner |
| image | TEXT | Imagem |
| state | TEXT | Estado (running/exited/...) |
| status | TEXT | Descricao do estado |
| ports | TEXT | Mapeamentos de portas em texto |
| observed_at | TEXT NOT NULL | Horario da observacao |

Indice: `(name, observed_at)`.

### 3.5 session_state (estado das sessoes)

| Coluna | Tipo | Descricao |
|---|---|---|
| name | TEXT PK | Nome da sessao |
| storage_path | TEXT NOT NULL | Caminho de armazenamento |
| cpu_count | INTEGER | Cota de CPU |
| memory_mb | INTEGER | Cota de memoria |
| created_at | TEXT | Horario de criacao inicial |
| last_started_at | TEXT | Horario do inicio mais recente |
| last_terminated_at | TEXT | Horario do encerramento mais recente |

## 4. Interfaces dos repositorios

| Repositorio | Operacoes |
|---|---|
| SettingsRepository | Get(key)/Set(key,value)/GetAll() |
| AuditLogRepository | Add(entry)/Query(limit, category?)/Clear()/Count() |
| PullHistoryRepository | Add(entry)/Query(limit)/Count() |
| ContainerSnapshotRepository | Add(entry)/QueryLatest(limit)/ClearOlderThan(days) |
| SessionStateRepository | Upsert(state)/Get(name)/GetAll() |

## 5. Fluxo de dados

- A leitura comeca no evento Loaded da pagina. O ViewModel chama um servico (CLI/API) e vincula os DTOs retornados a lista. A auditoria, o historico e as configuracoes sao lidos do banco.
- Na escrita, o usuario inicia uma operacao, um servico a executa, WslcAuditService.Record registra o sucesso ou a falha e a interface e atualizada.
- Apos uma atualizacao bem-sucedida da lista de conteineres, WslcSnapshotService.RecordContainer grava os instantaneos em lote. Mantenha os ultimos N dias.
