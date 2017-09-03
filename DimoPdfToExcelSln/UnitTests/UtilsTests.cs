﻿using DimoPdfToExcelWeb.BusinessLogic;
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

            var outputPath = Utils.GetExcelOutputFilePath(wwwRootFolder, path);

            ExcelPackage p = new ExcelPackage(new FileInfo(outputPath));
         

            Process.Start(outputPath);
        }
    }
}
