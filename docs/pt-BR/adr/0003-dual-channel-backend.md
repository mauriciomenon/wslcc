# ADR-0003. Usar a CLI wslc como backend principal e manter a API nativa

Estado: Aceita

## Contexto

O wslc oferece uma CLI com sintaxe semelhante a do Docker (`wslc.exe` com `--format json`) e uma API nativa (projecao C# do pacote NuGet Microsoft.WSL.Containers). Nos testes locais de setembro de 2026, a Session da API iniciava, mas operacoes como GetImages sempre lancavam COMException E_UNEXPECTED (0x8000FFFF). Os comandos info e image ls da CLI funcionavam. Comandos de containers e volumes falhavam ocasionalmente com E_UNEXPECTED. A versao preliminar era instavel naquele ambiente por interferencia da VPN ou da rede (microsoft/WSL#41082).

## Decisao

- Usar subprocessos `wslc.exe` e analisar JSON para as leituras e gravacoes da interface por padrao.
- Manter `WslcApiHost` como alternativa. Ele encapsula a deteccao do ambiente, sessoes, imagens e eventos de progresso para uso futuro em ambientes adequados.
- O servico de ambiente informa `ApiAvailable` e o painel mostra o canal em uso.

A CLI oficial fornece uma interface programavel estavel, com sintaxe semelhante a do Docker e saida estruturada. Ela tambem oferece operacoes como list, logs e stats que faltavam na API. Usar a API preliminar por padrao prejudicaria a disponibilidade. Manter as interfaces separadas permite uma troca futura sem espalhar detalhes da versao preliminar pelo aplicativo.

## Consequencias

O aplicativo funciona onde a CLI esta disponivel e pode migrar quando a API se estabilizar. A leitura da saida depende dos nomes dos campos, por isso o analisador aceita variantes. Iniciar o subprocesso tem um custo pequeno, desprezivel na interacao com o aplicativo.

## Revisao de 2026-09-29

O WSL 3.0.1 passou a disponibilidade geral em 2026-09-29, incluindo containers e a API. O anuncio oficial removeu as indicacoes de versao preliminar. Depois da atualizacao local de WSL e wslc para 3.0.1, o erro de rede E_UNEXPECTED deixou de ocorrer nos testes. A CLI continua sendo o canal principal para leituras e gravacoes.

A [ADR-0012](0012-sync-official-wslc-3.0.1-ga.md) atualizou o SDK para 3.0.1. Nenhum fluxo do aplicativo chama `WslcApiHost` atualmente. A equipe pode avaliar a API estavel quando houver necessidade de seguranca de tipos ou eventos de progresso.
