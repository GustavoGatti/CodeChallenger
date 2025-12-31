using CodeChallenger.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeChallenger.Read
{
    public static class ReadJson
    {
        // Variáveis que representam a posição atual do portfólio:
        // - quantidade de ações atualmente em carteira
        // - preço médio ponderado atual por ação
        private static int _quantidadeAtual = 0;
        private static decimal _mediaPonderadaAtual = 0m;
        private static decimal _prejuizoAcumulado = 0m;
        private const decimal _limiteIsencao = 20000m;
        private static decimal _imposto = 0m;

        public static void ReadJsonList()
        {
            var sb = new StringBuilder();
            string? lineInput;
            while ((lineInput = Console.ReadLine()) != null)
            {
                if (lineInput.Trim() == "]") break;
                sb.AppendLine(lineInput);
            }

            string raw = sb.ToString();
            var lines = raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            JsonConvert(raw);
        }

        private static void JsonConvert(string raw)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var list = JsonSerializer.Deserialize<List<TransactionDto>>(raw, options);

            ProcessTransactions(list);
            
        }

        private static void ProcessTransactions(List<TransactionDto> transactions)
        {
            for (int i = 0; i < transactions.Count; i++)
            {
                var tx = transactions[i];
                var op = (tx.Operation ?? string.Empty).Trim().ToLowerInvariant();
                var qtd = tx.Quantity;
                var price = tx.UnitCost;

                // Validações mínimas
                if (qtd <= 0)
                {
                    Console.Error.WriteLine($"Linha , item {i + 1}: quantity inválido ({qtd}). Deve ser > 0.");
                    continue;
                }
                if (price < 0m)
                {
                    Console.Error.WriteLine($"Linha, item {i + 1}: unit-cost inválido ({price}). Deve ser >= 0.");
                    continue;
                }

                if (op == "buy")
                {
                    // Ao comprar, recalculamos a média ponderada:
                    // nova-media = ((quantidade_atual * media_atual) + (qtd_comprada * preco_compra)) / (quantidade_atual + qtd_comprada)
                    //
                    // Se não houver ações atualmente (quantidade_atual == 0), a média passa a ser o preço da compra.
                    if (_quantidadeAtual == 0)
                    {
                        _mediaPonderadaAtual = price;
                        _quantidadeAtual = qtd;
                    }
                    else
                    {
                        var numerador = (_quantidadeAtual * _mediaPonderadaAtual) + (qtd * price);
                        var denominador = _quantidadeAtual + qtd;
                        _mediaPonderadaAtual = numerador / denominador;
                        _quantidadeAtual += qtd;
                    }

                    // Saída informativa
                    Console.WriteLine($"[Linha, item {i + 1}] BUY {qtd} @ {price:F2} -> nova quantidade: {_quantidadeAtual}, média ponderada: {_mediaPonderadaAtual:F2}");
                }
                else if (op == "sell")
                {
                    // Ao vender, o preço médio ponderado NÃO muda (sob o método de custo médio).
                    // Calculamos lucro/prejuízo comparando o preço de venda com a média ponderada atual.
                    if (qtd > _quantidadeAtual)
                    {
                        Console.Error.WriteLine($"Linha, item {i + 1}: tentativa de vender {qtd} ações mas possui apenas {_quantidadeAtual} (operação ignorada).");
                        continue;
                    }

                    var lucroPorAcao = price - _mediaPonderadaAtual;
                    var lucroTotal = lucroPorAcao * qtd;
                    var valorTotal = price * qtd;
                    lucroTotal = CalculoLucroPrejuizo(valorTotal,lucroTotal);
                    Console.WriteLine($"Prejuizo acumulado: {_prejuizoAcumulado:F2}");
                    _quantidadeAtual -= qtd;

                    // Se ficar com zero ações, redefinimos a média ponderada para 0 por clareza.
                    if (_quantidadeAtual == 0)
                    {
                        _mediaPonderadaAtual = 0m;
                    }

                    var tipo = lucroTotal > 0 ? "LUCRO" : (lucroTotal < 0 ? "PREJUÍZO" : "NEUTRO");
                    Console.WriteLine($"[Linha, item {i + 1}] SELL {qtd} @ {price:F2} -> {tipo}: {lucroTotal:F2} (por ação: {lucroPorAcao:F2}), quantidade restante: {_quantidadeAtual}, média ponderada: {_mediaPonderadaAtual:F2}");

                    if (valorTotal > _limiteIsencao)
                    {
                        //imposto
                        if (lucroTotal > _mediaPonderadaAtual)
                        {
                            _imposto += lucroTotal * 0.20m;
                            Console.WriteLine($"Imposto devido nesta venda: {lucroTotal * 0.20m:F2}. Imposto acumulado: {_imposto:F2}." );
                        }
                        Console.WriteLine($"Tax: {tx.Taxa}.");
                    }
                    else
                    {
                        Console.WriteLine($"Tax: {tx.Taxa}. Venda abaixo do limite de isenção de R$20.000,00. Sem imposto devido.");
                    }
                }
                else
                {
                    Console.Error.WriteLine($"Linha, item {i + 1}: operação desconhecida '{tx.Operation}'. Use 'buy' ou 'sell'.");
                }
            }
        }

        private static decimal CalculoLucroPrejuizo(decimal valorTotal, decimal lucroTotal)
        {
            if (valorTotal > _limiteIsencao)
            {
                if (lucroTotal > 0 && lucroTotal > _prejuizoAcumulado)
                {
                    lucroTotal += -_prejuizoAcumulado;
                }
                else
                {
                    _prejuizoAcumulado += lucroTotal;
                }
            }
            

            return lucroTotal;
        }
    }
}