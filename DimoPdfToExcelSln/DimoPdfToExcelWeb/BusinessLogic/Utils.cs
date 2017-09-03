using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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

        public static ExcelInputData GetExcelValues(ParsedPdfResult parsedPdfResult)
        {
            // balance
            ExcelInputData result = new ExcelInputData();
            result.BsValues = new Dictionary<int, int>();
            result.PlValues = new Dictionary<int, int>();

            var bsGroup = Mappings.HungarianBsRows.GroupBy(h => h.GoesToRowNumber);

            foreach (var group in bsGroup)
            {
                var sum = (int)group.Sum(g => g.CurrentYear);
                result.BsValues.Add(group.Key, sum);
            }

            var plGroup = Mappings.HungarianPlRows.GroupBy(h => h.GoesToRowNumber);

            foreach (var group in plGroup)
            {
                var sum = (int)group.Sum(g => g.CurrentYear);
                result.PlValues.Add(group.Key, sum);
            }           
            return result;
        }

        public static ParsedPdfResult ParseHungarianPdf(string physicalPath)
        {
            using (Stream stream = File.OpenRead(physicalPath))
            {
                // Load the input file.
                PdfFixedDocument document = new PdfFixedDocument(stream);

                PdfRgbColor penColor = new PdfRgbColor();
                PdfPen pen = new PdfPen(penColor, 0.5);
                Random rnd = new Random();
                byte[] rgb = new byte[3];


                StringBuilder sb = new StringBuilder();

                ParsedPdfResult parsedPdfResult = new ParsedPdfResult();

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

                var bsRows = Mappings.HungarianBsRows;
                var plRows = Mappings.HungarianPlRows;

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

                        foreach (var entry in Mappings.HungarianPlRows)
                        {
                            if (text.Equals(entry.Number + "."))
                            {
                                if (allStringFragmentsToCount.Contains(text))
                                {
                                    var keyBS = entry.Number;

                                    var intToAdd = GetCorrectValueFromPdfRow(i, tfc, entry.Number);
                                                                    
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


                        foreach (var entry in Mappings.HungarianBsRows)
                        {
                            if (text.Equals(entry.Number + "."))
                            {
                                var keyBS = entry.Number;

                                var intToAdd = GetCorrectValueFromPdfRow(i, tfc, entry.Number);
                                                              
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
                foreach (var bsRow in Mappings.HungarianBsRows)
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

                foreach (var plRow in Mappings.HungarianPlRows)
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

        private static ParsedPdfRow GetCorrectValueFromPdfRow(int numberInCollection, PdfTextFragmentCollection tfc, string currentNumberString)
        {
            ParsedPdfRow parsedPdfRow = new ParsedPdfRow();
            parsedPdfRow.Number = currentNumberString;
            // Проверяваме следващите 10 записа за втория целочислен запис

            string nextNumberString = "";
            if(int.TryParse(currentNumberString, out int currentNumberInt))
            {
                nextNumberString = (currentNumberInt + 1).ToString("D3");
            }
            else
            {

            }


            int successfulParsedNumbers = 0;
            for (int i = 1; i < 10; i++)
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

            return parsedPdfRow;
        }


    }
}
