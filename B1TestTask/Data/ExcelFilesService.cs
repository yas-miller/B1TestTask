using Microsoft.Extensions.FileProviders;

namespace B1TestTask.Data;

public class ExcelFilesService
{
    public void RegisterExcelFiles(IReadOnlyList<FileInfo> loadedExcelFiles)
    {
        // добавление записей excel-файлов в БД
        using (var db = new ApplicationDBContext())
        {
            foreach (var loadedExcelFile in loadedExcelFiles)
            {
                db.ExcelFiles.Add(new ExcelFile {FullName = loadedExcelFile.FullName, Name = loadedExcelFile.Name, Extension = loadedExcelFile.Extension});
            }
            db.SaveChanges();
        }
    }
    
    public void SaveExcelFile(ExcelFile excelFile)
    {
        // добавление данных
        using (var db = new ApplicationDBContext())
        {
            db.ExcelFiles.Add(excelFile);
            db.SaveChanges();
        }
    }

    public ExcelFile[] GetExcelFiles(DateTime? fromDate = null, DateTime? toDate = null)
    {
        // получение данных
        using (var db = new ApplicationDBContext())
        {
            var excelFiles = db.ExcelFiles.ToArray();
            return excelFiles;
        }
    }
}
