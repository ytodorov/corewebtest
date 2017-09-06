using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xfinium.Pdf;
using Xfinium.Pdf.Content;
using Xfinium.Pdf.Graphics;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class Utils
    {
        public static void PopulateHungarianMappingDictionaries(string dirWithFiles)
        {

            string dirPath = Path.Combine(dirWithFiles, "Files", "HungarianDistributionKeys.xlsx");
            FileInfo fileDistributionInfo = new FileInfo(dirPath);

            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (fileDistributionInfo.Exists)
            {
                using (ExcelPackage package = new ExcelPackage(fileDistributionInfo))
                {
                    for (int page = 1; page <= 2; page++)
                    {
                        ExcelWorksheet currentSheet = package.Workbook.Worksheets[page];

                        for (int i = 1; i <= 113; i++)
                        {
                            // Проверка за бял цвят
                            if (string.IsNullOrEmpty(currentSheet.Cells[i, 1].Style.Fill.BackgroundColor.Rgb))
                            {
                                // Проверка за невалиден ред
                                if (!string.IsNullOrWhiteSpace(currentSheet.Cells[i, 1].Value?.ToString()) &&
                                    !string.IsNullOrWhiteSpace(currentSheet.Cells[i, 3].Value?.ToString()))
                                {
                                    var inputValue = currentSheet.Cells[i, 1].Value.ToString().Substring(0, 3);
                                    var hungName = currentSheet.Cells[i, 1].Value.ToString().Substring(5);
                                    var goesTo = currentSheet.Cells[i, 2].Value.ToString();
                                    var goesToRowNumberString = currentSheet.Cells[i, 3]?.Value?.ToString();
                                    FinancialRow fr = new FinancialRow();
                                    fr.Number = inputValue;
                                    fr.Name = hungName;
                                    fr.GoesToRowTitle = goesTo;
                                    if (int.TryParse(goesToRowNumberString, out int goesToRowNumberInt))
                                    {
                                        fr.GoesToRowNumber = goesToRowNumberInt;
                                    }

                                    if (page == 1)
                                    {
                                        fr.Type = "BS";
                                        Mappings.HungarianBsRows.Add(fr);
                                    }
                                    else if (page == 2)
                                    {
                                        fr.Type = "PL";
                                        Mappings.HungarianPlRows.Add(fr);
                                    }
                                }
                            }
                        }
                    }



                }
            }

            var bsRows = Mappings.HungarianBsRows;
            var plRows = Mappings.HungarianPlRows;

        }

        public static void PopulateSerbianMappingDictionaries(string dirWithFiles)
        {

            string dirPath = Path.Combine(dirWithFiles, "Files", "SerbianDistributionKeys.xlsx");
            FileInfo fileDistributionInfo = new FileInfo(dirPath);

            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (fileDistributionInfo.Exists)
            {
                using (ExcelPackage package = new ExcelPackage(fileDistributionInfo))
                {
                    for (int page = 1; page <= 2; page++)
                    {
                        ExcelWorksheet currentSheet = package.Workbook.Worksheets[page];

                        for (int i = 1; i <= 150; i++)
                        {
                            // Проверка за бял цвят
                            if (string.IsNullOrEmpty(currentSheet.Cells[i, 1].Style.Fill.BackgroundColor.Rgb))
                            {
                                // Проверка за невалиден ред
                                if (!string.IsNullOrWhiteSpace(currentSheet.Cells[i, 1].Value?.ToString()) &&
                                    !string.IsNullOrWhiteSpace(currentSheet.Cells[i, 3].Value?.ToString()))
                                {
                                    var inputValue = currentSheet.Cells[i, 2]?.Value?.ToString();
                                    var name = currentSheet.Cells[i, 1].Value.ToString();
                                    //var goesTo = currentSheet.Cells[i, 2].Value.ToString();
                                    var goesToRowNumberString = currentSheet.Cells[i, 3]?.Value?.ToString();
                                    FinancialRow fr = new FinancialRow();
                                    fr.Number = inputValue;
                                    fr.Name = name;
                                    //fr.GoesToRowTitle = goesTo;
                                    if (int.TryParse(goesToRowNumberString, out int goesToRowNumberInt))
                                    {
                                        fr.GoesToRowNumber = goesToRowNumberInt;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                    if (page == 1)
                                    {
                                        fr.Type = "BS";
                                        Mappings.SerbianBsRows.Add(fr);
                                    }
                                    else if (page == 2)
                                    {
                                        fr.Type = "PL";
                                        Mappings.SerbianPlRows.Add(fr);
                                    }
                                }
                            }
                        }
                    }



                }
            }

            var bsRows = Mappings.SerbianBsRows;
            var plRows = Mappings.SerbianPlRows;

        }

        public static void PopulateCroatianMappingDictionaries(string dirWithFiles)
        {

            string dirPath = Path.Combine(dirWithFiles, "Files", "CroatianDistributionKeys.xlsx");
            FileInfo fileDistributionInfo = new FileInfo(dirPath);

            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (fileDistributionInfo.Exists)
            {
                using (ExcelPackage package = new ExcelPackage(fileDistributionInfo))
                {
                    for (int page = 1; page <= 2; page++)
                    {
                        ExcelWorksheet currentSheet = package.Workbook.Worksheets[page];

                        for (int i = 2; i <= 150; i++)
                        {
                            // Проверка за бял цвят
                            if (string.IsNullOrEmpty(currentSheet.Cells[i, 1].Style.Fill.BackgroundColor.Rgb))
                            {
                                // Проверка за невалиден ред
                                if (!string.IsNullOrWhiteSpace(currentSheet.Cells[i, 1].Value?.ToString()) &&
                                    !string.IsNullOrWhiteSpace(currentSheet.Cells[i, 2].Value?.ToString()))
                                {
                                    var inputValue = currentSheet.Cells[i, 2]?.Value?.ToString();
                                    var name = currentSheet.Cells[i, 1].Value.ToString();
                                    var goesToRowNumberString = currentSheet.Cells[i, 2]?.Value?.ToString();
                                    FinancialRow fr = new FinancialRow();
                                    string number = name.Split(".")[0];
                                    fr.Number = number;
                                    fr.Name = name;
                                    //fr.GoesToRowTitle = goesTo;
                                    if (int.TryParse(goesToRowNumberString, out int goesToRowNumberInt))
                                    {
                                        fr.GoesToRowNumber = goesToRowNumberInt;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                    if (page == 1)
                                    {
                                        fr.Type = "BS";
                                        Mappings.CroatiaBsRows.Add(fr);
                                    }
                                    else if (page == 2)
                                    {
                                        fr.Type = "PL";
                                        Mappings.CroatiaPlRows.Add(fr);
                                    }
                                }
                            }
                        }
                    }



                }
            }

            var bsRows = Mappings.SerbianBsRows;
            var plRows = Mappings.SerbianPlRows;

        }

        public static ExcelInputData GetExcelValues(List<FinancialRow> bsRows, List<FinancialRow> plRows)
        {
            // balance
            ExcelInputData result = new ExcelInputData();
            result.BsValues = new List<ExcellOutputRowData>();
            result.PlValues = new List<ExcellOutputRowData>();

            var bsGroup = bsRows.GroupBy(h => h.GoesToRowNumber);

            foreach (var group in bsGroup)
            {

                var sumCurrentYear = (int)group.Sum(g => g.CurrentYear);
                var sumPreviousYear = (int)group.Sum(g => g.PreviousYear);
                ExcellOutputRowData excellOutputRowData = new ExcellOutputRowData()
                {
                    RowNumber = group.Key,
                    CurrentYear = sumCurrentYear,
                    PreviousYear = sumPreviousYear
                };

                result.BsValues.Add(excellOutputRowData);

                //var sumPreviousYear = (int)group.Sum(g => g.PreviousYear);
                //result.BsValues.Add(group.Key, sumCurrentYear);
            }

            var plGroup = plRows.GroupBy(h => h.GoesToRowNumber);

            foreach (var group in plGroup)
            {
                var sumCurrentYear = (int)group.Sum(g => g.CurrentYear);
                var sumPreviousYear = (int)group.Sum(g => g.PreviousYear);
                ExcellOutputRowData excellOutputRowData = new ExcellOutputRowData()
                {
                    RowNumber = group.Key,
                    CurrentYear = sumCurrentYear,
                    PreviousYear = sumPreviousYear
                };
                result.PlValues.Add(excellOutputRowData);
            }
            return result;
        }

        public static string GetExcelOutputFilePath(string rootFolder, string pdfFilePath)
        {
            CountryFileTypes countryFileType = Utils.GetCountryFileTypesFromPdfFile(pdfFilePath);
            FileInfo fileEmptyOutput = new FileInfo(Path.Combine(rootFolder, "Files", "OUTPUT.xlsm"));
            if (!fileEmptyOutput.Exists)
            {
                throw new ApplicationException("Няма го файла OUTPUT.xlsm в папка Files");
            }

            CompanyPdfMetaData companyPdfMetaData = GetCompanyPdfMetaData(pdfFilePath);

            string outputFileName = $"OUTPUT_{companyPdfMetaData.CompanyRegistrationNumber}_{companyPdfMetaData.CompanyTaxNumber}_{DateTime.Now.Ticks}.xlsm";





            var invalidChars = Path.GetInvalidPathChars();
            foreach (var invalidChar in invalidChars)
            {
                outputFileName = outputFileName.Replace(invalidChar.ToString(), "");
            }
            string outputFilePath = Path.Combine(rootFolder, "OutputFiles", outputFileName);
            FileInfo fileInfoOutput = new FileInfo(outputFilePath);

            fileEmptyOutput.CopyTo(fileInfoOutput.FullName, true);

            using (ExcelPackage package = new ExcelPackage(fileEmptyOutput))
            {

                ExcelInputData excelInputData = null;
                if (countryFileType == CountryFileTypes.Hungarian)
                {
                    var parsedPdf = ParseHungarianPdf(pdfFilePath);
                    excelInputData = GetExcelValues(parsedPdf.BsRows, parsedPdf.PlRows);
                }
                else if (countryFileType == CountryFileTypes.Serbian)
                {
                    var parsedPdf = ParseSerbianPdf(pdfFilePath);
                    excelInputData = GetExcelValues(parsedPdf.BsRows, parsedPdf.PlRows);
                }

                ExcelRange cellsBS = package.Workbook.Worksheets[1].Cells;

                string balanceSheetDateCellName = "D4";
                string fiscalYearMonthCellName = "D5";

                var testVal = cellsBS[balanceSheetDateCellName]?.GetValue<DateTime>();
                var testVal2 = cellsBS[fiscalYearMonthCellName]?.Text;

                cellsBS[balanceSheetDateCellName].Value = companyPdfMetaData.EndPeriodOfReport.Date;
                cellsBS[fiscalYearMonthCellName].Value = companyPdfMetaData.EndPeriodOfReport.Year;


                foreach (var finRow in excelInputData.BsValues)
                {
                    string cellNameCurrentYear = $"D{finRow.RowNumber}";
                    string cellNamePrevoiusYear = $"G{finRow.RowNumber}";
                    cellsBS[cellNameCurrentYear].Value = finRow.CurrentYear;
                    cellsBS[cellNamePrevoiusYear].Value = finRow.PreviousYear;
                }

                ExcelRange cellsPl = package.Workbook.Worksheets[2].Cells;

                foreach (var finRow in excelInputData.PlValues)
                {
                    string cellNameCurrentYear = $"D{finRow.RowNumber}";
                    string cellNamePrevoiusYear = $"G{finRow.RowNumber}";
                    cellsPl[cellNameCurrentYear].Value = finRow.CurrentYear;
                    cellsPl[cellNamePrevoiusYear].Value = finRow.PreviousYear;
                }

                ExcelRange cellsPL = package.Workbook.Worksheets[2].Cells;

                package.SaveAs(fileInfoOutput);

                return fileInfoOutput.FullName;
            }
        }

        public static ParsedPdfResult ParseHungarianPdf(string pdfFileFullPhysicalPath)
        {
            using (Stream stream = File.OpenRead(pdfFileFullPhysicalPath))
            {
                // Load the input file.
                PdfFixedDocument document = new PdfFixedDocument(stream);

                StringBuilder sb = new StringBuilder();

                ParsedPdfResult parsedPdfResult = new ParsedPdfResult();

                parsedPdfResult.BsRows.AddRange(Mappings.GetFreshList(Mappings.HungarianBsRows));
                parsedPdfResult.PlRows.AddRange(Mappings.GetFreshList(Mappings.HungarianPlRows));

                Dictionary<string, bool> dictAddedInBs = new Dictionary<string, bool>();

                List<string> allStringFragments = new List<string>();

                foreach (var page in document.Pages)
                {
                    PdfContentExtractor ce = new PdfContentExtractor(page);
                    PdfTextFragmentCollection tfc = ce.ExtractTextFragments();

                    for (int i = 0; i < tfc.Count; i++)
                    {
                        allStringFragments.Add(tfc[i].Text);
                    }
                }

                List<string> allStringFragmentsToCount = new List<string>();

                var bsRows = parsedPdfResult.BsRows;
                var plRows = parsedPdfResult.PlRows;

                foreach (var page in document.Pages)
                {
                    PdfContentExtractor ce = new PdfContentExtractor(page);
                    PdfTextFragmentCollection tfc = ce.ExtractTextFragments();

                    for (int i = 0; i < tfc.Count; i++)
                    {

                        var text = tfc[i].Text;

                        if (text == "006.")
                        {

                        }

                        sb.AppendLine(text);

                        foreach (var entry in parsedPdfResult.PlRows)
                        {
                            if (text.Equals(entry.Number + "."))
                            {
                                if (allStringFragmentsToCount.Contains(text))
                                {
                                    var keyBS = entry.Number;

                                    var intToAdd = GetCorrectValueFromHungarianPdfRow(i, tfc, entry.Number);

                                    entry.CurrentYear = intToAdd.CurrentYear;

                                    if (!parsedPdfResult.DictWithValuesPL.Any(k => k.Number == keyBS))
                                    {
                                        ParsedPdfRow parsedPdfRow = new ParsedPdfRow();
                                        parsedPdfRow.Number = keyBS;
                                        parsedPdfRow.CurrentYear = intToAdd.CurrentYear;
                                        parsedPdfRow.PreviousYear = intToAdd.PreviousYear;
                                        parsedPdfResult.DictWithValuesPL.Add(parsedPdfRow);
                                    }
                                }
                            }
                        }


                        foreach (var entry in parsedPdfResult.BsRows)
                        {
                            if (text.Equals(entry.Number + "."))
                            {
                                var keyBS = entry.Number;

                                var intToAdd = GetCorrectValueFromHungarianPdfRow(i, tfc, entry.Number);

                                entry.CurrentYear = intToAdd.CurrentYear;

                                if (!parsedPdfResult.DictWithValuesBS.Any(k => k.Number == keyBS))
                                {
                                    ParsedPdfRow parsedPdfRow = new ParsedPdfRow();
                                    parsedPdfRow.Number = keyBS;
                                    parsedPdfRow.CurrentYear = intToAdd.CurrentYear;
                                    parsedPdfRow.PreviousYear = intToAdd.PreviousYear;
                                    parsedPdfResult.DictWithValuesBS.Add(parsedPdfRow);
                                }
                            }
                        }

                        allStringFragmentsToCount.Add(text);

                    }

                }

                var textFromPdf = sb.ToString();

                // Това трябва да го има
                foreach (var bsRow in parsedPdfResult.BsRows)
                {
                    foreach (var item in parsedPdfResult.DictWithValuesBS)
                    {
                        if (bsRow.Number.Equals(item.Number, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bsRow.CurrentYear = item.CurrentYear;
                            bsRow.PreviousYear = item.PreviousYear;
                        }
                    }
                }

                foreach (var plRow in parsedPdfResult.PlRows)
                {
                    foreach (var item in parsedPdfResult.DictWithValuesPL)
                    {
                        if (plRow.Number.Equals(item.Number, StringComparison.InvariantCultureIgnoreCase))
                        {
                            plRow.CurrentYear = item.CurrentYear;
                            plRow.PreviousYear = item.PreviousYear;
                        }
                    }
                }

                return parsedPdfResult;
            }
        }

        public static CountryFileTypes GetCountryFileTypesFromPdfFile(string pdfFileFullPhysicalPath)
        {
            using (Stream stream = File.OpenRead(pdfFileFullPhysicalPath))
            {
                // Load the input file.
                PdfFixedDocument document = new PdfFixedDocument(stream);

                StringBuilder sb = new StringBuilder();

                PdfContentExtractor ce = new PdfContentExtractor(document.Pages.FirstOrDefault());
                PdfTextFragmentCollection tfc = ce.ExtractTextFragments();

                for (int i = 0; i < tfc.Count; i++)
                {
                    sb.AppendLine(tfc[i].Text);
                }

                string allText = sb.ToString().ToUpperInvariant();

                CountryFileTypes result = CountryFileTypes.Undefined;

                if (allText.Contains("Попуњава правно лице".ToUpperInvariant()))
                {
                    result = CountryFileTypes.Serbian;
                }
                else if (allText.Contains("Nyilvántartási szám".ToUpperInvariant()))
                {
                    result = CountryFileTypes.Hungarian;
                }

                return result;
            }
        }

        public static ParsedPdfResult ParseCroatiaPdf(string pdfFileFullPhysicalPath)
        {
            using (Stream stream = File.OpenRead(pdfFileFullPhysicalPath))
            {
                PdfFixedDocument document = new PdfFixedDocument(stream);

                StringBuilder sb = new StringBuilder();

                StringBuilder sbFirstPage = new StringBuilder();
                PdfContentExtractor ceFirstPage = new PdfContentExtractor(document.Pages.FirstOrDefault());
                PdfTextFragmentCollection tfcFirstPage = ceFirstPage.ExtractTextFragments();
                for (int i = 0; i < tfcFirstPage.Count; i++)
                {
                    sbFirstPage.AppendLine(tfcFirstPage[i].Text);
                }
                string firstPageText = sbFirstPage.ToString();

                ParsedPdfResult parsedPdfResult = new ParsedPdfResult();

                parsedPdfResult.BsRows.AddRange(Mappings.GetFreshList(Mappings.CroatiaBsRows));
                parsedPdfResult.PlRows.AddRange(Mappings.GetFreshList(Mappings.CroatiaPlRows));

                Dictionary<string, bool> dictAddedInBs = new Dictionary<string, bool>();

                List<string> allStringFragments = new List<string>();

                foreach (var page in document.Pages)
                {
                    PdfContentExtractor ce = new PdfContentExtractor(page);
                    PdfTextFragmentCollection tfc = ce.ExtractTextFragments();

                    for (int i = 0; i < tfc.Count; i++)
                    {
                        allStringFragments.Add(tfc[i].Text);
                    }
                }

                return null;

            }
        }

        public static ParsedPdfResult ParseSerbianPdf(string pdfFileFullPhysicalPath)
        {
            using (Stream stream = File.OpenRead(pdfFileFullPhysicalPath))
            {
                PdfFixedDocument document = new PdfFixedDocument(stream);

                StringBuilder sb = new StringBuilder();

                StringBuilder sbFirstPage = new StringBuilder();
                PdfContentExtractor ceFirstPage = new PdfContentExtractor(document.Pages.FirstOrDefault());
                PdfTextFragmentCollection tfcFirstPage = ceFirstPage.ExtractTextFragments();
                for (int i = 0; i < tfcFirstPage.Count; i++)
                {
                    sbFirstPage.AppendLine(tfcFirstPage[i].Text);
                }
                string firstPageText = sbFirstPage.ToString();

                ParsedPdfResult parsedPdfResult = new ParsedPdfResult();

                parsedPdfResult.BsRows.AddRange(Mappings.GetFreshList(Mappings.SerbianBsRows));
                parsedPdfResult.PlRows.AddRange(Mappings.GetFreshList(Mappings.SerbianPlRows));

                Dictionary<string, bool> dictAddedInBs = new Dictionary<string, bool>();

                List<string> allStringFragments = new List<string>();



                foreach (var page in document.Pages)
                {
                    PdfContentExtractor ce = new PdfContentExtractor(page);
                    PdfTextFragmentCollection tfc = ce.ExtractTextFragments();

                    for (int i = 0; i < tfc.Count; i++)
                    {
                        allStringFragments.Add(tfc[i].Text);
                    }
                }

                List<string> allStringFragmentsToCount = new List<string>();

                var bsRows = parsedPdfResult.BsRows;
                var plRows = parsedPdfResult.PlRows;

                foreach (var page in document.Pages)
                {
                    PdfContentExtractor ce = new PdfContentExtractor(page);
                    PdfTextFragmentCollection tfc = ce.ExtractTextFragments();

                    for (int i = 0; i < tfc.Count; i++)
                    {

                        var text = tfc[i].Text;

                        if (text == "0008")
                        {

                        }

                        sb.AppendLine(text);
                        if (firstPageText.ToUpperInvariant().Contains("БИЛАНС УСПЕХА".ToUpperInvariant())) // Profit and loss
                        {
                            foreach (var entry in parsedPdfResult.PlRows)
                            {
                                if (text.Equals(entry.Number))
                                {
                                    var keyBS = entry.Number;

                                    var intToAdd = GetCorrectValueFromSerbianPdfRow(i, tfc, entry.Number, false);

                                    entry.CurrentYear = intToAdd.CurrentYear;

                                    if (!parsedPdfResult.DictWithValuesPL.Any(k => k.Number == keyBS))
                                    {
                                        ParsedPdfRow parsedPdfRow = new ParsedPdfRow();
                                        parsedPdfRow.Number = keyBS;
                                        parsedPdfRow.CurrentYear = intToAdd.CurrentYear;
                                        parsedPdfRow.PreviousYear = intToAdd.PreviousYear;
                                        parsedPdfResult.DictWithValuesPL.Add(parsedPdfRow);
                                    }
                                }
                            }
                        }

                        if (firstPageText.ToUpperInvariant().Contains("БИЛАНС СТАЊА".ToUpperInvariant())) // BalanceSheet
                        {
                            foreach (var entry in parsedPdfResult.BsRows)
                            {
                                if (text.Equals(entry.Number))
                                {
                                    var keyBS = entry.Number;

                                    var intToAdd = GetCorrectValueFromSerbianPdfRow(i, tfc, entry.Number, true);

                                    entry.CurrentYear = intToAdd.CurrentYear;

                                    if (!parsedPdfResult.DictWithValuesBS.Any(k => k.Number == keyBS))
                                    {
                                        ParsedPdfRow parsedPdfRow = new ParsedPdfRow();
                                        parsedPdfRow.Number = keyBS;
                                        parsedPdfRow.CurrentYear = intToAdd.CurrentYear;
                                        parsedPdfRow.PreviousYear = intToAdd.PreviousYear;
                                        parsedPdfResult.DictWithValuesBS.Add(parsedPdfRow);
                                    }
                                }
                            }

                            allStringFragmentsToCount.Add(text);
                        }
                    }

                }

                var textFromPdf = sb.ToString();

                // Това трябва да го има
                foreach (var bsRow in parsedPdfResult.BsRows)
                {
                    foreach (var item in parsedPdfResult.DictWithValuesBS)
                    {
                        if (bsRow.Number.Equals(item.Number, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bsRow.CurrentYear = item.CurrentYear;
                            bsRow.PreviousYear = item.PreviousYear;
                        }
                    }
                }

                foreach (var plRow in parsedPdfResult.PlRows)
                {
                    foreach (var item in parsedPdfResult.DictWithValuesPL)
                    {
                        if (plRow.Number.Equals(item.Number, StringComparison.InvariantCultureIgnoreCase))
                        {
                            plRow.CurrentYear = item.CurrentYear;
                            plRow.PreviousYear = item.PreviousYear;
                        }
                    }
                }

                return parsedPdfResult;
            }
        }

        public static CompanyPdfMetaData GetCompanyPdfMetaData(string pdfFileFullPhysicalPath)
        {
            CountryFileTypes countryFileType = Utils.GetCountryFileTypesFromPdfFile(pdfFileFullPhysicalPath);

            using (Stream stream = File.OpenRead(pdfFileFullPhysicalPath))
            {
                CompanyPdfMetaData result = new CompanyPdfMetaData();
                PdfFixedDocument document = new PdfFixedDocument(stream);

                StringBuilder sb = new StringBuilder();

                ParsedPdfResult parsedPdfResult = new ParsedPdfResult();

                Dictionary<string, bool> dictAddedInBs = new Dictionary<string, bool>();

                StringBuilder companyStringFragments = new StringBuilder();
                PdfPage firstPage = document.Pages.FirstOrDefault();

                PdfContentExtractor ce = new PdfContentExtractor(firstPage);
                PdfTextFragmentCollection tfc = ce.ExtractTextFragments();

                switch (countryFileType)
                {
                    case CountryFileTypes.Hungarian:
                        for (int i = 0; i < tfc.Count; i++)
                        {
                            var text = tfc[i].Text;
                            if (text.ToUpperInvariant().Contains("(Nyilvántartási szám:".ToUpperInvariant()))
                            {
                                break;
                            }
                            if (i != 0)
                            {
                                companyStringFragments.Append(" ");
                            }
                            companyStringFragments.Append(text);

                        }
                        string companyName = companyStringFragments.ToString();
                        result.CompanyName = companyName;

                        for (int i = 0; i < tfc.Count; i++)
                        {
                            var text = tfc[i].Text;
                            if (text.ToUpperInvariant().Contains("(Nyilvántartási szám:".ToUpperInvariant()))
                            {
                                int firstIndexOfColon = text.IndexOf(":");
                                int firstIndexOfComma = text.IndexOf(",");

                                string rowRegistrationNumber = text.Substring(firstIndexOfColon + 1, firstIndexOfComma - firstIndexOfColon - 1);
                                string registrationNumber = rowRegistrationNumber.Replace(" ", string.Empty);
                                result.CompanyRegistrationNumber = registrationNumber;

                                int lastIndexOfColon = text.LastIndexOf(":");
                                int firstIndexOfBracket = text.LastIndexOf(")");

                                string rowTaxNumber = text.Substring(lastIndexOfColon + 1, firstIndexOfBracket - lastIndexOfColon - 1);
                                string taxNumber = rowTaxNumber.Replace(" ", string.Empty);
                                result.CompanyTaxNumber = taxNumber;
                                break;
                            }
                        }

                        for (int i = 0; i < tfc.Count; i++)
                        {
                            var text = tfc[i].Text;
                            if (text.ToUpperInvariant().Contains("időszakra vonatkozó".ToUpperInvariant()))
                            {
                                string rowPeroidText = tfc[i - 1]?.Text;

                                string firstHalfRow = rowPeroidText.Split('-')[0].Trim();
                                string secondHalfRaw = rowPeroidText.Split('-')[1].Trim();

                                var hungarianCulture = new CultureInfo("hu-HU");

                                DateTime.TryParse(firstHalfRow, hungarianCulture, DateTimeStyles.None, out DateTime startDate);
                                DateTime.TryParse(secondHalfRaw, hungarianCulture, DateTimeStyles.None, out DateTime endDate);

                                result.StartPeriodOfReport = startDate;
                                result.EndPeriodOfReport = endDate;
                                break;

                            }
                        }




                        break;
                    case CountryFileTypes.Serbian:
                        for (int i = 0; i < tfc.Count; i++)
                        {
                            //Матични број
                            var text = tfc[i].Text;
                            if (text.ToUpperInvariant().Contains("Матични број".ToUpperInvariant()))
                            {
                                result.CompanyRegistrationNumber = tfc[i + 1]?.Text;
                                continue;
                            }
                            if (text.ToUpperInvariant().Contains("Шифра делатности".ToUpperInvariant()))
                            {
                                result.ActivityCode = tfc[i + 1]?.Text;
                                continue;
                            }
                            if (text.ToUpperInvariant().Contains("ПИБ".ToUpperInvariant()))
                            {
                                result.CompanyTaxNumber = tfc[i + 1]?.Text;
                                continue;
                            }
                            if (text.ToUpperInvariant().Contains("Назив".ToUpperInvariant()))
                            {
                                result.CompanyName = tfc[i + 1]?.Text;
                                continue;
                            }
                            if (text.ToUpperInvariant().Contains("Седиште".ToUpperInvariant()))
                            {
                                result.HeadOfficeAddress = tfc[i + 1]?.Text;
                                continue;
                            }
                            if (text.ToUpperInvariant().Contains("године".ToUpperInvariant())) // Balance sheet
                            {
                                string rowDate = tfc[i]?.Text;

                                string previousText = tfc[i - 1]?.Text;

                                if (!previousText.ToUpperInvariant().Contains("за период од".ToUpperInvariant())) // Balance sheet
                                {
                                    string stringDate = tfc[i]?.Text?.Replace("на дан", "").Replace("године", "").Trim();
                                    var serbianCulture = new CultureInfo("sr-Cyrl");
                                    DateTime.TryParse(stringDate, serbianCulture, DateTimeStyles.None, out DateTime endDate);
                                    DateTime startDate = new DateTime(endDate.Year, 1, 1);

                                    result.StartPeriodOfReport = startDate;
                                    result.EndPeriodOfReport = endDate;

                                    var all = CultureInfo.GetCultures(CultureTypes.AllCultures);
                                }
                                else // Profit and loss
                                {
                                    string[] parts = text?.Replace("на дан", "").Replace("године", "").Trim().Split("до");
                                    var serbianCulture = new CultureInfo("sr-Cyrl");

                                    DateTime.TryParse(parts[0].Trim(), serbianCulture, DateTimeStyles.None, out DateTime startDate);
                                    DateTime.TryParse(parts[1].Trim(), serbianCulture, DateTimeStyles.None, out DateTime endDate);

                                    result.StartPeriodOfReport = startDate;
                                    result.EndPeriodOfReport = endDate;
                                    continue;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }



                return result;
            }
        }

        private static ParsedPdfRow GetCorrectValueFromSerbianPdfRow(int numberInCollection,
            PdfTextFragmentCollection tfc, string currentNumberString, bool isBalanceSheet)
        {
            //if (numberInCollection + 3 < tfc.Count)


            //((Xfinium.Pdf.Graphics.PdfRgbColor)(new System.Collections.Generic.Mscorlib_CollectionDebugView<Xfinium.Pdf.Content.PdfTextFragment>(tfc).Items[101]).Brush.Color).B
            //string noteNumberString = tfc[numberInCollection + 1]?.Text;
            //int.TryParse(noteNumberString, out int noteNumberInt);

            string currentYear = string.Empty;
            string previousYear = string.Empty;

            PdfTextFragment rowNumberFr = tfc[numberInCollection];

            PdfTextFragment first = null;
            if (numberInCollection + 1 < tfc.Count)
            {
                first = tfc[numberInCollection + 1];
            }
            PdfTextFragment second = null;
            if (numberInCollection + 2 < tfc.Count)
            {
                second = tfc[numberInCollection + 2];
            }
            PdfTextFragment third = null;
            if (numberInCollection + 3 < tfc.Count)
            {
                third = tfc[numberInCollection + 3];
            }

            List<PdfTextFragment> fragments = new List<PdfTextFragment>()
                {
                    first,second,third
                };

            foreach (var fr in fragments)
            {
                if (fr != null)
                {
                    if (Math.Abs(fr.FragmentCorners[1].Y - rowNumberFr.FragmentCorners[1].Y) < 10) // Проверка дали са на същия ред
                    {
                        // current year
                        if (isBalanceSheet)
                        {
                            if (Math.Abs(fr.FragmentCorners[1].X - 401) < 20)
                            {
                                currentYear = fr?.Text;
                            }
                            else if (Math.Abs(fr.FragmentCorners[1].X - 482) < 20)
                            {
                                previousYear = fr?.Text;
                            }
                        }
                        else
                        {
                            if (Math.Abs(fr.FragmentCorners[1].X - 465.69) < 20)
                            {
                                currentYear = fr?.Text;
                            }
                            else if (Math.Abs(fr.FragmentCorners[1].X - 563.94) < 20)
                            {
                                previousYear = fr?.Text;
                            }
                        }
                    }
                }
            }

            // използваме координати по X за да намерим колоната и правим проверка по y за да сме сигурни че реда е същия




            // само ако е 84 е правилна кутия
            //if (first.Brush.Color.ToRgbColor().B == 84)
            //{
            //    int.TryParse(first.Text, out int noteNumberInt);


            //    if (0 < noteNumberInt && noteNumberInt < 32)
            //    {
            //        // значи има note number
            //        if (second.Brush.Color.ToRgbColor().B == 84)
            //        {
            //            currentYear = second?.Text;
            //        }
            //        if (third.Brush.Color.ToRgbColor().B == 84)
            //        {
            //            previousYear = third?.Text;
            //        }
            //    }
            //    else
            //    {
            //        if (first.Brush.Color.ToRgbColor().B == 84)
            //        {
            //            currentYear = first?.Text;
            //        }
            //        if (second.Brush.Color.ToRgbColor().B == 84)
            //        {
            //            previousYear = second?.Text;
            //        }
            //    }
            //}

            // Да се пита Димо noteNumberInt на сръбски дали е в интервала [1-31]


            int.TryParse(currentYear, out int currentYearInt);
            int.TryParse(previousYear, out int previousYearInt);

            var result = new ParsedPdfRow();
            result.Number = currentNumberString;
            result.CurrentYear = currentYearInt;
            result.PreviousYear = previousYearInt;
            return result;


        }

        private static ParsedPdfRow GetCorrectValueFromHungarianPdfRow(int numberInCollection, PdfTextFragmentCollection tfc, string currentNumberString)
        {
            ParsedPdfRow parsedPdfRow = new ParsedPdfRow();
            parsedPdfRow.Number = currentNumberString;
            // Проверяваме следващите 10 записа за втория целочислен запис

            string nextNumberString = "";
            if (int.TryParse(currentNumberString, out int currentNumberInt))
            {
                nextNumberString = (currentNumberInt + 1).ToString("D3");
            }
            else
            {

            }


            int successfulParsedNumbers = 0;
            for (int i = 1; i < 10; i++)
            {
                if (numberInCollection + i < tfc.Count)
                {
                    var currentFragment = tfc[numberInCollection + i];

                    var text = currentFragment.Text?.Replace(" ", "");

                    if (int.TryParse(text, out int dummy))
                    {
                        successfulParsedNumbers++;
                        if (successfulParsedNumbers == 1)
                        {
                            parsedPdfRow.PreviousYear = dummy;
                        }
                    }
                    if (successfulParsedNumbers == 2)
                    {
                        parsedPdfRow.CurrentYear = dummy;
                        //return dummy;
                        break;
                    }
                    if (!string.IsNullOrEmpty(nextNumberString))
                    {
                        if (text.ToUpperInvariant().Trim().Equals((nextNumberString + ".").ToUpperInvariant().Trim(),
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            //return 0;
                            break;
                        }
                    }
                }
            }

            return parsedPdfRow;
        }


    }
}
