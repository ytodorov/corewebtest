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
    public class UtilsTests : UnitTestBase
    {
        [Fact]
        public void ParseHungarianPdfTest()
        {
            foreach (var path in HungarianFileNames)
            {
                //if (!path.Contains("4"))
                {
                    var result = Utils.ParseHungarianPdf(path);
                    var outputPath = Utils.GetExcelOutputFilePath(WwwRootFolder, path);
                    var isValid = Utils.IsFinalExcelFileValid(outputPath, result);
                    Assert.True(isValid);
                }
                
            }
        }

        [Fact]
        public void ParseSerbianPdfTest()
        {
            foreach (var path in SerbianFileNames)
            {
                var result = Utils.ParseSerbianPdf(path);
                var outputPath = Utils.GetExcelOutputFilePath(WwwRootFolder, path);
                var isValid = Utils.IsFinalExcelFileValid(outputPath);
                Assert.True(isValid);
            }
        }

        [Fact]
        public void ParseCroatiaPdfTest()
        {
            //foreach (var path in CroatiaFileNames)
            //{
            //    var result = Utils.ParseCroatiaPdf(path);
            //    var outputPath = Utils.GetExcelOutputFilePath(WwwRootFolder, path);
            //}
        }

        [Fact]
        public void ParseSlovenianPdfTest()
        {
            //foreach (var path in SlovenianFileNames)
            //{
            //    var result = Utils.ParseSlovenianPdf(path);
            //    var outputPath = Utils.GetExcelOutputFilePath(WwwRootFolder, path);
            //}
        }

        [Fact]
        public void GetCompanyPdfMetaDataTest()
        {
            foreach (var fullFileName in AllFileNames)
            {
                CompanyPdfMetaData res = Utils.GetCompanyPdfMetaData(fullFileName);

                Assert.False(string.IsNullOrEmpty(res.CompanyName), "Името на компанията не може да е празен стринг!");
                Assert.False(string.IsNullOrEmpty(res.CompanyRegistrationNumber), "CompanyRegistrationNumber не може да е празен стринг!");
                Assert.False(string.IsNullOrEmpty(res.CompanyTaxNumber), "CompanyTaxNumber не може да е празен стринг!");

                Assert.True(res.StartPeriodOfReport.Date != new DateTime().Date, "StartPeriodOfReport");
                Assert.True(res.EndPeriodOfReport.Date != new DateTime().Date, "EndPeriodOfReport");
            }
        }

        [Fact]
        public void GetCountryFileTypesFromPdfFileTest()
        {
            foreach (var fullFileName in HungarianFileNames)
            {
                var type = Utils.GetCountryFileTypesFromPdfFile(fullFileName);
                Assert.Equal(CountryFileTypes.Hungarian, type);
            }

            foreach (var fullFileName in SerbianFileNames)
            {
                var type = Utils.GetCountryFileTypesFromPdfFile(fullFileName);
                Assert.Equal(CountryFileTypes.Serbian, type);
            }
        }
    }
}
