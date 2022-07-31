using B1TestTask.Data;

namespace B1TestTask.Helpers;

public static class BankAccountDetailsSummator
{
    public static BankAccountDetails SumClassBankAccountDetails(ClassDetails classDetails)
    {
        var outputSumClassBankAccountDetails = new BankAccountDetails
        {
            InputSaldoDetails = new InputSaldoDetails(), TurnoverDetails = new TurnoverDetails(), OutputSaldoDetails = new OutputSaldoDetails()
        };
        foreach (var bankAccountDetails in classDetails.BankAccountDetailsArray)
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
    
    public static BankAccountDetails SumBankAccountDetails(List<BankAccountDetails> bankAccountDetailsList)
    {
        throw new NotImplementedException();
    }
}