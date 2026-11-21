<div align="center">
  <img src="src/WSLCC.App/Assets/AppIcon.png" alt="WSLCC" width="120" />
  <p><strong>WSLCC | Fork dedicado exclusivamente a traducao pt-br e en, sem alteracoes no codigo</strong></p>
  <h1><a href="https://github.com/iwvw/wslcc">Projeto original: iwvw/wslcc</a></h1>
  <p>O WSLCC foi desenvolvido por iwvw e pelos colaboradores do projeto original. Este fork traduz a interface e a documentacao para portugues do Brasil e ingles.</p>
</div>

[Portugues (Brasil)](README.md) | [English](README.en.md) | [简体中文](README.zh-CN.md)

## Idioma da interface

Neste fork, abra **Configuracoes > Aparencia > Idioma** para seguir o idioma do sistema ou escolher ingles, portugues do Brasil ou chines simplificado. Encerre o WSLCC pelo icone da bandeja e abra o aplicativo novamente para aplicar a mudanca.

Consulte a documentacao do projeto em [chines](docs), [ingles](docs/en) ou [portugues do Brasil](docs/pt-BR).

## Documentacao do projeto original

As secoes abaixo traduzem o README de iwvw/wslcc. [Ler o README original em chines](https://github.com/iwvw/wslcc#readme)

O WSLCC e um gerenciador de containers WSL (wslc) para Windows. Gerencie imagens, containers, projetos Compose, volumes e logs pela interface grafica que usa a WSL Container API da Microsoft e a CLI wslc.

## Recursos

- **Painel.** Consulte o estado do ambiente, as cotas de recursos e as sessoes ativas. Abra o terminal de uma sessao com um clique.
- **Containers.** Liste os containers com os icones obtidos automaticamente dos sites, consulte detalhes e uso de recursos, inicie, pare, reinicie ou remova containers. Abra as portas expostas no navegador, acesse um terminal e copie arquivos com a opcao de seguir links simbolicos. Consulte os logs por quantidade de linhas finais, com marcas de tempo ou por intervalo. Crie containers com volumes, montagens, variaveis de ambiente, verificacoes de integridade, aliases de rede e limites de recursos.
- **Compose.** Implante, edite, reconstrua, pare e remova projetos. Acompanhe cada etapa da implantacao e seus logs. Reutilize as imagens locais ou baixe as versoes mais recentes.
- **Imagens.** Consulte detalhes, digests e camadas intermediarias. Veja o historico de downloads, baixe todas as tags, remova imagens especificas ou limpe as imagens sem uso.
- **Volumes.** Gerencie volumes montados e remova os que estao sem uso.
- **Redes.** Liste, inspecione, crie e remova redes. Conecte ou desconecte containers e remova redes sem uso.
- **Eventos.** Consulte o historico de eventos do ciclo de vida de containers e redes ou acompanhe os eventos em tempo real.
- **Limpeza.** Remova containers, imagens, volumes e redes sem uso com um clique.
- **Logs e auditoria.** Consulte os logs dos containers e o registro das operacoes.
- **Minipainel.** Clique com o botao esquerdo no icone da bandeja para abrir o painel no canto inferior direito. Consulte o estado dos containers, inicie, pare ou reinicie um container, abra portas expostas e inicie ou pare a sessao wslc. Deslize para baixo para recolher o painel.
- **Relatos de erros.** Copie os dados de diagnostico e abra uma issue no GitHub com um clique.
- **Configuracoes.** Configure a sessao no arquivo oficial `settings.yaml` do wslc, defina um espelho do registro de imagens, escolha o tema e o material de fundo ou inicie o aplicativo minimizado na bandeja. As opcoes aparecem em cartoes agrupados no estilo do Windows 11.
- **Uso de memoria.** Ao ocultar o WSLCC na bandeja, o aplicativo libera as paginas da janela principal. A memoria residente do processo pode cair para cerca de 40 MB.

## Requisitos

- Windows 10 ou 11 (x64).
- CLI WSL Containers da Microsoft, ja disponivel para uso geral. Atualize o WSL com `wsl --update`. [Documentacao do WSL Containers](https://github.com/MicrosoftDocs/wsl/blob/main/WSL/wsl-container.md)
- SDK do .NET 10 para compilar o projeto.
- A edicao dependente de framework tambem exige o .NET 10 Desktop Runtime e o Windows App Runtime 2.4. A edicao autocontida inclui os runtimes.

## Compilacao

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

Na primeira compilacao, o script baixa o pacote do SDK `Microsoft.WSL.Containers` para `nuget-packages/`. A Microsoft distribui esse pacote exclusivamente pelas releases de microsoft/WSL no GitHub. Tambem e possivel executar `scripts/fetch-wslc-sdk.ps1` manualmente.

## Empacotamento e releases

- **Autocontida.** Esta edicao e maior, inclui o runtime do .NET e pode ser executada logo apos a extracao.
- **Dependente de framework.** Esta edicao e menor e usa o .NET 10 Desktop Runtime e o Windows App Runtime 2.4 instalados no sistema.

```powershell
# Autocontida (padrao)
powershell -ExecutionPolicy Bypass -File publish.ps1

# Dependente de framework
powershell -ExecutionPolicy Bypass -File publish.ps1 -Framework
```

Cada comando gera um ZIP que contem o aplicativo em uma unica pasta. Extraia o arquivo e execute o WSLCC. Acrescente `-InnoSetup` para gerar tambem o instalador da edicao escolhida.

O envio de uma tag com o padrao `v*` aciona o fluxo do GitHub Actions que compila o projeto e cria uma release com os seguintes arquivos.

| Edicao | Arquivo ZIP | Instalador |
| --- | --- | --- |
| Autocontida | `WSLCC-win-x64.zip` | `WSLCC-Setup.exe` |
| Dependente de framework | `WSLCC-win-x64-framework.zip` | `WSLCC-Setup-framework.exe` |

O atualizador integrado detecta novas versoes, baixa o pacote correspondente a edicao instalada e substitui a versao atual.

## Colaboradores

- Design do icone do aplicativo por [lemonno2333](https://github.com/lemonno2333).
- Desenvolvimento do projeto por [DSUK](https://github.com/iwvw).

## Comunidade

- Discussoes sobre tecnologia no [Linux.do](https://linux.do).

## Licenca

O WSLCC usa a [licenca MIT](LICENSE).
