using DimoPdfToExcelWeb.BusinessLogic;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class UtilsTests
    {
        [Fact]
        public void ParseHungarianPdfTest()
        {
            var rootSolution = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var path = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot", "Files", "Hungarian1.pdf");

            var wwwRootFolder = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot");
            Utils.PopulateHungarianMappingDictionaries(wwwRootFolder);
            var result = Utils.ParseHungarianPdf(path);

            var outputPath = Utils.GetExcelOutputFilePath(wwwRootFolder, path, CountryFileTypes.Hungarian);
        }

        [Fact]
        public void ParseSerbianPdfTest()
        {
            var rootSolution = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var path = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot", "Files", "SerbianBalanceSheet1.pdf");

            var wwwRootFolder = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot");
            Utils.PopulateSerbianMappingDictionaries(wwwRootFolder);
            var result = Utils.ParseSerbianPdf(path);

            var outputPath = Utils.GetExcelOutputFilePath(wwwRootFolder, path, CountryFileTypes.Serbian);
        }

        [Fact]
        public void GetCompanyPdfMetaDataHungarianTest()
        {
            List<string> hungarianFileNames = new List<string>()
            {
                "Hungarian1", "Hungarian2"
            };

            foreach (var hungarianFileName in hungarianFileNames)
            {
                var rootSolution = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
                var path = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot", "Files", $"{hungarianFileName}.pdf");

                var wwwRootFolder = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot");
                Utils.PopulateSerbianMappingDictionaries(wwwRootFolder);

                var res = Utils.GetCompanyPdfMetaData(path, CountryFileTypes.Hungarian);

                Assert.False(string.IsNullOrEmpty(res.CompanyName), "Името на компанията не може да е празен стринг!");
                Assert.False(string.IsNullOrEmpty(res.CompanyRegistrationNumber), "CompanyRegistrationNumber не може да е празен стринг!");
                Assert.False(string.IsNullOrEmpty(res.CompanyTaxNumber), "CompanyTaxNumber не може да е празен стринг!");

                Assert.True(res.StartPeriodOfReport.Date != new DateTime().Date, "StartPeriodOfReport");
                Assert.True(res.EndPeriodOfReport.Date != new DateTime().Date, "EndPeriodOfReport");
            }
           


        }
    }
}
