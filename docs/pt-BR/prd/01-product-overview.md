# Visao geral do gerenciador de conteineres WSLC

## 1. Finalidade

O gerenciador de conteineres WSLC (WSLCC) oferece aos desenvolvedores no Windows uma interface grafica para o Microsoft WSL Containers (wslc). Ela permite consultar conteineres e imagens, criar e executar conteineres, parar ou excluir conteineres e baixar imagens sem memorizar a sintaxe dos comandos wslc.

- O WSL inclui o wslc. O uso dispensa licenca do Docker Desktop, daemon separado e dependencia do Docker Engine.
- Listas, formularios, indicadores de progresso e estados dao acesso as operacoes da linha de comando.
- O banco de dados registra as operacoes para auditoria, consulta do historico e diagnostico.
- A separacao entre interface e backend, a persistencia dos dados e o registro de erros facilitam a manutencao em producao.

## 2. Publico-alvo

| Perfil | Ambiente | Principais necessidades |
|---|---|---|
| Desenvolvedor individual | Usa WSL no dia a dia | Baixar imagens, iniciar e parar conteineres e consultar seu estado rapidamente |
| Desenvolvedor de empresa | Trabalha sob politicas do Intune ou GPO | Controlar e auditar operacoes e manter as funcoes basicas em redes restritas |
| Profissional de operacoes ou DevOps | Gerencia conteineres em lote | Atualizar listas, iniciar ou parar com um clique e entender erros |

## 3. Escopo

### 3.1 Incluido do MVP ate a v1.0

- Verificar as versoes do wslc, do kernel e do Windows, os componentes ausentes e as sessoes ativas.
- Listar, baixar com progresso em tempo real e excluir imagens, e atualizar a lista.
- Listar conteineres com nome, imagem, estado e portas. Criar conteineres com nome, imagem, mapeamentos de portas, variaveis de ambiente, remocao automatica e execucao em segundo plano. Iniciar, parar, reiniciar e excluir conteineres.
- Configurar e salvar nome da sessao, caminho de armazenamento e cotas de CPU e memoria.
- Armazenar configuracoes do aplicativo, registros de auditoria, historico de downloads de imagens e instantaneos do estado dos conteineres em um banco SQLite local.
- Exibir os erros da CLI e da API em linguagem clara e de forma consistente em uma InfoBar.

### 3.2 Previsto para versoes v1.x posteriores

- Terminal integrado ligado aos fluxos stdin/stdout do conteiner.
- Orquestracao com Docker Compose.
- Gerenciamento de hosts remotos.
- Gerenciamento de varias sessoes em paralelo.
- Assistente de compilacao de imagens com Dockerfile.

## 4. Restricoes e pre-requisitos

- Compatibilidade com Windows 10 versao 1809 ou posterior (plataforma minima 10.0.17763). O desenvolvimento e a execucao tem como alvo o Windows 11.
- Requer WSL 2.9.3 ou posterior (versao preliminar com wslc.exe). O aplicativo verifica a dependencia e informa o comando de atualizacao quando ela falta.
- O download de imagens requer acesso a um registro de imagens de conteiner. Se a rede impedir o download, o aplicativo deve mostrar um erro compreensivel e continuar em execucao.
- A plataforma usa WinUI 3 (Windows App SDK 2.4.0), .NET 10, CommunityToolkit.Mvvm, Microsoft.WSL.Containers (API nativa), CLI wslc (alternativa) e SQLite (Microsoft.Data.Sqlite).

## 5. Criterios de sucesso

- A compilacao autocontida executa em uma maquina sem Windows App Runtime instalado.
- A verificacao do ambiente e as listas de imagens e conteineres funcionam em um ambiente wslc real.
- As operacoes de escrita (baixar, iniciar, parar, excluir e criar) funcionam quando o wslc esta disponivel. Caso contrario, os erros indicam a causa.
- O SQLite mantem as configuracoes, os registros de auditoria, o historico de downloads e os instantaneos dos conteineres apos reiniciar o aplicativo.
- A compilacao termina sem avisos do compilador, inclusive MVVMTK0045.
