# ADR-0005. Gerenciar uma sessao por processo com inicio e termino explicitos

Estado: Aceita

## Contexto

Os testes mostraram que apenas uma sessao com determinado nome pode estar ativa. Uma segunda chamada a `new Session(name).Start()` lanca COMException 0x800700B7 (o arquivo ja existe). Depois de Terminate, o mesmo nome pode ser iniciado outra vez e reutilizado entre reinicios do aplicativo. A API nativa nao permite enumerar ou reutilizar sessoes. O gerenciamento incorreto pode causar conflitos ou vazamentos de recursos.

## Decisao

- `WslcApiHost` cria uma unica Session por processo. SemaphoreSlim serializa o acesso.
- `EnsureSessionAsync` inicia a sessao de forma idempotente. `TerminateSessionAsync` encerra a sessao explicitamente e libera a VM.
- Ao sair do aplicativo, a sessao continua ativa para preservar o estado dos containers e da VM. O usuario pode gerenciar sessoes pela interface ou com `wslc system session terminate`. O painel lista as sessoes ativas para limpeza manual.
- AppSettings persiste no banco o nome da sessao, o caminho de armazenamento e as configuracoes de CPU e memoria.

## Consequencias

Os nomes de sessao nao entram em conflito, os containers sobrevivem aos reinicios do aplicativo e o usuario consegue ver e controlar os recursos usados pela VM. Sessoes de longa duracao consomem memoria e exigem idleTimeout ou gerenciamento manual.
