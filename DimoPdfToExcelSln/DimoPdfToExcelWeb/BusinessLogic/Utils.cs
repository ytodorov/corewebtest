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
        public static void PopulateMappingDictionaries(string dirWithFiles)
        {

            string dirPath = Path.Combine(dirWithFiles, "Files", "I-O Distribution Key.xlsx");
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
                                        Mappings.BsDict.Add(inputValue, goesTo);
                                        fr.Type = "BS";
                                        Mappings.HungarianBsRows.Add(fr);
                                    }
                                    else if (page == 2)
                                    {
                                        Mappings.PlDict.Add(inputValue, goesTo);
                                        fr.Type = "PL";
                                        Mappings.HungarianPlRows.Add(fr);
                                    }
                                }
                            }
                        }
                    }



                }
            }

            var bsDict = Mappings.BsDict;
            var plDict = Mappings.PlDict;

            foreach (var item in Mappings.BsDict)
            {
                if (!Mappings.ExcelBsTitles.Contains(item.Value))
                {
                    Mappings.ExcelBsTitles.Add(item.Value);
                }
            }

            foreach (var item in Mappings.PlDict)
            {
                if (!Mappings.ExcelPlTitles.Contains(item.Value))
                {
                    Mappings.ExcelPlTitles.Add(item.Value);
                }
            }

            var excelBsTitles = Mappings.ExcelBsTitles;
            var excelPlTitles = Mappings.ExcelPlTitles;

            var bsRows = Mappings.HungarianBsRows;
            var plRows = Mappings.HungarianPlRows;


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

            //foreach (var title in Mappings.ExcelBsTitles)
            //{
            //    var sum = 0;
            //    foreach (var map in Mappings.BsDict)
            //    {
            //        if (map.Value.Equals(title, StringComparison.InvariantCultureIgnoreCase))
            //        {
            //            var val = parsedPdfResult.DictWithValuesBS[map.Key];
            //            sum += val;
            //        }
            //    }
            //    result.BsValues.Add(title, sum);
            //}

        
            //foreach (var title in Mappings.ExcelPlTitles)
            //{
            //    var sum = 0;
            //    foreach (var map in Mappings.PlDict)
            //    {
            //        if (map.Value.Equals(title, StringComparison.InvariantCultureIgnoreCase))
            //        {
            //            var val = parsedPdfResult.DictWithValuesPL[map.Key];
            //            sum += val;
            //        }
            //    }
            //    result.PlValues.Add(title, sum);
            //}

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

                foreach (var page in document.Pages)
                {
                    PdfContentExtractor ce = new PdfContentExtractor(page);
                    PdfTextFragmentCollection tfc = ce.ExtractTextFragments();



                    for (int i = 0; i < tfc.Count; i++)
                    {

                        var text = tfc[i].Text;

                        if (text == "081.")
                        {

                        }

                        sb.AppendLine(text);

                        var bsDict = Mappings.BsDict;
                        var plDict = Mappings.PlDict;

                        foreach (var entry in Mappings.PlDict)
                        {
                            if (text.Equals(entry.Key + "."))
                            {
                                if (allStringFragmentsToCount.Contains(text))
                                {
                                    var keyBS = entry.Key;
                                    int intToAdd = GetCorrectValueFromPdfRow(i, tfc);

                                    

                                    if (!parsedPdfResult.DictWithValuesPL.ContainsKey(keyBS))
                                    {
                                        parsedPdfResult.DictWithValuesPL.Add(keyBS, intToAdd);
                                    }
                                }
                            }
                        }


                        foreach (var entry in Mappings.BsDict)
                        {
                            if (text.Equals(entry.Key + "."))
                            {
                                var keyBS = entry.Key;
                                var intToAdd = GetCorrectValueFromPdfRow(i, tfc);

                                var finRow = Mappings.HungarianBsRows.FirstOrDefault(h => h.Number == entry.Key);
                                if (finRow != null)
                                {
                                    finRow.CurrentYear = intToAdd;
                                }

                                if (!parsedPdfResult.DictWithValuesBS.ContainsKey(keyBS))
                                {
                                    parsedPdfResult.DictWithValuesBS.Add(keyBS, intToAdd);
                                }
                            }
                        }

                        allStringFragmentsToCount.Add(text);

                    }

                }

                int totalBs = 0;
                foreach (var item in parsedPdfResult.DictWithValuesBS)
                {
                    totalBs += item.Value;
                }

                int totalPl = 0;
                foreach (var item in parsedPdfResult.DictWithValuesPL)
                {
                    totalPl += item.Value;
                }

                var textFromPdf = sb.ToString();

                foreach (var bsRow in Mappings.HungarianBsRows)
                {
                    foreach (var item in parsedPdfResult.DictWithValuesBS)
                    {
                        if (bsRow.Number.Equals(item.Key, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bsRow.CurrentYear = item.Value;
                        }
                    }
                }

                foreach (var plRow in Mappings.HungarianPlRows)
                {
                    foreach (var item in parsedPdfResult.DictWithValuesPL)
                    {
                        if (plRow.Number.Equals(item.Key, StringComparison.InvariantCultureIgnoreCase))
                        {
                            plRow.CurrentYear = item.Value;
                        }
                    }
                }

                var bsRows = Mappings.HungarianBsRows;
                var plRows = Mappings.HungarianPlRows;

                return parsedPdfResult;
                // Do your work with the document inside the using statement.
            }
        }

        private static int GetCorrectValueFromPdfRow(int numberInCollection, PdfTextFragmentCollection tfc)
        {
            // Проверяваме следващите 10 записа за втория целочислен запис

            int successfulParsedNumbers = 0;
            for (int i = 1; i < 10; i++)
            {
                var currentFragment = tfc[numberInCollection + i];

                var text = currentFragment.Text?.Replace(" ", "");

                if (int.TryParse(text, out int dummy))
                {
                    successfulParsedNumbers++;
                }
                if (successfulParsedNumbers == 2)
                {
                    return dummy;
                }
            }

            return 0;
        }


    }
}
