using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallenger.Calculation
{
    public abstract class BaseCalculation
    {
        protected static int _quantidadeAtual = 0;
        protected static decimal _mediaPonderadaAtual = 0m;
        protected static decimal _prejuizoAcumulado = 0m;
        protected const decimal _limiteIsencao = 20000m;
        protected static decimal _imposto = 0m;

        public static void InicializationValues()
        {
            _quantidadeAtual = 0;
            _mediaPonderadaAtual = 0m;
            _prejuizoAcumulado = 0m;
            _imposto = 0m;
        }
    }
}
