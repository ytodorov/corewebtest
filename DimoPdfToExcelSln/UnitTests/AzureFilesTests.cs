using DimoPdfToExcelWeb.BusinessLogic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class AzureFilesTests : UnitTestBase
    {
        [Fact]
        public void UploadFileFromPathTest()
        {
            foreach (var path in CroatiaFileNames)
            {
                var fileNameWithExtension = Path.GetFileName(path);
                AzureFilesUtils.UploadFile("testDir", fileNameWithExtension, path);
            }
        }

        [Fact]
        public void ListFilesTest()
        {
            var list = AzureFilesUtils.ListAllFiles();
            Assert.NotEmpty(list);
        }

    }
}
