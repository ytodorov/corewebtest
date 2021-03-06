﻿using Microsoft.AspNetCore.Hosting;
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
using DimoPdfToExcelWeb.Extensions;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class Utils
    {
        //private static Object thisLock = new Object();
        public static void PopulateHungarianMappingDictionaries(string dirWithFiles)
        {
            //lock (thisLock)
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

                            for (int i = 2; i <= 113; i++)
                            {
                                // Проверка за бял цвят
                                //if (string.IsNullOrEmpty(currentSheet.Cells[i, 1].Style.Fill.BackgroundColor.Rgb))
                                {
                                    // Проверка за невалиден ред
                                    if (!string.IsNullOrWhiteSpace(currentSheet.Cells[i, 1].Value?.ToString()) &&
                                        !string.IsNullOrWhiteSpace(currentSheet.Cells[i, 3].Value?.ToString()))
                                    {
                                        var inputValue = currentSheet.Cells[i, 1].Value.ToString().Substring(0, 3);
                                        var hungName = currentSheet.Cells[i, 1].Value.ToString().Substring(5);
                                        var goesTo = currentSheet.Cells[i, 2].Value?.ToString();
                                        var goesToRowNumberString = currentSheet.Cells[i, 3]?.Value?.ToString()?.Trim()?.Replace(" ", string.Empty);
                                        var sign = currentSheet.Cells[i, 4]?.Value?.ToString();

                                        var alphaParent = currentSheet.Cells[i, 5]?.Value?.ToString()?.Trim();
                                        var romanParent = currentSheet.Cells[i, 6]?.Value?.ToString()?.Trim();

                                        FinancialRow fr = new FinancialRow();
                                        if (!string.IsNullOrEmpty(currentSheet.Cells[i, 1].Style.Fill.BackgroundColor.Rgb))
                                        {
                                            fr.IsSum = true;
                                        }
                                        fr.Number = inputValue;
                                        fr.Name = hungName;
                                        fr.GoesToRowTitle = goesTo;
                                        fr.AlphaParent = alphaParent;
                                        fr.RomanParent = romanParent;

                                        string[] rowNumbers = goesToRowNumberString.Split(',');
                                        List<int> rowNumbersList = new List<int>();

                                        foreach (var rowNum in rowNumbers)
                                        {
                                            if (int.TryParse(rowNum, out int goesToRowNumberInt))
                                            {
                                                fr.GoesToRowNumber.Add(goesToRowNumberInt);
                                            }
                                        }


                                        if (!string.IsNullOrEmpty(sign) && sign.Trim().Equals("-", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            fr.Sign = "-";
                                        }

                                        if (page == 1)
                                        {
                                            fr.Type = "BS";
                                            if (!Mappings.HungarianBsRows.Any(f => f.Name == fr.Name))
                                            {
                                                Mappings.HungarianBsRows.Add(fr);
                                            }
                                        }
                                        else if (page == 2)
                                        {
                                            fr.Type = "PL";
                                            if (!Mappings.HungarianPlRows.Any(f => f.Name == fr.Name))
                                            {
                                                Mappings.HungarianPlRows.Add(fr);
                                            }
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

                                    var sign = currentSheet.Cells[i, 4]?.Value?.ToString();

                                    FinancialRow fr = new FinancialRow();
                                    fr.Number = inputValue;
                                    fr.Name = name;
                                    if (!string.IsNullOrEmpty(sign) && sign.Trim().Equals("-", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        fr.Sign = "-";
                                    }

                                    string[] rowNumbers = goesToRowNumberString.Split(',');
                                    List<int> rowNumbersList = new List<int>();

                                    foreach (var rowNum in rowNumbers)
                                    {
                                        if (int.TryParse(rowNum, out int goesToRowNumberInt))
                                        {
                                            fr.GoesToRowNumber.Add(goesToRowNumberInt);
                                        }
                                    }

                                    if (fr.GoesToRowNumber.Count == 0)
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

                                    string[] rowNumbers = goesToRowNumberString.Split(',');
                                    List<int> rowNumbersList = new List<int>();

                                    foreach (var rowNum in rowNumbers)
                                    {
                                        if (int.TryParse(rowNum, out int goesToRowNumberInt))
                                        {
                                            fr.GoesToRowNumber.Add(goesToRowNumberInt);
                                        }
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

            //var bsGroup = bsRows.GroupBy(h => h.GoesToRowNumber);

            for (int rowNumber = 0; rowNumber < 110; rowNumber++)
            {
                var bsGroup = bsRows.Where(b => b.GoesToRowNumber.Contains(rowNumber)).ToList();
                if (bsGroup.Count == 0)
                {
                    continue;
                }

                //foreach (var group in bsGroup)
                {

                    //var sumCurrentYear = (int)group.Sum(g => g.CurrentYear);
                    int sumCurrentYear = 0;
                    foreach (var item in bsGroup)
                    {
                        if (item.Sign.Equals("-", StringComparison.InvariantCultureIgnoreCase))
                        {
                            sumCurrentYear -= (int)item.CurrentYear;
                        }
                        else
                        {
                            sumCurrentYear += (int)item.CurrentYear;
                        }
                    }
                    //var sumPreviousYear = (int)group.Sum(g => g.PreviousYear);
                    int sumPreviousYear = 0;
                    foreach (var item in bsGroup)
                    {
                        if (item.Sign.Equals("-", StringComparison.InvariantCultureIgnoreCase))
                        {
                            sumPreviousYear -= (int)item.PreviousYear;
                        }
                        else
                        {
                            sumPreviousYear += (int)item.PreviousYear;
                        }
                    }
                    ExcellOutputRowData excellOutputRowData = new ExcellOutputRowData()
                    {
                        RowNumber = rowNumber,//group.Key,
                        CurrentYear = sumCurrentYear,
                        PreviousYear = sumPreviousYear
                    };

                    result.BsValues.Add(excellOutputRowData);

                    //var sumPreviousYear = (int)group.Sum(g => g.PreviousYear);
                    //result.BsValues.Add(group.Key, sumCurrentYear);
                }
            }

            //var plGroup = plRows.GroupBy(h => h.GoesToRowNumber);
            for (int rowNumber = 0; rowNumber < 110; rowNumber++)
            {
                var plGroup = plRows.Where(b => b.GoesToRowNumber.Contains(rowNumber)).ToList();
                if (plGroup.Count == 0)
                {
                    continue;
                }
                //foreach (var group in plGroup)
                {
                    //var sumCurrentYear = (int)group.Sum(g => g.CurrentYear);
                    //var sumPreviousYear = (int)group.Sum(g => g.PreviousYear);
                    //var sumCurrentYear = (int)group.Sum(g => g.CurrentYear);
                    int sumCurrentYear = 0;
                    foreach (var item in plGroup)
                    {
                        if (item.Sign.Equals("-", StringComparison.InvariantCultureIgnoreCase))
                        {
                            sumCurrentYear -= (int)item.CurrentYear;
                        }
                        else
                        {
                            sumCurrentYear += (int)item.CurrentYear;
                        }
                    }
                    //var sumPreviousYear = (int)group.Sum(g => g.PreviousYear);
                    int sumPreviousYear = 0;
                    foreach (var item in plGroup)
                    {
                        if (item.Sign.Equals("-", StringComparison.InvariantCultureIgnoreCase))
                        {
                            sumPreviousYear -= (int)item.PreviousYear;
                        }
                        else
                        {
                            sumPreviousYear += (int)item.PreviousYear;
                        }
                    }
                    ExcellOutputRowData excellOutputRowData = new ExcellOutputRowData()
                    {
                        RowNumber = rowNumber,//group.Key,
                        CurrentYear = sumCurrentYear,
                        PreviousYear = sumPreviousYear
                    };
                    result.PlValues.Add(excellOutputRowData);
                }
            }
            return result;
        }

        public static string GetExcelOutputFilePath(string rootFolder, string pdfFilePath, string xlsmFilePath = null)
        {
            CountryFileTypes countryFileType = Utils.GetCountryFileTypesFromPdfFile(pdfFilePath);
            FileInfo fileEmptyOutput = null;
            if (string.IsNullOrEmpty(xlsmFilePath))
            {               
                fileEmptyOutput = new FileInfo(Path.Combine(rootFolder, "Files", "OUTPUT.xlsm"));
                if (!fileEmptyOutput.Exists)
                {
                    throw new ApplicationException("Няма го файла OUTPUT.xlsm в папка Files");
                }
            }
            else
            {
                fileEmptyOutput = new FileInfo(xlsmFilePath);
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
                    if (finRow.RowNumber > 0)
                    {
                        string cellNameCurrentYear = $"D{finRow.RowNumber}";
                        string cellNamePrevoiusYear = $"G{finRow.RowNumber}";
                        cellsBS[cellNameCurrentYear].Value = finRow.CurrentYear;
                        cellsBS[cellNamePrevoiusYear].Value = finRow.PreviousYear;
                    }
                }

                ExcelRange cellsPl = package.Workbook.Worksheets[2].Cells;

                foreach (var finRow in excelInputData.PlValues)
                {
                    if (finRow.RowNumber > 0)
                    {
                        string cellNameCurrentYear = $"D{finRow.RowNumber}";
                        string cellNamePrevoiusYear = $"G{finRow.RowNumber}";
                        cellsPl[cellNameCurrentYear].Value = finRow.CurrentYear;
                        cellsPl[cellNamePrevoiusYear].Value = finRow.PreviousYear;
                    }
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

                double currentYearX = 0;
                double previousYearX = 0;

                foreach (var page in document.Pages)
                {
                    PdfContentExtractor ce = new PdfContentExtractor(page);
                    PdfTextFragmentCollection tfc = ce.ExtractTextFragments();

                    for (int i = 0; i < tfc.Count; i++)
                    {
                        allStringFragments.Add(tfc[i].Text);

                        if (tfc[i].Text?.ToUpperInvariant().StartsWith("Előző üzleti év".ToUpperInvariant()) == true)
                        {
                            var colorOfB = tfc[i].Brush.Color.ToRgbColor().B;
                            if (colorOfB == 125 || colorOfB == 127)
                            {
                                previousYearX = tfc[i].FragmentCorners[1].X;
                            }
                        }
                        if (tfc[i].Text?.ToUpperInvariant().StartsWith("Tárgyévi adatok".ToUpperInvariant()) == true)
                        {
                            var colorOfB = tfc[i].Brush.Color.ToRgbColor().B;
                            if (colorOfB == 125 || colorOfB == 127)
                            {
                                currentYearX = tfc[i].FragmentCorners[1].X;
                            }
                        }
                    }

                    
                }

                List<string> allStringFragmentsToCount = new List<string>();

                var bsRows = parsedPdfResult.BsRows;
                var plRows = parsedPdfResult.PlRows;

                List<FinancialRow> allRows = new List<FinancialRow>();
                allRows.AddRange(parsedPdfResult.BsRows);
                allRows.AddRange(parsedPdfResult.PlRows);

                foreach (var page in document.Pages)
                {
                    PdfContentExtractor ce = new PdfContentExtractor(page);
                    PdfTextFragmentCollection tfc = ce.ExtractTextFragments();

                    var ExtractWords = ce.ExtractWords();
                    var ExtractVisualObjects = ce.ExtractVisualObjects(false);
                    var ExtractText = ce.ExtractText();
                    //var ExtractOptionalContentGroup = ce.ExtractOptionalContentGroup();
                    var resdfsd = ce.SearchText("Költségek, ráfordítások aktív időbeli elhatárolása");
                    var ExtractContentStreamOperators = ce.ExtractContentStreamOperators();

                    for (int i = 0; i < tfc.Count; i++)
                    {
                        var text = tfc[i].Text;

                        string fullRowText = text;

                        if (text.Contains("Befektetett pénzügyi eszközökből"))
                        {

                        }

                        if (text?.ExtractTextOnlyFromString2()?.Length > 3 && allRows.Any(r => r.Name.ExtractTextOnlyFromString2().Contains(text.ExtractTextOnlyFromString2())))
                        {
                            int counter = 0;
                            string rowName = string.Empty;
                            // Проверка за уникалност
                            bool foundInList = false;
                            foreach (var row in allRows)
                            {
                           


                                var entryToCheck = row.Name;
                                if (entryToCheck.Contains("."))
                                {
                                    entryToCheck = entryToCheck.Substring(row.Name.LastIndexOf("."));
                                }
                                if (entryToCheck.ExtractTextOnlyFromString2().StartsWith(text.ExtractTextOnlyFromString2()))
                                {
                                    counter++;
                                    rowName = entryToCheck;
                                }
                            }
                            if (counter == 1)
                            {
                                fullRowText = rowName;
                                foundInList = true;
                            }

                            // Тук сме само ако text е нещо което ни интересува
                            //if (fullRowText == text)
                            if (!foundInList)
                            {
                                //if (text.Contains("Befektetett pénzügyi eszközökből"))
                                {
                                    var currentX = tfc[i].FragmentCorners[0].X;

                                    for (int curr = 1; curr < 5; curr++)
                                    {
                                        if (i + curr < tfc.Count)
                                        {
                                            var next = tfc[i + curr];

                                            if (Math.Abs(next.FragmentCorners[0].X - currentX) < 20)
                                            {
                                                if (!string.IsNullOrWhiteSpace(next.Text.ExtractTextOnlyFromString2()))
                                                {
                                                    fullRowText += next.Text;

                                                    // TO DO Тук може да отива и на следващата страница ПРОБЛЕМ Hungarian1
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(text))
                        {
                            continue;
                        }

                        if (text.Contains("Költségek, ráfordítások aktív időbeli"))
                        {

                        }                                  

                        sb.AppendLine(text);
                        
                        
                  
                        foreach (var entry in allRows)
                        {
                            var entryToCheck = entry.Name;
                            if (entryToCheck.Contains("."))
                            {
                                entryToCheck = entryToCheck.Substring(entry.Name.LastIndexOf("."));
                            }

                           // Не трябва да се използва СтартсВитх              
                            if (entryToCheck.ExtractTextOnlyFromString2().Equals(fullRowText.ExtractTextOnlyFromString2(),
                                StringComparison.InvariantCultureIgnoreCase))


                            {
                                 
                                var keyBS = entry.Name;

                                var intToAdd = GetCorrectValueFromHungarianPdfRow(i, tfc, currentYearX, previousYearX);
                                
                                entry.CurrentYear = intToAdd.CurrentYear;
                                entry.PreviousYear = intToAdd.PreviousYear;                                
                            }
                        }

                        allStringFragmentsToCount.Add(text);

                    }

                }

                var textFromPdf = sb.ToString();

                parsedPdfResult.BsRows = parsedPdfResult.BsRows.Where(r => r.CurrentYear != 0 || r.PreviousYear != 0).ToList();

                parsedPdfResult.AllBsRows.AddRange(parsedPdfResult.BsRows);
                parsedPdfResult.AllPlRows.AddRange(parsedPdfResult.PlRows);

                List<FinancialRow> itemsToRemoveBS = new List<FinancialRow>();

               
                for (int i = 0; i < parsedPdfResult.BsRows.Count; i++)
                {
                    var current = parsedPdfResult.BsRows[i];
                    FinancialRow next = null;
                    if (i < parsedPdfResult.BsRows.Count - 1)
                    {
                        next = parsedPdfResult.BsRows[i + 1];
                    }
                    int indexOfDot = current.Name.IndexOf(".");
                    if (indexOfDot != -1)
                    {
                        var romanNumber = current.Name.Substring(0, indexOfDot).Trim();
                        var romanName = current.Name.Substring(indexOfDot + 1).Trim();
                        if (Constants.RomanLetters.Contains(romanNumber))
                        {
                            if (next?.RomanParent?.ToUpperInvariant()?.Contains(romanName?.ToUpperInvariant()) == true)
                            {
                                itemsToRemoveBS.Add(current);
                                continue;
                            }
                        }

                        var alphaLetter = current.Name.Substring(0, indexOfDot).Trim();
                        var alphaName = current.Name.Substring(indexOfDot + 1).Trim();
                        if (Constants.AlphabetLetters.Contains(alphaLetter))
                        {
                            if (next?.AlphaParent?.ToUpperInvariant()?.Contains(alphaName?.ToUpperInvariant()) == true)
                            {
                                itemsToRemoveBS.Add(current);
                                continue;
                            }
                        }
                    }
                }
                
                foreach (var item in itemsToRemoveBS)
                {
                    var realItem = parsedPdfResult.BsRows.FirstOrDefault(f => f.Name == item.Name);
                    parsedPdfResult.BsRows.Remove(realItem);
                }

                parsedPdfResult.PlRows = parsedPdfResult.PlRows.Where(r => r.CurrentYear != 0 || r.PreviousYear != 0).ToList();
                List<FinancialRow> itemsToRemovePL = new List<FinancialRow>();

                for (int i = 0; i < parsedPdfResult.PlRows.Count; i++)
                {
                    if (i > 0)
                    {
                        var current = parsedPdfResult.PlRows[i];
                        var previous = parsedPdfResult.PlRows[i - 1];


                        int indexOfDot = current.Name.IndexOf(".");
                        if (indexOfDot != -1)
                        {
                            var romanNumber = current.Name.Substring(0, indexOfDot).Trim();
                            var romanName = current.Name.Substring(indexOfDot + 1).Trim();
                            if (Constants.RomanLetters.Contains(romanNumber))
                            {
                                if (previous?.RomanParent?.ToUpperInvariant()?.Contains(romanName?.ToUpperInvariant()) == true)
                                {
                                    itemsToRemovePL.Add(current);
                                    continue;
                                }
                            }

                            var alphaLetter = current.Name.Substring(0, indexOfDot).Trim();
                            var alphaName = current.Name.Substring(indexOfDot + 1).Trim();
                            if (Constants.AlphabetLetters.Contains(alphaLetter))
                            {
                                if (previous?.AlphaParent?.ToUpperInvariant()?.Contains(alphaName?.ToUpperInvariant()) == true)
                                {
                                    itemsToRemovePL.Add(current);
                                    continue;
                                }
                            }
                        }
                    }
                }

                foreach (var item in itemsToRemovePL)
                {
                    var realItem = parsedPdfResult.PlRows.FirstOrDefault(f => f.Name == item.Name);
                    parsedPdfResult.PlRows.Remove(realItem);
                    //parsedPdfResult.PlRows.Remove(item);
                }

                StringBuilder sbBalance = new StringBuilder();
                foreach (var item in parsedPdfResult.BsRows)
                {
                    sbBalance.AppendLine($"{item.Name} {item.CurrentYear} {item.PreviousYear}");
                }
                foreach (var item in parsedPdfResult.PlRows)
                {
                    sbBalance.AppendLine($"{item.Name} {item.CurrentYear} {item.PreviousYear}");
                }
                var test = sbBalance.ToString();



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

        public static ParsedPdfResult ParseSlovenianPdf(string pdfFileFullPhysicalPath)
        {
            using (Stream stream = File.OpenRead(pdfFileFullPhysicalPath))
            {
                PdfFixedDocument document = new PdfFixedDocument(stream);

                StringBuilder sb = new StringBuilder();

                StringBuilder sbFirstPage = new StringBuilder();
                PdfContentExtractor ceFirstPage = new PdfContentExtractor(document.Pages.FirstOrDefault());
                PdfTextFragmentCollection tfcFirstPage = ceFirstPage.ExtractTextFragments();

                var images = ceFirstPage.ExtractImages(true);

                var asdasd = ceFirstPage.ExtractVisualObjects(true, true);

                for (int i = 0; i < tfcFirstPage.Count; i++)
                {
                    sbFirstPage.AppendLine(tfcFirstPage[i].Text);
                }
                string firstPageText = sbFirstPage.ToString();

                ParsedPdfResult parsedPdfResult = new ParsedPdfResult();



                return parsedPdfResult;
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
                        //if (firstPageText.ToUpperInvariant().Contains("БИЛАНС УСПЕХА".ToUpperInvariant())) // Profit and loss
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

                        //if (firstPageText.ToUpperInvariant().Contains("БИЛАНС СТАЊА".ToUpperInvariant())) // BalanceSheet
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
                        if (bsRow.Number?.Equals(item.Number, StringComparison.InvariantCultureIgnoreCase) == true)
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
                        if (plRow.Number?.Equals(item.Number, StringComparison.InvariantCultureIgnoreCase) == true)
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
                                /*
                                int firstIndexOfColon = text.IndexOf(":");
                                int firstIndexOfComma = text.IndexOf(",");
                                //  гърми (Nyilvántartási szám: 01-09-562315, Adószám: 12183511-2-41)
                                string rowRegistrationNumber = text.Substring(firstIndexOfColon + 1, firstIndexOfComma - firstIndexOfColon - 1);
                                string registrationNumber = rowRegistrationNumber.Replace(" ", string.Empty);
                                result.CompanyRegistrationNumber = registrationNumber;

                                int lastIndexOfColon = text.LastIndexOf(":");
                                int firstIndexOfBracket = text.LastIndexOf(")");

                                string rowTaxNumber = text.Substring(lastIndexOfColon + 1, firstIndexOfBracket - lastIndexOfColon - 1);
                                string taxNumber = rowTaxNumber.Replace(" ", string.Empty);
                                result.CompanyTaxNumber = taxNumber;


                                */
                                result.CompanyRegistrationNumber = "1";
                                result.CompanyTaxNumber = "2";
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

        private static ParsedPdfRow GetCorrectValueFromHungarianPdfRow(int numberInCollection,
            PdfTextFragmentCollection tfc, double currentYearXCoordinate, double previousYearXCoordinate)
        {
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

            PdfTextFragment fourth = null;
            if (numberInCollection + 4 < tfc.Count)
            {
                fourth = tfc[numberInCollection + 4];
            }

            PdfTextFragment fifth = null;
            if (numberInCollection + 5 < tfc.Count)
            {
                fifth = tfc[numberInCollection + 5];
            }

            PdfTextFragment sixth = null;
            if (numberInCollection + 6 < tfc.Count)
            {
                sixth = tfc[numberInCollection + 6];
            }



            List<PdfTextFragment> fragments = new List<PdfTextFragment>()
                {
                    first,second,third,fourth,fifth,sixth
                };

            foreach (var fr in fragments)
            {
                if (fr != null)
                {
                    if (Math.Abs(fr.FragmentCorners[1].Y - rowNumberFr.FragmentCorners[1].Y) < 10) // Проверка дали са на същия ред
                    {
                        // current year
                        
                            if (Math.Abs(fr.FragmentCorners[1].X - currentYearXCoordinate) < 50)
                            {
                                currentYear = fr?.Text;
                            }
                            else if (Math.Abs(fr.FragmentCorners[1].X - previousYearXCoordinate) < 50)
                            {
                                previousYear = fr?.Text;
                            }
                      
                    }
                }
            }                  
           


            int.TryParse(currentYear.Replace(" ", string.Empty), out int currentYearInt);
            int.TryParse(previousYear.Replace(" ", string.Empty), out int previousYearInt);

            var result = new ParsedPdfRow();
            result.CurrentYear = currentYearInt;
            result.PreviousYear = previousYearInt;
            return result;
        }

        public static bool IsFinalExcelFileValid(string path, ParsedPdfResult parsedPdfResult = null)
        {
            FileInfo fi = new FileInfo(path);
            using (ExcelPackage package = new ExcelPackage(fi))
            {
                var bsWorkSheet = package.Workbook.Worksheets[1];

                bsWorkSheet.Cells["D92"].Calculate();
                var bsCheckCellValueCurrentYear = bsWorkSheet.Cells["D92"]?.Value?.ToString();
                bsWorkSheet.Cells["G92"].Calculate();
                var bsCheckCellValuePreviousYear = bsWorkSheet.Cells["G92"]?.Value?.ToString();

                var plWorkSheet = package.Workbook.Worksheets[2];
                plWorkSheet.Cells["D102"].Calculate();
                var plCheckCellValueCurrentYear = plWorkSheet.Cells["D102"]?.Value?.ToString();
                plWorkSheet.Cells["G102"].Calculate();
                var plCheckCellValuePreviousYear = plWorkSheet.Cells["G102"]?.Value?.ToString();

                var plTotal = parsedPdfResult.AllPlRows.FirstOrDefault(r => r.Name.Contains("Adózott eredmény"));



                if (bsCheckCellValueCurrentYear?.Equals("0", StringComparison.CurrentCultureIgnoreCase) != true)
                {
                    return false;
                }
                if (bsCheckCellValuePreviousYear?.Equals("0", StringComparison.CurrentCultureIgnoreCase) != true)
                {
                    return false;
                }

                if (plCheckCellValueCurrentYear.ToString().Trim() != ((int)plTotal.CurrentYear).ToString())
                {
                    return false;
                }
                if (plCheckCellValuePreviousYear.ToString().Trim() != ((int)plTotal.PreviousYear).ToString())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
