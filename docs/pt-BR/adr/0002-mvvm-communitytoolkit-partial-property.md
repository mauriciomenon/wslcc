# ADR-0002. Implementar MVVM com CommunityToolkit.Mvvm e propriedades parciais

Estado: Aceita

## Contexto

As paginas WinUI 3 precisam vincular estados e comandos as suas interfaces. Implementar INotifyPropertyChanged e ICommand a mao exige muito codigo e facilita erros. O projeto precisa do toolkit MVVM recomendado pela Microsoft e de compatibilidade com compilacao antecipada (AOT) do CsWinRT.

## Decisao

Usar CommunityToolkit.Mvvm 8.4 e declarar cada `[ObservableProperty]` como propriedade parcial (`public partial string X { get; set; }`).

- O aviso MVVMTK0045 explica que propriedades observaveis declaradas em campos geram codigo incompativel com AOT no WinUI 3. As propriedades parciais permitem ao gerador de codigo do CsWinRT produzir o marshaling correto para WinRT.
- Usar RelayCommand e AsyncRelayCommand para os comandos. Comandos assincronos aceitam atualizacoes de CanExecute e await.

## Consequencias

A implementacao compila sem avisos, preserva a seguranca de tipos e exige bem menos codigo repetitivo. Propriedades parciais requerem C# 13 ou superior, disponivel em net10.0.
