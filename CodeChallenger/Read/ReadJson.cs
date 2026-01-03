using CodeChallenger.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeChallenger.Read
{
    public static class ReadJson
    {
        public static List<List<TransactionDto>> ReadJsonList()
        {
            string raw = ReadInputString();
            var arrays = ExtractJsonArrays(raw);

            return JsonConvertOperation(arrays);
        }

        private static string ReadInputString()
        {
            var sb = new StringBuilder();
            string? lineInput;

            while ((lineInput = Console.ReadLine()) != null)
            {
                var trimmed = lineInput.Trim();
                if (trimmed == "]")
                    break;
                sb.AppendLine(lineInput);
            }

            string raw = sb.ToString();
            return raw;
        }

        private static List<List<TransactionDto>> JsonConvertOperation(List<string> arrays)
        {
            var result = new List<List<TransactionDto>>(capacity: arrays.Count);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            for (int i = 0; i < arrays.Count; i++)
            {
                var arrayJson = arrays[i];

                try
                {
                    var list = JsonSerializer.Deserialize<List<TransactionDto>>(arrayJson, options);
                    if (list == null)
                    {
                        result.Add(new List<TransactionDto>());
                        continue;
                    }

                    result.Add(list);
                }
                catch (JsonException ex)
                {
                    result.Add(new List<TransactionDto>());
                }
                catch (Exception ex)
                {
                    result.Add(new List<TransactionDto>());
                }
            }

            return result;
        }

        private static List<string> ExtractJsonArrays(string raw)
        {
            var arrays = new List<string>();
            if (string.IsNullOrWhiteSpace(raw)) return arrays;

            int length = raw.Length;
            int index = 0;

            while (index < length)
            {
                int start = raw.IndexOf('[', index);
                if (start == -1) break;

                int depth = 0;
                int end = -1;
                for (int i = start; i < length; i++)
                {
                    char c = raw[i];
                    if (c == '[')
                    {
                        depth++;
                    }
                    else if (c == ']')
                    {
                        depth--;
                        if (depth == 0)
                        {
                            end = i;
                            break;
                        }
                    }
                }

                if (end != -1 && end > start)
                {
                    var arrayJson = raw.Substring(start, end - start + 1);
                    arrays.Add(arrayJson);
                    index = end + 1;
                }
                else
                {
                    break;
                }
            }

            return arrays;
        }
    }
}