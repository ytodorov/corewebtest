using DimoPdfToExcelWeb.BusinessLogic;
using System;
using System.IO;
using Xunit;

namespace UnitTests
{
    public class ParseDistributionKeysTests : UnitTestBase
    {
        [Fact]
        public void ParseHungarianMappingFileTest()
        {
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
            foreach (var row in Mappings.SerbianBsRows)
            {
                Assert.False(row.GoesToRowNumber == 0);
            }

            foreach (var row in Mappings.SerbianPlRows)
            {
                Assert.False(row.GoesToRowNumber == 0);
            }
        }

        [Fact]
        public void ParseCroatianMappingFileTest()
        {
            foreach (var row in Mappings.CroatiaBsRows)
            {
                Assert.False(row.GoesToRowNumber == 0);
            }

            foreach (var row in Mappings.CroatiaPlRows)
            {
                Assert.False(row.GoesToRowNumber == 0);
            }
        }
    }
}
