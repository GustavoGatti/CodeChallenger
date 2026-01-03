using System;
using Xunit;
using CodeChallenger.Calculation;
using CodeChallenger.Dto;
using System.Collections.Generic;

namespace CodeChallenger.Tests.Calculation
{
    public class BuyTests
    {
        [Fact]
        public void ExecuteBuy_ReturnsZero()
        {
            // Arrange
            BaseCalculation.InicializationValues();
            var oper = new TransactionDto { Operation = "buy", UnitCost = 10m, Quantity = 100 };

            // Act
            var result = Buy.ExecuteBuy(oper);

            // Assert
            Assert.Equal(0m, result);
        }       

        [Fact]
        public void BuyThenSell_AboveLimit_CalculatesTax()
        {
            // Arrange
            BaseCalculation.InicializationValues();

            // buy 1000 at 10
            var buy = new TransactionDto { Operation = "buy", UnitCost = 10m, Quantity = 1000 };
            Buy.ExecuteBuy(buy);

            // Act
            // sell 1000 at 50 -> lucroPorAcao = 40, lucroTotal = 40000, tax = 20% => 8000
            var sell = new TransactionDto { Operation = "sell", UnitCost = 50m, Quantity = 1000 };
            var tax = Sell.ExecuteSell(sell);

            // Assert
            Assert.Equal(8000m, tax);
        }
    }
}
