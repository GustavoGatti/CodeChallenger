using CodeChallenger.Calculation;
using CodeChallenger.Read;
using CodeChallenger.Write;

public class Program
{
    private static void Main(string[] args)
    {
        var opers = ReadJson.ReadJsonList();
        var taxes = Calculation.ExecuteCalculation(opers);
        WriteJson.WriteJsonList(taxes);
    }
}