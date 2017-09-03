using DimoPdfToExcelWeb.BusinessLogic;
using System;
using System.IO;
using Xunit;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void ParseHungarianMappingFileTest()
        {
            var rootSolution = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var wwwRootFolder = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot");
            Utils.PopulateHungarianMappingDictionaries(wwwRootFolder);
            
            foreach (var row in Mappings.HungarianBsRows)
            {
                Assert.False(row.GoesToRowNumber == 0);                
            }

            foreach (var row in Mappings.HungarianPlRows)
            {
                Assert.False(row.GoesToRowNumber == 0);
            }

        }

        [Fact]
        public void ParseSerbianMappingFileTest()
        {
            var rootSolution = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var wwwRootFolder = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot");
            Utils.PopulateSerbianMappingDictionaries(wwwRootFolder);

            foreach (var row in Mappings.SerbianBsRows)
            {
                Assert.False(row.GoesToRowNumber == 0);
            }

            foreach (var row in Mappings.SerbianPlRows)
            {
                Assert.False(row.GoesToRowNumber == 0);
            }

        }
    }
}
