using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CodeChallenger.Dto
{
    public class TransactionDto
    {
        [JsonPropertyName("operation")]
        public required string? Operation { get; set; }
        [JsonPropertyName("unit-cost")]
        public decimal UnitCost { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("tax")]
        public decimal Taxa { get; set; }
    }
}
