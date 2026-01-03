using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using CodeChallenger.Read;
using CodeChallenger.Dto;

namespace CodeChallenger.Tests.Read
{
    public class ReadJsonTests
    {
        [Fact]
        public void ReadJsonList_EmptyInput_ReturnsEmptyList()
        {
            // Arrange
            var input = string.Empty;
            using var sr = new StringReader(input);
            var originalIn = Console.In;
            Console.SetIn(sr);

            try
            {
                // Act
                var result = ReadJson.ReadJsonList();

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
            finally
            {
                Console.SetIn(originalIn);
            }
        }

        [Fact]
        public void ReadJsonList_SingleEmptyArray_ReturnsOneEmptyInnerList()
        {
            // Arrange
            var input = "[]";
            using var sr = new StringReader(input);
            var originalIn = Console.In;
            Console.SetIn(sr);

            try
            {
                // Act
                var result = ReadJson.ReadJsonList();

                // Assert
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Empty(result[0]);
            }
            finally
            {
                Console.SetIn(originalIn);
            }
        }

        [Fact]
        public void ReadJsonList_TwoEmptyArrays_ReturnsTwoEmptyInnerLists()
        {
            // Arrange
            // two arrays concatenated -> ExtractJsonArrays deve identificar ambas
            var input = "[][]";
            using var sr = new StringReader(input);
            var originalIn = Console.In;
            Console.SetIn(sr);

            try
            {
                // Act
                var result = ReadJson.ReadJsonList();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Empty(result[0]);
                Assert.Empty(result[1]);
            }
            finally
            {
                Console.SetIn(originalIn);
            }
        }

        [Fact]
        public void ReadJsonList_InvalidJson_ReturnsEmptyInnerList()
        {
            // Arrange
            var input = "[invalid]";
            using var sr = new StringReader(input);
            var originalIn = Console.In;
            Console.SetIn(sr);

            try
            {
                // Act
                var result = ReadJson.ReadJsonList();

                // Assert
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Empty(result[0]);
            }
            finally
            {
                Console.SetIn(originalIn);
            }
        }


    }
}