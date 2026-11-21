# ADR-0008. Distribuir com WindowsAppSDKSelfContained e WindowsPackageType=None

Estado: Aceita

## Contexto

O modelo cria um aplicativo empacotado em MSIX por padrao. Em uma maquina sem o Windows App Runtime, abrir diretamente o executavel fez a inicializacao do DeploymentManager falhar. `DeploymentInitializeOptions..ctor()` lancou COMException 0x80040154 (classe nao registrada) e o aplicativo encerrou.

## Decisao

- Definir `<WindowsAppSDKSelfContained>true</WindowsAppSDKSelfContained>` no projeto para distribuir o runtime do Windows App SDK junto com o aplicativo.
- Definir `<WindowsPackageType>None</WindowsPackageType>` para declarar um aplicativo sem pacote e dispensar a implantacao MSIX e a instalacao automatica do runtime.
- Abrir o executavel compilado diretamente em `bin\x64\Debug\net10.0-windows10.0.26100.0\win-x64\WSLCC.App.exe`.
- Manter os PublishProfiles win-x64, arm64 e x86 para uma futura distribuicao MSIX.

## Consequencias

O aplicativo pode ser aberto diretamente, permite distribuicao portatil e simplifica a verificacao na CI. Incluir o runtime aumenta o tamanho da pasta de saida. Sem empacotamento, algumas integracoes com o sistema, como notificacoes em modo escuro, dependem do gerenciamento feito pelo Windows App SDK.
