using CodeChallenger.Dto;
using System.Text.Json;

namespace CodeChallenger.Write
{
    public static class WriteJson
    {
        public static void WriteJsonList(List<List<TaxDto>> output)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            foreach (var item in output)
            {
                var oper = JsonSerializer.Serialize(item, options);
                Console.WriteLine(oper);
            }
        }
    }
}
