using CodeChallenger.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallenger.Calculation
{
    public class Buy: BaseCalculation
    {
        public static decimal ExecuteBuy(TransactionDto oper)
        {
            if (_quantidadeAtual == 0)
            {
                _mediaPonderadaAtual = oper.UnitCost;
                _quantidadeAtual = oper.Quantity;
            }
            else
            {
                var numerador = (_quantidadeAtual * _mediaPonderadaAtual) + (oper.Quantity * oper.UnitCost);
                var denominador = _quantidadeAtual + oper.Quantity;
                _mediaPonderadaAtual = numerador / denominador;
                _quantidadeAtual += oper.Quantity;
            }
            return 0;
        }
    }
}
