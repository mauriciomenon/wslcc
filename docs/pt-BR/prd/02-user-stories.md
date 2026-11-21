# Historias de usuario e cenarios de uso

## 1. Historias de usuario

### 1.1 Ambiente e primeiros passos

- US-01. Como desenvolvedor no Windows, quero ver as versoes do wslc e do kernel, os componentes ausentes e as sessoes ativas ao abrir o aplicativo para saber se o ambiente esta pronto.
- US-02. Como desenvolvedor, quero ver o comando de atualizacao quando um componente estiver ausente ou desatualizado para resolver o problema sem decifrar um codigo de erro.
- US-03. Como usuario em uma empresa, quero saber se a API nativa esta disponivel e qual canal de dados esta ativo para entender as diferencas de funcionalidade.

### 1.2 Gerenciamento de imagens

- US-10. Como desenvolvedor, quero uma lista das imagens locais com nome, tag, tamanho e data de criacao para escolher qual executar ou excluir.
- US-11. Como desenvolvedor, quero informar o nome de uma imagem, clicar em Baixar, acompanhar o progresso e o resultado em tempo real e ver a lista atualizada automaticamente quando o download terminar com sucesso.
- US-12. Como desenvolvedor, quero excluir a imagem selecionada, confirmar a exclusao antes de executa-la e saber a causa de uma falha.
- US-13. Como desenvolvedor, quero entender a mensagem quando falhar o download ou a exclusao de uma imagem, por exemplo se a rede restringir o acesso ao registro.

### 1.3 Gerenciamento de conteineres

- US-20. Como desenvolvedor, quero uma lista de conteineres com nome, imagem, estado, portas e tempo em execucao para distinguir rapidamente os ativos dos parados.
- US-21. Como desenvolvedor, quero criar um conteiner por formulario, com nome, imagem, mapeamentos de portas, variaveis de ambiente, remocao automatica e execucao em segundo plano, sem escrever um comando.
- US-22. Como desenvolvedor, quero iniciar, parar, reiniciar ou excluir o conteiner selecionado com um clique e saber se a operacao teve sucesso ou falhou.
- US-23. Como desenvolvedor, quero consultar os logs do conteiner para diagnosticar problemas de execucao.

### 1.4 Gerenciamento de sessoes

- US-30. Como desenvolvedor, quero configurar e salvar o nome da sessao, o caminho de armazenamento e as cotas de CPU e memoria.
- US-31. Como desenvolvedor, quero uma lista de sessoes ativas para saber quais sessoes de maquina virtual estao em execucao.

### 1.5 Observabilidade e auditoria

- US-40. Como administrador, quero que cada operacao na interface (baixar, criar, iniciar, parar e excluir) seja registrada com horario, categoria, acao, resultado e duracao.
- US-41. Como administrador, quero consultar o historico de downloads de imagens com repositorio, quantidade total e resultado.
- US-42. Como desenvolvedor, quero manter as configuracoes, o historico e os instantaneos ao reiniciar o aplicativo.

## 2. Cenarios de ponta a ponta

### Cenario A. Primeira verificacao do ambiente

Abra o aplicativo. O painel mostra wslc 2.9.10, kernel 6.18, todos os componentes presentes e zero sessoes ativas. Abra Imagens. Com a lista vazia, informe `alpine:latest` e inicie o download. Acompanhe o progresso e verifique se `alpine:latest` aparece na lista ao terminar com sucesso.

### Cenario B. Executar um servico web

Em Imagens, confirme que nginx existe. Em Conteineres, escolha Novo. Use `web` como nome, `nginx:latest` como imagem e `8080:80` como mapeamento de portas. Execute em segundo plano. Apos a criacao, a lista mostra `web` no estado `running` com `8080:80`. Pare o conteiner, confirme que seu estado passou para `exited` e exclua-o.

### Cenario C. Diagnosticar uma falha

Ocorre uma falha ao parar um conteiner. A InfoBar mostra uma mensagem clara com o codigo de erro do wslc. O painel identifica o canal de dados ativo. A pagina Atividade exibe o registro da falha e a duracao da operacao.

### Cenario D. Manter as configuracoes

Em Configuracoes, defina o nome da sessao como `wslc-prod`, o caminho de armazenamento como `D:\wslc-data` e a memoria como 8 GB. Reinicie o aplicativo. As configuracoes permanecem e a nova sessao as utiliza.

## 3. Restricoes

- US-50. Como profissional de operacoes, quero iniciar o aplicativo em redes sem acesso direto ao Docker Hub. Uma falha de download deve afetar apenas essa operacao.
- US-51. Como desenvolvedor, quero que a compilacao do aplicativo termine sem avisos do compilador.
