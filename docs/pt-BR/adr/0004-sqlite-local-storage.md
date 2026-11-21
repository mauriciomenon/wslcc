# ADR-0004. Armazenar os dados locais em SQLite com Microsoft.Data.Sqlite

Estado: Aceita

## Contexto

O aplicativo precisa preservar configuracoes do usuario (nome da sessao, caminho de armazenamento e cotas), registros de auditoria, historico de downloads de imagens e instantaneos do estado dos containers. As alternativas eram SQLite, arquivos JSON e EF Core com SQLite.

## Decisao

Salvar o banco em `%LOCALAPPDATA%\WSLCC\wslcc.db`. Usar o provedor ADO.NET da Microsoft, Microsoft.Data.Sqlite, por meio de repositorios. Inicializar o esquema de forma idempotente, ativar o modo WAL e serializar as gravacoes no nivel do banco de dados.

- Para um aplicativo local com poucos dados e baixa concorrencia, o SQLite dispensa a instalacao de um servidor.
- O SQLite permite consultas, agregacoes como contagens, transacoes e futuras extensoes que arquivos JSON simples nao oferecem diretamente.
- O uso direto do provedor evita o custo de geradores de codigo e migracoes do EF Core em WinUI/AOT e reduz as dependencias.
- WSLCC.Data e um projeto separado. As dependencias seguem de App para Core e de Core para Data.

## Consequencias

Os dados persistem, aceitam consultas, permitem backup em um arquivo e podem ser lidos por outras ferramentas. A equipe mantem o esquema e o codigo dos repositorios, algo viavel nesta escala.
