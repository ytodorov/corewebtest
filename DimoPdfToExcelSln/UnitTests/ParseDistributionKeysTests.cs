using DimoPdfToExcelWeb.BusinessLogic;
using System;
using System.IO;
using Xunit;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void ParseHungarianFileTest()
        {
            var rootSolution = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var wwwRootFolder = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot");
            Utils.PopulateMappingDictionaries(wwwRootFolder);
            
            foreach (var row in Mappings.HungarianBsRows)
            {
                Assert.False(row.GoesToRowNumber == 0);                
            }

            foreach (var row in Mappings.HungarianPlRows)
            {
                Assert.False(row.GoesToRowNumber == 0);
            }

        }
    }
}
