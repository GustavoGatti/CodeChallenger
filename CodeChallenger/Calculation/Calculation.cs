using CodeChallenger.Dto;

namespace CodeChallenger.Calculation
{
    public class Calculation : BaseCalculation
    {
        public static List<List<TaxDto>> ExecuteCalculation(List<List<TransactionDto>> transactions)
        {
            var taxes = new List<List<TaxDto>>();
            foreach (var list in transactions)
            {
                taxes.Add(ProcessListOperation(list));
                InicializationValues();
            }
            return taxes;
        }

        private static List<TaxDto> ProcessListOperation(List<TransactionDto> list)
        {
            var taxes = new List<TaxDto>();
            foreach (var oper in list)
            {
                bool flowControl = OperationValidation(oper);

                if (!flowControl)
                {
                    continue;
                }
                taxes.Add(OperationKind(oper));
            }
            return taxes;
        }

        private static TaxDto OperationKind(TransactionDto oper)
        {
            var taxes = new TaxDto();
            switch (oper.Operation)
            {
                case "buy":
                    taxes = new TaxDto { Tax = Buy.ExecuteBuy(oper) };
                    break;
                case "sell":
                    taxes = new TaxDto { Tax = Sell.ExecuteSell(oper) };
                    break;
                default:
                    break;
            }
            return taxes;
        }

        private static bool OperationValidation(TransactionDto oper)
        {
            if (oper.Quantity <= 0)
            {
                return false;
            }
            if (oper.UnitCost < 0m)
            {
                return false;
            }
            return true;
        }

    }
}
