# Modelo de notas de versao

Escolha o modelo correspondente ao tipo de versao. O projeto original publica as notas em chines simplificado. Use frases curtas e evite linguagem promocional.

---

## Versao com novidades

Use este modelo para versoes com novos recursos, mudancas de comportamento ou incompatibilidades, como v2.0.0 ou v1.8.0.

```markdown
# vX.Y.Z

Resuma a principal mudanca em uma ou duas frases.

## Novidades

- Descreva um recurso para o usuario por item.
- Para uma nova configuracao, indique onde encontra-la.

## Melhorias

- Descreva o comportamento de um recurso antes e depois da mudanca.

## Correcoes

- Descreva o sintoma e as condicoes em que ocorria.

## Incompatibilidades

- Explique quem a mudanca afeta e como migrar.
- Remova a secao se nao houver incompatibilidades.

## Instalacao

- Edicao autocontida (pronta para uso): WSLCC-Setup.exe / WSLCC-win-x64.zip
- Edicao dependente do framework (menor, requer .NET 10 Desktop Runtime e Windows App Runtime 2.4): WSLCC-Setup-framework.exe / WSLCC-win-x64-framework.zip

## Historico completo

- https://github.com/iwvw/wslcc/compare/v<versao-anterior>...vX.Y.Z
```

---

## Versao de correcao

Use este modelo para corrigir erros ou fazer pequenas melhorias que preservam a compatibilidade e nao adicionam recursos, como na v1.7.2.

```markdown
# vX.Y.Z

Descreva a versao em uma frase.

## Alteracoes

- Escreva uma frase por item, em ordem de importancia.

## Instalacao

- Edicao autocontida: WSLCC-Setup.exe / WSLCC-win-x64.zip
- Edicao dependente do framework: WSLCC-Setup-framework.exe / WSLCC-win-x64-framework.zip
```

---

## Convencoes

- Use apenas o numero da versao no titulo, como `v1.7.2`, sem prefixo ou sufixo adicional.
- Use a mesma versao da tag do Git.
- Nomeie os artefatos da edicao autocontida como `WSLCC-Setup.exe` e `WSLCC-win-x64.zip`. Para a edicao dependente do framework, use `WSLCC-Setup-framework.exe` e `WSLCC-win-x64-framework.zip`.
- Nas versoes de correcao menores, mantenha Instalacao e omita as outras secoes vazias.
- Inclua o link de comparacao nas versoes com novidades. Ele e opcional nas versoes de correcao.
