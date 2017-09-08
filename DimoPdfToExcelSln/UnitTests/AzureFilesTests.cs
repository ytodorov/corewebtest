using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class AzureFilesTests
    {
        [Fact]
        public void UploadFileTest()
        {
            StorageCredentials sc = new StorageCredentials("yordansto" + "rageaccount",
                "WHN5k4wFTmFmiuzFaWkB4N646yYE9PjrOpiyx7j5iWe3XC" + "GVgi/5ja8jT9LGiIXsvaLB9DYDpUenu7/NQJVZWA==");
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(sc, true);

            // Create a CloudFileClient object for credentialed access to Azure File storage.
            CloudFileClient fileClient = cloudStorageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare share = fileClient.GetShareReference("test");
            var res = share.CreateIfNotExistsAsync().Result;

            

            // Ensure that the share exists.
            if (share.ExistsAsync().Result)
            {
                //// Get a reference to the root directory for the share.
                //CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                //// Get a reference to the directory we created previously.
                //CloudFileDirectory sampleDir = rootDir.GetDirectoryReference("CustomLogs");

                //// Ensure that the directory exists.
                //if (sampleDir.ExistsAsync().Result)
                //{
                //    // Get a reference to the file we created previously.
                //    CloudFile file = sampleDir.GetFileReference("Log1.txt");

                //    // Ensure that the file exists.
                //    if (file.ExistsAsync().Result)
                //    {
                //        // Write the contents of the file to the console window.
                //        Console.WriteLine(file.DownloadTextAsync().Result);
                //    }
                //}
            }
        }
    }
}
