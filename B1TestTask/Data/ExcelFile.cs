namespace B1TestTask.Data;

public class ExcelFile
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public string FullName { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime AddedDate { get; set; }

    public ExcelFile()
    {
        AddedDate = DateTime.Now;
    }
}