namespace B1TestTask.Data;

public class ExcelFileReportDetails
{
    public int Id { get; set; }
    public string BankName { get; set; }
    public string? Title { get; set; }

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? GeneratedDate { get; set; }
    
    public string? Currency { get; set; }
    
    public List<ClassDetails> ClassDetailsArray { get; set; }
}