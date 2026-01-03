using System;
using Xunit;
using CodeChallenger.Calculation;
using CodeChallenger.Dto;

namespace CodeChallenger.Tests.Calculation
{
    public class SellTests
    {
        [Fact]
        public void ExecuteSell_NoTax_WhenBelowLimit()
        {
            // Arrange
            BaseCalculation.InicializationValues();
            Buy.ExecuteBuy(new TransactionDto { Operation = "buy", UnitCost = 10m, Quantity = 100 });

            // Act
            var tax = Sell.ExecuteSell(new TransactionDto { Operation = "sell", UnitCost = 100m, Quantity = 100 });

            // Assert
            Assert.Equal(0m, tax);
        }

        [Fact]
        public void ExecuteSell_AppliesTax_WhenAboveLimit()
        {
            // Arrange
            BaseCalculation.InicializationValues();
            Buy.ExecuteBuy(new TransactionDto { Operation = "buy", UnitCost = 10m, Quantity = 1000 });

            // Act
            var tax = Sell.ExecuteSell(new TransactionDto { Operation = "sell", UnitCost = 50m, Quantity = 1000 });

            // Assert
            Assert.Equal(8000m, tax);
        }

        [Fact]
        public void ExecuteSell_MultiplePartialSells_CumulativeTax()
        {
            // Arrange
            BaseCalculation.InicializationValues();
            Buy.ExecuteBuy(new TransactionDto { Operation = "buy", UnitCost = 10m, Quantity = 1000 });

            // Act
            var taxAfterFirstSell = Sell.ExecuteSell(new TransactionDto { Operation = "sell", UnitCost = 50m, Quantity = 500 });
            var taxAfterSecondSell = Sell.ExecuteSell(new TransactionDto { Operation = "sell", UnitCost = 50m, Quantity = 500 });

            // Assert
            // first partial sell: lucroPorAcao = 40, lucroTotal = 40 * 500 = 20000 -> tax = 20% => 4000
            Assert.Equal(4000m, taxAfterFirstSell);
            // second partial sell accumulates additional 4000 -> total 8000
            Assert.Equal(8000m, taxAfterSecondSell);
        }
    }
}
