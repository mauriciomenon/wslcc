# Criterios de aceitacao

## 1. Compilacao

- AC-01. Compile com o MSBuild do Visual Studio (amd64) usando `MSBuild.exe WSLCC.App.csproj -p:Platform=x64`. A compilacao termina com zero erros e zero avisos.
- AC-02. Compile com WindowsAppSDKSelfContained e WindowsPackageType=None. O executavel funciona diretamente em uma maquina sem Windows App Runtime instalado.

## 2. Execucao

- AC-03. Ao iniciar, a janela principal mostra o fundo Mica, a NavigationView e o Painel como pagina inicial. O processo continua em execucao sem falha.
- AC-04. O painel mostra as versoes do wslc, kernel e Windows, os componentes ausentes, as sessoes ativas e a disponibilidade da API.
- AC-05. A pagina Imagens lista as imagens locais quando `wslc image ls` esta disponivel. O campo e o botao de download funcionam e uma falha de download gera uma mensagem clara.
- AC-06. A pagina Conteineres lista os conteineres quando o wslc esta disponivel. O dialogo de criacao abre com campos para nome, imagem, portas, variaveis de ambiente, remocao automatica e execucao em segundo plano. O estado dos botoes iniciar, parar e excluir acompanha o item selecionado.
- AC-07. Em Logs, selecione um conteiner para consultar a saida de `wslc logs`. Um conteiner inexistente ou indisponivel gera uma mensagem clara.
- AC-08. A pagina Atividade mostra registros de auditoria e historico de downloads lidos do SQLite.

## 3. Persistencia

- AC-09. Altere o nome da sessao, o caminho de armazenamento e as cotas em Configuracoes e salve. Verifique se os valores permanecem apos reiniciar o aplicativo.
- AC-10. Cada operacao de baixar, criar, iniciar, parar ou excluir gera um registro de auditoria com categoria, acao, resultado e duracao.
- AC-11. Quando um download termina, a referencia da imagem, o estado e o horario aparecem no historico de downloads.
- AC-12. O arquivo `%LOCALAPPDATA%\WSLCC\wslcc.db` existe e pode ser lido.

## 4. Robustez

- AC-13. Em um ambiente onde os comandos wslc retornam E_UNEXPECTED, o aplicativo continua em execucao e mostra erros compreensiveis para as operacoes afetadas.
- AC-14. Quando o bloqueio de TLS impede o acesso direto ao Docker Hub, uma falha de download afeta apenas essa operacao. As outras paginas continuam disponiveis.
- AC-15. O aplicativo grava as excecoes globais em `%LOCALAPPDATA%\WSLCC\app-crash.log`.

## 5. Design

- AC-16. Toda a interface usa controles WinUI padrao, sem componentes de desenho personalizado como Path ou Canvas.
- AC-17. Todos os ViewModels usam [ObservableProperty] em propriedades parciais e geram zero avisos MVVMTK0045.
- AC-18. WSLCC.Data concentra o acesso ao banco e WSLCC.Core concentra os servicos. App nao acessa o banco diretamente.
