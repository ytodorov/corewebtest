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
        public static void PopulateMappingDictionaries(IHostingEnvironment env)
        {
            string dirPath = Path.Combine(env.WebRootPath, "Files", "I-O Distribution Key.xlsx");
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
                                    var goesTo = currentSheet.Cells[i, 3].Value.ToString();
                                    if (page == 1)
                                    {
                                        Mappings.BsDict.Add(inputValue, goesTo);
                                    }
                                    else if (page == 2)
                                    {
                                        Mappings.PlDict.Add(inputValue, goesTo);
                                    }
                                }
                            }
                        }
                    }



                }
            }
        }

        public static ParsedPdfResult ParsePdf(string physicalPath)
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
                        
                        sb.AppendLine(text);

                        foreach (var entry in Mappings.PlDict)
                        {
                            if (text.Equals(entry.Key + "."))
                            {
                                if (allStringFragmentsToCount.Contains(text))
                                {
                                    var keyBS = entry.Key;
                                    var valueBsString = tfc[i + 5]?.Text?.Replace(" ", "");

                                    if (int.TryParse(valueBsString, out int val))
                                    {
                                        if (!parsedPdfResult.DictWithValuesPL.ContainsKey(keyBS))
                                        {
                                            parsedPdfResult.DictWithValuesPL.Add(keyBS, val);
                                        }
                                    }
                                }
                            }
                        }


                        foreach (var entry in Mappings.BsDict)
                        {
                            if (text.Equals(entry.Key + "."))
                            {
                                var keyBS = entry.Key;
                                var valueBsString = tfc[i + 5]?.Text?.Replace(" ", "");

                                if (int.TryParse(valueBsString, out int val))
                                {
                                    if (!parsedPdfResult.DictWithValuesBS.ContainsKey(keyBS))
                                    {
                                        parsedPdfResult.DictWithValuesBS.Add(keyBS, val);
                                    }                                   
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

                return parsedPdfResult;
                // Do your work with the document inside the using statement.
            }
        }
    }
}
