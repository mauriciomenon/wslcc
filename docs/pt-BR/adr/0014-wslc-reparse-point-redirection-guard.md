# ADR-0014. Diagnosticar 0x800701c0 quando a pasta de dados do wslc e um ponto de nova analise

Estado: Aceita

## Contexto

Alguns usuarios movem `%LOCALAPPDATA%` ou uma subpasta para outra unidade, deixando `%LOCALAPPDATA%\wslc` como juncao. O wslc 2.9.13 e posteriores usam RedirectionGuard. No modo de imposicao, ele impede atravessar pontos de montagem nao confiaveis. Com isso, o wslc nao consegue ler `settings.yaml` nem o armazenamento da sessao. A mensagem de erro traduzida e:

```text
O caminho nao pode ser percorrido porque contem um ponto de montagem nao confiavel.
Codigo de erro: 0x800701c0
```

Um teste minimo confirmou que ler um arquivo por uma juncao com a politica imposta sempre lanca `IOException` (0x800701c0). O erro nao ocorreu no modo de auditoria nem com a politica desativada. `wslc.exe` importa `SetProcessMitigationPolicy` e usa esse mecanismo. Processos filhos herdam o RedirectionGuard do processo pai. Os testes confirmaram que os atributos de mitigacao de `CreateProcess` nao podem flexibilizar uma politica imposta pelo pai.

## Decisao

Executar o diagnostico ao iniciar e fornecer orientacoes especificas:

- `WslcPathDiagnostics.DetectUntrustedReparsePoints` verifica se `%LOCALAPPDATA%\wslc` e o caminho de armazenamento padrao sao juncoes ou links simbolicos.
- Na pagina inicial, exibir orientacao se o erro contiver `800701c0` ou mencionar um ponto de montagem nao confiavel, ou se a verificacao inicial detectar um ponto de nova analise.
- Mostrar na pagina Containers uma InfoBar de aviso de politica de caminhos com a lista de caminhos afetados.
- Identificar `0x800701c0` separadamente. Explicar como selecionar um diretorio real para armazenamento ou restaurar `%LOCALAPPDATA%\wslc` como diretorio comum.
- Evitar o excesso de registros durante as consultas de estatisticas gravando o mesmo erro apenas uma vez.

## Consequencias

O usuario ve o problema e a solucao diretamente, sem precisar pesquisar a lista de problemas do WSL. O aplicativo preserva a estrutura dos diretorios. Cabe ao usuario ajustar o caminho. O aplicativo nao consegue contornar automaticamente o RedirectionGuard imposto pelo processo pai usando `CreateProcess`.
