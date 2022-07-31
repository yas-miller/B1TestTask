namespace B1TestTask.Data;

public class BankAccountDetails
{
    public int Id { get; set; }
    public int Number { get; set; }
    public InputSaldoDetails InputSaldoDetails { get; set; }
    public TurnoverDetails TurnoverDetails { get; set; }

    public OutputSaldoDetails OutputSaldoDetails { get; set; }
}