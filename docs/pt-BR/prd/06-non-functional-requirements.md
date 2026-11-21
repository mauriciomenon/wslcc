# Requisitos nao funcionais

## 1. Desempenho

- Com dados locais, a janela principal deve responder ao usuario em ate 3 segundos apos o inicio do processo.
- Atualize a lista de imagens ou conteineres em ate 2 segundos. O comando wslc consome a maior parte desse tempo e a interface deve continuar responsiva.
- Execute operacoes demoradas (baixar e executar) de forma assincrona, com indicacao de progresso e interface responsiva.
- As operacoes do banco devem levar milissegundos e nunca bloquear a thread da interface.

## 2. Confiabilidade

- Trate as excecoes da CLI e da API de forma consistente e mostre mensagens claras. Essas excecoes nunca devem derrubar o aplicativo.
- O SQLite deve preservar as configuracoes e o historico completos apos reiniciar o aplicativo.
- Se o wslc estiver indisponivel por falta do processo, versao antiga ou falha em uma sessao de maquina virtual, os usuarios ainda podem consultar o conteudo do banco.
- Permita o uso sem conexao de rede. Se o registro de imagens estiver inacessivel, apenas a operacao afetada deve falhar.

## 3. Seguranca

- Nao armazene credenciais. Operacoes sensiveis, como o login no registro de imagens, ficam fora do escopo da v1.
- Omita argumentos sensiveis dos registros de auditoria. Para mapeamentos de portas e variaveis de ambiente, podem ser registrados os nomes das chaves sem os valores.
- Armazene o banco no diretorio de configuracao local do usuario, fora do diretorio de instalacao.

## 4. Manutencao

- Mantenha as dependencias em sentido unico de App (interface) para Core (servicos) e depois para Data (armazenamento).
- Forneca interfaces para os servicos (IWslc*Service) para permitir a troca de implementacoes e os testes unitarios.
- Elimine todos os avisos do compilador. Use propriedades parciais para resolver MVVMTK0045.
- Forneca um comando de compilacao repetivel que possa ser executado por script, conforme [ADR-0007](../adr/0007-msbuild-build-strategy.md).

## 5. Observabilidade

- Grave as excecoes globais sem tratamento em `%LOCALAPPDATA%\WSLCC\app-crash.log`.
- Registre todas as escritas na auditoria com categoria, acao, detalhes, resultado e duracao.
- Mantenha um historico rastreavel de downloads de imagens com referencia, estado, horarios de inicio e fim e erro.
- Mostre o canal de dados (disponibilidade da API nativa) no painel para ajudar a diagnosticar diferencas de funcionalidade.

## 6. Compatibilidade

- O alvo de execucao e o Windows 11. A plataforma minima e 10.0.17763.
- Aceite nomes alternativos de campos JSON quando a saida do wslc mudar, para que a ausencia de um campo nao invalide todo o resultado.
- A API preliminar do WSL apresentou instabilidade nos testes locais. Use a CLI como canal principal para evitar E_UNEXPECTED na camada de sessao da API nativa.

## 7. Localizacao

- A especificacao original preve textos da interface em chines simplificado.
- Preserve os codigos de erro originais do wslc, como E_UNEXPECTED, para facilitar a consulta de problemas correspondentes no repositorio oficial.
