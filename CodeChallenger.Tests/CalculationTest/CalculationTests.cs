using System;
using System.Collections.Generic;
using Xunit;
using CodeChallenger.Calculation;
using CodeChallenger.Dto;

namespace CodeChallenger.Tests.Calculation
{
    public class CalculationTests
    {
        [Fact]
        public void ExecuteCalculation_EmptyTransactions_ReturnsEmptyTaxes()
        {
            BaseCalculation.InicializationValues();
            var input = new List<List<TransactionDto>>();

            var result = CodeChallenger.Calculation.Calculation.ExecuteCalculation(input);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ExecuteCalculation_OneEmptyInnerList_ReturnsEmptyInnerTaxes()
        {
            BaseCalculation.InicializationValues();
            var input = new List<List<TransactionDto>> { new List<TransactionDto>() };

            var result = CodeChallenger.Calculation.Calculation.ExecuteCalculation(input);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Empty(result[0]);
        }

        [Fact]
        public void ExecuteCalculation_BuyTransaction_ProducesZeroTax()
        {
            BaseCalculation.InicializationValues();
            var input = new List<List<TransactionDto>>
            {
                new List<TransactionDto>
                {
                    new TransactionDto { Operation = "buy", UnitCost = 10m, Quantity = 100 }
                }
            };

            var result = CodeChallenger.Calculation.Calculation.ExecuteCalculation(input);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Single(result[0]);
            Assert.Equal(0m, result[0][0].Tax);
        }
    }
}
