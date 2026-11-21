# ADR-0010. Adicionar um minipainel de containers junto a bandeja do sistema

Estado: Aceita

## Contexto

Iniciar e parar containers sao operacoes frequentes. Voltar a janela principal so para consultar um estado ou controlar um container exige varios passos. Um painel leve e residente perto da bandeja permite consultar o estado e iniciar, parar ou reiniciar containers.

## Decisao

Adicionar a janela independente `MiniWindow` (380 por 540 DIP) no canto inferior direito da area de trabalho, com margem de 12 DIP. O icone da bandeja controla sua visibilidade.

- A janela separada tem uma maquina de estados simples. Seu uso leve e frequente difere das operacoes completas da janela principal. A animacao afeta apenas o painel, que pode continuar inicializado entre usos.
- Somente as acoes da bandeja alternam a visibilidade. Ocultar ao perder o foco causa uma disputa, pois clicar no icone da barra de tarefas desativa a janela antes da alternancia. Desativar a ocultacao automatica elimina a disputa e torna o comportamento previsivel.
- Posicionar a janela no grupo sempre visivel, imediatamente abaixo da barra de tarefas, com `SetWindowPos(hwnd, Shell_TrayWnd, ...)`. Assim ela desliza de tras da barra e outras janelas nao a cobrem. Chamar `PlaceBelowTaskbar` depois de `BringToForeground`.
- `Window` nao tem animacao nativa para exibir ou ocultar. Usar `SetWindowPos` por quadro em uma unica thread residente. Um identificador de geracao permite que um novo pedido invalide a animacao em curso. Cada quadro usa `SWP_NOZORDER` para manter a ordem de sobreposicao sem cintilacao. A abertura dispensa o aparecimento gradual, que esconderia o deslocamento e faria o painel parecer surgir de repente.
- Um `SystemBackdrop` proprio mantem `IsInputActive` verdadeiro para o `DesktopAcrylicController`. O `DesktopAcrylicBackdrop` nativo fica opaco quando a janela sai do primeiro plano. Usar fundo solido se Mica estiver desativado.
- Reutilizar `IWslcContainerService` para listar containers e obter `stats`. Consultar o uso de recursos a cada tres segundos. Cada linha permite iniciar, parar e reiniciar e oferece Abrir quando ha portas mapeadas. O painel residente independe da navegacao principal.
- Por padrao, o clique esquerdo no icone alterna o painel. O usuario pode desativar isso nas configuracoes para voltar a abrir a janela principal. O menu de contexto da bandeja tambem pode exibir ou ocultar o painel. Persistir em `app.trayMiniPanel`, habilitada por padrao.

## Consequencias

Um clique na bandeja da acesso ao estado e aos controles. Animacao, ordem de sobreposicao e materiais podem evoluir de forma independente. A janela residente adicional consome recursos. O posicionamento depende de `Shell_TrayWnd`, detalhe de implementacao estavel ha muito tempo. Se a barra de tarefas nao for encontrada, o painel fica sempre visivel. Barras secundarias usam `Shell_SecondaryTrayWnd`, portanto o posicionamento atual considera apenas a area de trabalho da tela principal.

## Registros relacionados

- [ADR-0001. WinUI 3 e Windows App SDK](0001-use-winui3-windows-app-sdk.md)
- [ADR-0002. CommunityToolkit.Mvvm](0002-mvvm-communitytoolkit-partial-property.md)
