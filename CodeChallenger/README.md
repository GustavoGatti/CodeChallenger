# CodeChallenger

Projeto foi implementado como etapa de teste de código para processamento de operações financeiras (compra/venda) com cálculo de impostos para o desafio proposto.

## Visão geral e decisões técnicas

- Arquitetura simples e orientada a responsabilidades: leitura de entrada (`ReadJson`), cálculo (`Calculation`, `Buy`, `Sell`, `BaseCalculation`) e escrita de saída (`WriteJson`). Cada componente implementa lógica bem definida para facilitar testes e manutenção.
- Uso de DTOs (`TransactionDto`, `TaxDto`) para transferência de dados entre camadas e para desserialização de JSON.
- Classes estáticas para operações centrais (leitura, cálculo, escrita) visam simplicidade e facilidade de invocação a partir de um `Program` console. Em um sistema maior, essas poderiam ser substituídas por serviços instanciáveis para permitir injeção de dependência e maior testabilidade.
- Parsing de entrada: `ReadJson.ExtractJsonArrays` identifica arrays JSON contidos na entrada de texto bruto, permitindo que a aplicação processe múltiplas listas de transações em sequência.
- Tratamento de erros: desserialização e operações de cálculo capturam exceções e, quando apropriado, retornam estruturas vazias em vez de propagar falhas para a aplicação inteira — escolha orientada a tolerância a falhas na ingestão de dados.

## Justificativa de frameworks

- .NET 10 e C# 14: plataforma escolhida pelo exercício pois tenho mais afinidade.
- `System.Text.Json`: serialização/desserialização JSON padrão do .NET supre as necessidades do projeto e tem respaudo da Microsoft.
- `xUnit` para testes automatizados pois é a biblioteca que mais tenho afinidade e suporta bem testes para grandes aplicações em.NET. Facilita execução via `dotnet test`.
- Não há dependências externas.

## Como compilar e executar

Pré-requisitos:
- .NET 10 SDK instalado

Passos:	
1. Abra um terminal na raiz do repositório (`CodeChallenger`).
2. Restaurar dependências e compilar:

   `dotnet build`

3. Executar a aplicação console (projeto principal `CodeChallenger`):

   `dotnet run --project CodeChallenger/CodeChallenger.csproj`

A aplicação lê linhas JSON da entrada padrão (console). Dependendo do formato de entrada esperado, ela desserializa uma ou mais listas de transações e escreve o resultado (impostos) no console e utilize uma string de saída "]" para

## Como executar os testes

Na raiz do repositório execute:

`dotnet test`

Isso irá construir os projetos de teste e executar todos os testes xUnit presentes em `CodeChallenger.Tests`.

## Estrutura relevante do código

- `CodeChallenger/Read/ReadJson.cs` — lógica de leitura e extração de arrays JSON da entrada padrão.
- `CodeChallenger/Calculation/` — classes `Calculation`, `Buy`, `Sell`, `BaseCalculation` com regras de negócio e manutenção de estado entre operações da mesma lista.
- `CodeChallenger/Write/WriteJson.cs` — serialização e escrita de saída (JSON) no console.
- `CodeChallenger/Dto/` — DTOs (`TransactionDto`, `TaxDto`).
- `CodeChallenger.Tests/` — testes unitários em xUnit.

## Notas importantes e possíveis melhorias futuras

- Estado global: `BaseCalculation` usa campos estáticos para manter estado (_quantidadeAtual, _mediaPonderadaAtual, etc.). Isso simplifica a implementação mas torna o código não thread-safe e mais difícil de estender. Recomenda-se migrar para serviços instanciáveis com injeção de dependência se o projeto crescer.
- Mensagens e testes: os testes foram implementados em xUnit e cobrem cenários principais (compras, vendas, vendas parciais e limites). Ajustes nos valores de entrada/saída podem ser necessários conforme regras de negócio refinadas.
- Robustez do parser: `ExtractJsonArrays` funciona para entradas simples, mas uma solução mais robusta poderia usar um parser JSON incremental/tokenizador (`Utf8JsonReader`) para lidar com casos complexos (strings contendo colchetes, espaços em branco diversos, comentários, etc.).
- Logging e observabilidade: adicionar logs e métricas ajudaria na operação em produção.
