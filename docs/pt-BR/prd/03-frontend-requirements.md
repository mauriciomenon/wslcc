# Requisitos funcionais da interface (WinUI 3)

## 1. Diretrizes de design

- Siga o Fluent Design e o estilo do Windows 11. Use fundos Mica, estilos de texto do sistema (Title/Subtitle/BodyStrong/CaptionTextBlockStyle), cartoes arredondados (Border com CardBackgroundFillColorDefaultBrush), InfoBar, ProgressRing/ProgressBar, ContentDialog e NavigationView.
- Use controles padrao e recursos do tema para todos os elementos visuais. E proibido desenhar com Path, Canvas, Polyline ou modelos de controle personalizados com desenho manual.
- Use controles WinUI padrao para todas as interacoes, incluindo NavigationView, Frame, ListView, TextBox, CheckBox, Button, InfoBar, ProgressBar, ContentDialog e TextBlock.
- Siga o MVVM. Vincule o DataContext de cada pagina ao ViewModel. Use RelayCommand/AsyncRelayCommand para comandos e ObservableObject/[ObservableProperty] para alteracoes de dados. Use propriedades parciais no WinUI para manter a compatibilidade com CsWinRT AOT.
- Carregue os dados da pagina no evento Loaded. Use o estado de ObservableProperty, como IsLoading, IsPulling e IsRunning, para mostrar o andamento das operacoes assincronas.

## 2. Navegacao

| Item | Pagina | Icone | Conteudo |
|---|---|---|---|
| Painel | HomePage | E80F (Home) | Ambiente, canal de dados e lista de sessoes |
| Conteineres | ContainersPage | E7F4 | Listar, criar, iniciar, parar e excluir conteineres |
| Imagens | ImagesPage | EB9F | Listar, baixar e excluir imagens |
| Logs | LogsPage | E7C3 (Page/Script) | Logs em tempo real do conteiner selecionado |
| Atividade | ActivityPage | E823 (History) | Registro de auditoria e historico de downloads |
| Sobre | AboutPage | EA3A | Versao e descricao |
| Configuracoes (entrada integrada, IsSettingsSelected) | SettingsPage | E713 | Configuracoes de sessao e dados do banco |

## 3. Requisitos por pagina

### 3.1 HomePage (Painel)

- O cabecalho exibe titulo da pagina, botao Atualizar e indicador de carregamento.
- O cartao de ambiente mostra as versoes do wslc, kernel Linux, Windows e gerenciador de sessoes, a quantidade de sessoes ativas, os componentes ausentes, a disponibilidade da API nativa e o caminho do arquivo de configuracoes.
- O cartao de sessoes lista nomes e IDs das sessoes ativas retornadas por `wslc info`.
- Mostre um erro compreensivel na InfoBar se a leitura do ambiente falhar.

### 3.2 ContainersPage (Conteineres)

- O cabecalho contem titulo, Atualizar e Novo (ContentDialog).
- A barra do conteiner selecionado mostra o nome e botoes para iniciar, parar, reiniciar e excluir. O CanExecute de cada comando acompanha o estado do conteiner.
- A ListView mostra o icone de estado (triangulo de reproducao para ativo, quadrado para parado), nome, imagem, Status e portas.
- O dialogo de criacao usa controles padrao para nome, imagem, mapeamentos de portas `host:container` e variaveis de ambiente `KEY=VALUE`. Separe por virgulas os itens de cada campo. Use CheckBox para remocao automatica e execucao em segundo plano.
- Atualize a lista apos acoes no fim da linha ou no item selecionado. Registre as falhas na auditoria e mostre uma InfoBar.

### 3.3 ImagesPage (Imagens)

- O cabecalho contem titulo, Atualizar e Excluir selecionada.
- A linha de download tem uma TextBox com uma dica como "Nome da imagem, por exemplo nginx:latest" e um botao Baixar.
- A area de progresso tem uma ProgressBar indeterminada e um texto de estado com a saida do download do wslc em tempo real, linha a linha.
- A ListView mostra nome, ID abreviado, tamanho e data de criacao da imagem.
- Apos excluir com sucesso, remova o item e atualize a lista.

### 3.4 LogsPage (Logs)

- O cabecalho contem titulo, uma ComboBox para selecionar o conteiner e Atualizar.
- Exiba a saida de `wslc logs` em uma TextBox somente leitura. Desative TextWrapping, defina VerticalScrollBarVisibility=Auto, use fonte monoespacada e defina a propriedade padrao IsReadOnly como true.
- Mostre uma mensagem clara na InfoBar se o conteiner estiver parado ou os logs estiverem indisponiveis.

### 3.5 ActivityPage (Atividade)

- O cabecalho contem titulo e Atualizar.
- A ListView de auditoria mostra horario, categoria, acao, resultado (sucesso ou falha) e duracao.
- A ListView do historico de downloads mostra referencia da imagem, total, estado e horarios de inicio e fim.
- As duas listas mostram dados do SQLite local obtidos de `WslcAuditService` e `WslcHistoryService`.

### 3.6 SettingsPage (Configuracoes)

- O cartao da sessao mostra nome, caminho de armazenamento e cotas de CPU e memoria. Carregue o formulario a partir de AppSettings, grave as alteracoes no banco e confirme o salvamento.
- O cartao do canal de dados mostra a disponibilidade da API nativa e o canal atual.
- O cartao do banco de dados mostra o caminho do arquivo, a contagem de registros de auditoria e a de downloads.
- O botao para limpar o registro de auditoria abre um ContentDialog de confirmacao.

## 4. Restricoes visuais

- Use recursos do tema do sistema para cores, raios dos cantos e espacamento. Evite valores de cor fixos nas paginas.
- Use FontIcon com os pontos de codigo padrao de Segoe Fluent Icons para todos os icones. Nao use imagens nem vetores personalizados.
