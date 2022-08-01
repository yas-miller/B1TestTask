using B1TestTask.Data;

namespace B1TestTask.Helpers;

public static class BankAccountDetailsSummator
{
    public static BankAccountDetails SumClassBankAccountDetails(ClassDetails classDetails)
    {
        return SumBankAccountDetailsArray(classDetails.BankAccountDetailsArray);
    }
    
    public static BankAccountDetails SumAllClassesBankAccountDetails(List<ClassDetails> classDetailsArray)
    {
        var outputSumAllClassesBankAccountDetails = new BankAccountDetails
        {
            InputSaldoDetails = new InputSaldoDetails(), TurnoverDetails = new TurnoverDetails(), OutputSaldoDetails = new OutputSaldoDetails()
        };
        foreach (var classDetails in classDetailsArray)
        {
            var sum = SumClassBankAccountDetails(classDetails);
            outputSumAllClassesBankAccountDetails.InputSaldoDetails.Active += sum.InputSaldoDetails.Active;
            outputSumAllClassesBankAccountDetails.InputSaldoDetails.Passive += sum.InputSaldoDetails.Passive;
            outputSumAllClassesBankAccountDetails.TurnoverDetails.Debit += sum.TurnoverDetails.Debit;
            outputSumAllClassesBankAccountDetails.TurnoverDetails.Credit += sum.TurnoverDetails.Credit;
            outputSumAllClassesBankAccountDetails.OutputSaldoDetails.Active += sum.OutputSaldoDetails.Active;
            outputSumAllClassesBankAccountDetails.OutputSaldoDetails.Passive += sum.OutputSaldoDetails.Passive;
        }
        return outputSumAllClassesBankAccountDetails;
    }
    
    private static BankAccountDetails SumBankAccountDetailsArray(List<BankAccountDetails> bankAccountDetailsArray)
    {
        var outputSumClassBankAccountDetails = new BankAccountDetails
        {
            InputSaldoDetails = new InputSaldoDetails(), TurnoverDetails = new TurnoverDetails(), OutputSaldoDetails = new OutputSaldoDetails()
        };
        foreach (var bankAccountDetails in bankAccountDetailsArray.OrderBy(bad => bad.Number))
        {
            outputSumClassBankAccountDetails.InputSaldoDetails.Active += bankAccountDetails.InputSaldoDetails.Active;
            outputSumClassBankAccountDetails.InputSaldoDetails.Passive += bankAccountDetails.InputSaldoDetails.Passive;
            outputSumClassBankAccountDetails.TurnoverDetails.Debit += bankAccountDetails.TurnoverDetails.Debit;
            outputSumClassBankAccountDetails.TurnoverDetails.Credit += bankAccountDetails.TurnoverDetails.Credit;
            outputSumClassBankAccountDetails.OutputSaldoDetails.Active += bankAccountDetails.OutputSaldoDetails.Active;
            outputSumClassBankAccountDetails.OutputSaldoDetails.Passive += bankAccountDetails.OutputSaldoDetails.Passive;
        }
        return outputSumClassBankAccountDetails;
    }
    
    public static List<List<BankAccountDetails>> GroupSumClassBankAccountDetails(ClassDetails classDetails)
    {
        var outputGroupSumClassBankAccountDetails = new List<List<BankAccountDetails>>();
        var bankAccountDetailsArraySorted = classDetails.BankAccountDetailsArray.OrderBy(bad => bad.Number);
        var bankAccountNumberShort = bankAccountDetailsArraySorted.FirstOrDefault()?.Number.ToString().Substring(0, 2);
        var enumerator = bankAccountDetailsArraySorted.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return null;
        }
        
        do
        {
            BankAccountDetails sum;
            bankAccountNumberShort = enumerator.Current.Number.ToString().Substring(0, 2);
            outputGroupSumClassBankAccountDetails.Add(new List<BankAccountDetails>());

            while (bankAccountNumberShort == enumerator.Current.Number.ToString().Substring(0, 2))
            {
                outputGroupSumClassBankAccountDetails.Last().Add(new BankAccountDetails
                {
                    Number = enumerator.Current.Number,
                    InputSaldoDetails = enumerator.Current.InputSaldoDetails,
                    TurnoverDetails = enumerator.Current.TurnoverDetails,
                    OutputSaldoDetails = enumerator.Current.OutputSaldoDetails
                });
                
                if (!enumerator.MoveNext())
                {
                    sum = SumBankAccountDetailsArray(outputGroupSumClassBankAccountDetails.Last());
                    sum.Number = int.Parse(bankAccountNumberShort);
                    outputGroupSumClassBankAccountDetails.Last()
                        .Add(sum);
                    
                    return outputGroupSumClassBankAccountDetails;
                }
            }

            sum = SumBankAccountDetailsArray(outputGroupSumClassBankAccountDetails.Last());
            sum.Number = int.Parse(bankAccountNumberShort);
            outputGroupSumClassBankAccountDetails.Last()
                .Add(sum);
        }
        while (true);
    }
}
