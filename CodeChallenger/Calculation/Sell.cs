using CodeChallenger.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallenger.Calculation
{
    public class Sell: BaseCalculation
    {
        public static decimal ExecuteSell(TransactionDto oper)
        {

            var lucroPorAcao = oper.UnitCost - _mediaPonderadaAtual;
            var lucroTotal = lucroPorAcao * oper.Quantity;
            var valorTotal = oper.UnitCost * oper.Quantity;


            lucroTotal = CalculoLucroPrejuizo(valorTotal, lucroTotal);

            _quantidadeAtual -= oper.Quantity;

            if (_quantidadeAtual == 0)
            {
                _mediaPonderadaAtual = 0m;
            }

            var tipo = lucroTotal > 0 ? "LUCRO" : (lucroTotal < 0 ? "PREJUÍZO" : "NEUTRO");

            if (IsVendaAcimaLimite(valorTotal))
            {
                if (lucroTotal > _mediaPonderadaAtual)
                {
                    _imposto += lucroTotal * 0.20m;   
                }
            }
          
            return _imposto;
        }

        private static bool IsVendaAcimaLimite(decimal valorTotal)
        {
            return valorTotal > _limiteIsencao;
        }

        private static decimal CalculoLucroPrejuizo(decimal valorTotal, decimal lucroTotal)
        {
            if (IsVendaAcimaLimite( valorTotal))
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
