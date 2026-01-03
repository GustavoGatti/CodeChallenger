using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace CodeChallenger.Dto
{
    [ExcludeFromCodeCoverage]
    public class TransactionDto
    {
        public required string? Operation { get; set; }
        [JsonPropertyName("unit-cost")]
        public decimal UnitCost { get; set; }
        public int Quantity { get; set; }
    }
}
