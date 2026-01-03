using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using CodeChallenger.Write;
using CodeChallenger.Dto;

namespace CodeChallenger.Tests.Write
{
    public class WriteJsonTests
    {
        [Fact]
        public void WriteJsonList_WritesSerializedListsToConsole()
        {
            // Arrange
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var inner1 = new List<TaxDto> { new TaxDto { Tax = 1.23m } };
            var inner2 = new List<TaxDto> { new TaxDto { Tax = 4.56m } };

            var output = new List<List<TaxDto>> { inner1, inner2 };

            var expectedLines = new[]
            {
                JsonSerializer.Serialize(inner1, options),
                JsonSerializer.Serialize(inner2, options)
            };

            var originalOut = Console.Out;
            try
            {
                using var sw = new StringWriter();
                Console.SetOut(sw);

                // Act
                WriteJson.WriteJsonList(output);

                // Assert
                sw.Flush();
                var captured = sw.ToString();
                var lines = captured.Replace("\r\n", "\n").Split('\n', StringSplitOptions.RemoveEmptyEntries);

                Assert.Equal(expectedLines.Length, lines.Length);
                for (int i = 0; i < expectedLines.Length; i++)
                {
                    Assert.Equal(expectedLines[i], lines[i]);
                }
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }
}