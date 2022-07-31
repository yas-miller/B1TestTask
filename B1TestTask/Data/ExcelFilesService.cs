using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
                var excelFile = new ExcelFile
                {
                    FullName = loadedExcelFile.FullName, Name = Path.GetFileNameWithoutExtension(loadedExcelFile.Name), Extension = loadedExcelFile.Extension
                };
                db.ExcelFiles.Add(excelFile);
                db.SaveChanges();

                new ExcelFileParser(ref excelFile).parseExcelFile();
                db.SaveChanges();
            }
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

    public ExcelFile? GetExcelFileById(int id)
    {
        // получение данных
        using (var db = new ApplicationDBContext())
        {
            return db.ExcelFiles
                .Where(ef => ef.Id == id)
                .Include(ef => ef.ExcelFileReportDetails)
                .ThenInclude(efrd => efrd.ClassDetailsArray)
                .ThenInclude(cda => cda.BankAccountDetailsArray)
                .ThenInclude(bada => bada.InputSaldoDetails)
                .Include(ef => ef.ExcelFileReportDetails)
                .ThenInclude(efrd => efrd.ClassDetailsArray)
                .ThenInclude(cda => cda.BankAccountDetailsArray)
                .ThenInclude(bada => bada.TurnoverDetails)
                .Include(ef => ef.ExcelFileReportDetails)
                .ThenInclude(efrd => efrd.ClassDetailsArray)
                .ThenInclude(cda => cda.BankAccountDetailsArray)
                .ThenInclude(bada => bada.OutputSaldoDetails)
                .FirstOrDefault();
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
