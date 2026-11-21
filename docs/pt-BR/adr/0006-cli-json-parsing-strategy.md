# ADR-0006. Aceitar nomes alternativos de campos ao analisar o JSON da CLI

Estado: Aceita

## Contexto

A CLI usa saida no estilo Docker. Com `--format json`, cada linha contem um objeto JSON. Um resultado vazio nao produz saida ou gera uma tabela vazia. Versoes preliminares ja mudaram os campos. Por exemplo, a 2.9.9 ampliou a saida de `volume list --format json` de dois campos para dez campos compativeis com Docker. Alguns comandos (container/volume list) retornam E_UNEXPECTED no ambiente atual, impedindo a coleta de amostras locais dos campos reais.

## Decisao

- `WslcRunner.RunJsonLinesAsync` le linhas que representam objetos e comecam com `{`, usa JsonDocument para analisa-las, pula linhas invalidas e retorna um fluxo de valores JsonElement.
- Tentar os nomes candidatos em ordem, incluindo `Names` para nome de container, `ID`/`Id` para identificador e alternativas para a imagem. Usar padroes como `"-"`, `""` ou `<none>` para campos ausentes.
- Combinar stdout e stderr. Reconhecer `错误代码： X` e `Error code: X`, incluindo a mensagem localizada de falha catastrofica `灾难性故障`. Mapear o codigo para `WslcCliException.WslcErrorCode`.
- Para pull, run e logs, enviar cada linha de progresso a um callback e mostrar o texto original. Mudancas na estrutura do progresso nao afetam a exibicao.

## Consequencias

Mudancas nos campos nao invalidam a lista inteira. Os codigos de erro continuam legiveis e a exibicao do progresso e robusta. Alguns campos, como tamanhos, sao textos com precisao limitada. Amostras reais podem orientar melhorias futuras.
