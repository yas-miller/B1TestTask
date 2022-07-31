namespace B1TestTask.Data;

public class ClassDetails
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Title { get; set; }

    public List<BankAccountDetails> BankAccountDetailsArray { get; set; }
}