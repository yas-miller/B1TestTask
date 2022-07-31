using B1TestTask.Data;

namespace B1TestTask.Helpers;

public static class BankAccountDetailsAdder
{
    public static BankAccountDetails ClassBankAccountDetailsAdder(ClassDetails classDetails)
    {
        var output = new BankAccountDetails { InputSaldoDetails = new InputSaldoDetails(), TurnoverDetails = new TurnoverDetails(), OutputSaldoDetails = new OutputSaldoDetails() };
        foreach (var bankAccountDetails in classDetails.BankAccountDetailsArray)
        {
            output.deta
        }
    }
    
    public static BankAccountDetails BankAccountDetailsAdder(List<BankAccountDetails> bankAccountDetailsList)
    {
        
    }
}