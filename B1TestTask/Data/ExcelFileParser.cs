using System.Globalization;
using System.Text.RegularExpressions;
using B1TestTask.Helpers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Row = DocumentFormat.OpenXml.Spreadsheet.Row;

namespace B1TestTask.Data;

public class ExcelFileParser
{
    private ExcelFile excelFile;
    public ExcelFileParser(ref ExcelFile excelFile)
    {
        this.excelFile = excelFile;
    }

    public void parseExcelFile()
    {
        try
        {
            this.excelFile.ExcelFileReportDetails = new ExcelFileReportDetails();

            checkInputExcelFileExtension();
            parseReportCommonInfo();

            this.excelFile.ExcelFileReportDetails.IsValidReport = true;
        }
        catch
        {
            this.excelFile.ExcelFileReportDetails = null;
            throw;
        }
    }
    

    private void parseReportCommonInfo()
    {
        var fileName = this.excelFile.FullName;
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

            //Get sheet from excel
            var sheets = workbookPart.Workbook.Descendants<Sheet>();

            //First sheet from excel
            Sheet sheet = sheets.FirstOrDefault();

            var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
            var rows = worksheetPart.Worksheet.Descendants<Row>().ToList();

            //Get all data rows from sheet
            Row headerRow = rows.First();
            var headerCells = headerRow.Elements<Cell>();
            int totalColumns = headerCells.Count();


            List<string> lstHeaders = new List<string>();
            foreach (var value in headerCells)
            {
                var stringId = Convert.ToInt32(value.InnerText == "" ? 0 : value.InnerText);
                lstHeaders.Add(workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText);
            }

            //Dictionary to map row data into key value pair
            Dictionary<string, List<KeyValuePair<string, string>>> dict = new Dictionary<string, List<KeyValuePair<string, string>>>();
            
            //Iterate to all rows
            foreach (Row r in rows)
            { //если строка
                //Iterate to all cell in current row
                foreach (Cell c in r.Elements<Cell>())
                {
                    string val;
                    if (c.DataType != null && c.DataType == CellValues.SharedString)
                    {
                        var stringId = Convert.ToInt32(c.InnerText);
                        val = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText;
                    }
                    else
                    {
                        val = c.InnerText;
                    }
                    
                    
                    // Reading stub constants
                    switch (c.CellReference.ToString())
                    {
                        case "A1":
                            this.excelFile.ExcelFileReportDetails.BankName = val;
                            break;
                        case "A2":
                            this.excelFile.ExcelFileReportDetails.Title = val;
                            break;
                        case "A3":
                            List<DateTime> dates = new List<DateTime>();
                            foreach (var subStr in val.Split(" "))
                            {
                                DateTime dateTime;
                                if (DateTime.TryParse(subStr, out dateTime))
                                {
                                    dates.Add(DateTime.Parse(subStr));
                                }
                            }

                            if (dates.Count != 2)
                            {
                                throw new Exception("Ошибка при парсинге дат начала и конца отчета");
                            }
                            this.excelFile.ExcelFileReportDetails.FromDate = dates[0];
                            this.excelFile.ExcelFileReportDetails.ToDate = dates[1];
                            break;
                        case "A4":
                            this.excelFile.ExcelFileReportDetails.TargetName = val;
                            break;
                        case "A6":
                            DateTime generatedDateTime;
                            double doubleVal;
                            if (DateTime.TryParse(val, out generatedDateTime))
                            {
                                this.excelFile.ExcelFileReportDetails.GeneratedDate = generatedDateTime;
                            }
                            else
                            {
                                generatedDateTime = new DateTime(1900, 1, 1);
                                if (double.TryParse(val, out doubleVal))
                                {
                                    generatedDateTime = generatedDateTime.AddDays(doubleVal - 2);
                                }
                                this.excelFile.ExcelFileReportDetails.GeneratedDate = generatedDateTime;
                            }
                            break;
                        case "G6":
                            this.excelFile.ExcelFileReportDetails.Currency = val;
                            break;
                    }

                    switch (GetColumnIndex(c.CellReference))
                    {
                        case 1:
                            if (val.Contains("КЛАСС "))
                            {
                                if (this.excelFile.ExcelFileReportDetails.ClassDetailsArray is null)
                                {
                                    this.excelFile.ExcelFileReportDetails.ClassDetailsArray =
                                        new List<ClassDetails>();
                                }

                                var cd = new ClassDetails();
                                cd.Title = String.Join(" ", val.Split("  ").Skip(2));
                                cd.Number = int.Parse(val.Split()[2]);
                                
                                this.excelFile.ExcelFileReportDetails.ClassDetailsArray.Add(cd);
                            }

                            int bankAccountNumber;
                            if (int.TryParse(val, out bankAccountNumber) && bankAccountNumber.ToString().Length == 4)
                            {
                                if (this.excelFile.ExcelFileReportDetails.ClassDetailsArray.Last().BankAccountDetailsArray is null)
                                {
                                    this.excelFile.ExcelFileReportDetails.ClassDetailsArray.Last()
                                            .BankAccountDetailsArray =
                                        new List<BankAccountDetails>();
                                }

                                BankAccountDetails bankAccountDetails = new BankAccountDetails();
                                bankAccountDetails.Number = bankAccountNumber;

                                var rawBankAccountDetailsValues = parseBankAccountDetailsRow(workbookPart, r);
                                bankAccountDetails.InputSaldoDetails = new InputSaldoDetails
                                {
                                    Active = rawBankAccountDetailsValues[0],
                                    Passive = rawBankAccountDetailsValues[1]
                                };
                                bankAccountDetails.TurnoverDetails = new TurnoverDetails
                                {
                                    Debit = rawBankAccountDetailsValues[2],
                                    Credit = rawBankAccountDetailsValues[3]
                                };
                                bankAccountDetails.OutputSaldoDetails = new OutputSaldoDetails
                                {
                                    Active = rawBankAccountDetailsValues[4],
                                    Passive = rawBankAccountDetailsValues[5]
                                };
                                
                                this.excelFile.ExcelFileReportDetails.ClassDetailsArray.Last().BankAccountDetailsArray.Add(bankAccountDetails);
                            }
                            break;

                        case 2:
                            
                            break;

                        case 3:
                            
                            break;

                        case 4:
                            
                            break;

                        case 5:
                            
                            break;

                        case 6:
                            
                            break;
                        
                        case 7:
                            
                            break;
                    }
                }
            }
        }
    }

    private List<decimal> parseBankAccountDetailsRow(WorkbookPart workbookPart, Row r)
    {
        List<decimal> outputRawValues = new List<decimal>(6);
        foreach (Cell c in r.Elements<Cell>())
        {
            try
            {
                string val;
                if (c.DataType != null && c.DataType == CellValues.SharedString)
                {
                    var stringId = Convert.ToInt32(c.InnerText);
                    val = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText;
                }
                else
                {
                    val = c.InnerText;
                }

                if (GetColumnIndex(c.CellReference) >= 2 && GetColumnIndex(c.CellReference) <= 7)
                {
                    outputRawValues.Add(decimal.Parse(val, CultureInfo.InvariantCulture));
                }
            }
            catch (FormatException exc)
            {
                if (c.InnerText == "" && outputRawValues.Count == 6)
                {
                    return outputRawValues;
                }
                throw;
            }
        }
        return outputRawValues;
    }
    
    private static int? GetColumnIndex(string cellReference)
    {
        if (string.IsNullOrEmpty(cellReference))
        {
            return null;
        }

        string columnReference = Regex.Replace(cellReference.ToUpper(), @"[\d]", string.Empty);

        int columnNumber = -1;
        int mulitplier = 1;

        foreach (char c in columnReference.ToCharArray().Reverse())
        {
            columnNumber += mulitplier * ((int)c - 64);

            mulitplier = mulitplier * 26;
        }

        return columnNumber + 1;
    }
    
    private void checkInputExcelFileExtension()
    {
        if (this.excelFile.Extension == ".xlsx")
        {

        }
        else if (this.excelFile.Extension == ".xls")
        {
            convertXlsToXlsx();
        }
        else throw new NotValidFileExtension("Расширение файла не .xlsx (.xls)");
    }
    
    private void convertXlsToXlsx()
    {
        XlsToXlsx converter = new XlsToXlsx();
        string path = this.excelFile.FullName;
        var destinationPath = Path.GetDirectoryName(path) + $"/{excelFile.Name}.xlsx";
        /*if (File.Exists(destinationPath))
        {
            File.Delete(destinationPath);
        }*/
        converter.ConvertToXlsxFile(path, destinationPath);

        this.excelFile.FullName = Path.GetDirectoryName(this.excelFile.FullName) + $"/{excelFile.Name}.xlsx";
        this.excelFile.Extension = ".xlsx";

        using (ApplicationDBContext db = new ApplicationDBContext())
        {
            db.ExcelFiles.Update(this.excelFile);
            db.SaveChanges();
        }
    }
}

public class NotValidFileExtension : Exception
{
    public NotValidFileExtension(string? message): base(message)
    {

    }
}
