# ADR-0011. Adicionar digests, all-tags, follow-link e eventos do wslc 2.9.13

Estado: Aceita

## Contexto

Em 2026-09-25, o WSL publicou a versao preliminar 2.9.13. Ela acrescentou os seguintes recursos compativeis com Docker em relacao a 2.9.12:

1. `image list --digests` acrescenta a coluna DIGEST. O JSON so contem o digest real com `--digests`. Sem a opcao, o valor e `<none>`, conforme a verificacao `needDigest` do Docker.
2. `pull --all-tags` (ou `-a`) baixa todas as tags de um repositorio. Referencias que ja contem tag ou digest sao recusadas com `tag can't be used with --all-tags/-a`.
3. `container cp --follow-link` (ou `-L`) segue links simbolicos na copia. Por padrao, ainda copia o proprio link.
4. `events` (alias `system events`) transmite eventos e aceita `--since`, `--until` e `--filter` repetido com as chaves `type`, `container`, `image`, `network` ou `event`. Permite filtrar eventos do ciclo de vida de containers e redes, como create, start, stop, destroy, connect e disconnect.
5. O campo `CreatedSince` no JSON das listas passou a usar texto em ingles independente da localidade.

A CLI tambem introduziu opcoes globais com escopo por comando, preparando o suporte a compose. Opcoes como `--session` devem aparecer depois do comando que as declara.

## Decisao

- Atualizar `Microsoft.WSL.Containers` de 2.9.12 para 2.9.13 e ajustar a versao padrao em `scripts/fetch-wslc-sdk.ps1`.
- Adicionar `WslcCapabilities` (`IWslcCapabilities`) para ler e armazenar em cache `wslc --version`. `ImageDigests`, `PullAllTags`, `CopyFollowLink` e `Events` exigem pelo menos 2.9.13. A versao 2.9.12 rejeita essas opcoes. Consultar a disponibilidade antes de passa-las e chamar `Invalidate()` quando a sessao ou o ambiente mudar.
- Adicionar `CreatedSince` e `Digest` a `ImageItem`. Solicitar `--digests` nas versoes compativeis, priorizar `CreatedSince` para a idade e mostrar o digest quando presente. Oferecer `--all-tags` nas versoes que o aceitam.
- Adicionar `followLink` a `CopyAsync` e incluir `--follow-link` quando houver suporte.
- Adicionar `WslcEventService` (`IWslcEventService`). `QueryAsync` converte resultados historicos em `WslcEventEntry` (instante, tipo, acao, alvo, atributos). `StreamAsync` informa os eventos ao vivo usando `RunStreamingAsync`. A pagina Eventos oferece filtros por inicio, fim, tipo e container e permite alternar entre historico e escuta ao vivo.

## Consequencias

A interface oferece digests, download de todas as tags, copia de links simbolicos e eventos de containers e redes. Na versao 2.9.12 e anteriores, os controles ficam desabilitados e indicam a necessidade de atualizar. A tabela de versoes minimas deve acompanhar mudancas no significado das opcoes. O analisador de eventos segue o formato dos testes oficiais de ponta a ponta, `<instante> <tipo> <acao> <ID> (k=v, ...)`. Se o formato mudar, entradas desconhecidas podem deixar de aparecer.

## Resultados dos testes de 2026-09-27

Os testes na versao 2.9.13 confirmaram o contrato oficial:

- `image list --digests --format json` retornou digests `sha256:` reais. Sem `--digests`, `Digest` retornou `<none>`.
- `pull <referencia-com-tag> --all-tags` foi recusado com `E_INVALIDARG` e a mensagem localizada `tag 不能与 --all-tags/-a 一起使用`.
- `container cp -L` copiou o conteudo de destino (13 bytes). Sem `-L`, copiou o link (0 bytes).
- O historico de `events` e `--filter type=network|container` funcionaram e foi possivel cancelar a transmissao ao vivo.

### Detalhes confirmados nos testes

1. Sem `--until`, `events` transmite indefinidamente. `QueryAsync` precisa informar um limite superior. Um evento ocorrido no mesmo segundo do limite pode faltar, por isso as consultas historicas usam o segundo Unix atual mais um, como nos testes oficiais.
2. Na 2.9.13, o RedirectionGuard rejeita a travessia de pontos de nova analise. Se `%LOCALAPPDATA%\wslc` ou `storagePath` for uma juncao ou link simbolico, as operacoes wslc falham com `0x800701c0` (ponto de montagem nao confiavel). Na tela de configuracoes do aplicativo, definir `storagePath` em `settings.yaml` como um diretorio real. Caminhos padrao sem pontos de nova analise nao sao afetados.

## Acompanhamento

O WSL 3.0.1 passou a disponibilidade geral em 2026-09-29. Os recursos que exigem 2.9.13 ou posterior continuam disponiveis na linha estavel. A [ADR-0012](0012-sync-official-wslc-3.0.1-ga.md) registra o novo canal de atualizacao.
