using DimoPdfToExcelWeb.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class AzureFilesUtils
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private static StorageCredentials GetStorageCredentials()
        {
            StorageCredentials sc = new StorageCredentials("al" + "dautomotive",
               "qJmaOC9XhO126Dr4X0kWybU/3lTFBFHXoK9Tte+Ogxy1tuCCMrIDzzNZy7I8XMxsrswTgOlhOp1XiVRq8W" + "Smdw==");
            return sc;
        }

        public static CloudBlobContainer GetCloudDirectoryShare(bool inputFilesShare = true)
        {
            StorageCredentials sc = GetStorageCredentials();
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(sc, true);

            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = null;
            if (inputFilesShare)
            {
                container = blobClient.GetContainerReference("input");
            }
            else
            {
                container = blobClient.GetContainerReference("output");
            }
            var boolResult = container.CreateIfNotExistsAsync().Result;
            return container;
           
        }

        public static void UploadFile(string directoryName, string fileNameWithExtension, Stream stream)
        {
            CloudBlobContainer container = GetCloudDirectoryShare();

            CloudBlockBlob blob = container.GetBlockBlobReference(fileNameWithExtension);
            var boolResult = blob.DeleteIfExistsAsync().Result;
            blob.UploadFromStreamAsync(stream).Wait();
        }

        public static string UploadFile(string directoryName, string fileNameWithExtension, string path)
        {
            var invalidChars = Path.GetInvalidFileNameChars();

            foreach (var invalidChar in invalidChars)
            {
                directoryName = directoryName.Replace(invalidChar.ToString(), string.Empty);
                fileNameWithExtension = fileNameWithExtension.Replace(invalidChar.ToString(), string.Empty);
            }

            CloudBlobContainer container = GetCloudDirectoryShare();
            if (fileNameWithExtension.ToUpperInvariant().EndsWith(".xlsm".ToUpperInvariant()))
            {
                container = GetCloudDirectoryShare(false);
            }
            CloudBlockBlob blob = container.GetBlockBlobReference($"{directoryName}_{fileNameWithExtension}");
            var boolResult = blob.DeleteIfExistsAsync().Result;
            blob.UploadFromFileAsync(path).Wait();
            return blob.Uri.ToString();
        }

        public static void DeleteFile(string directoryName, string fileNameWithExtension, string path)
        {
            CloudBlobContainer container = GetCloudDirectoryShare();
            CloudBlockBlob blob = container.GetBlockBlobReference($"{directoryName}_{fileNameWithExtension}");
            var boolResult = blob.DeleteIfExistsAsync().Result;
        }

        public static void DeleteFileByUri(Uri uri)
        {
            StorageCredentials sc = GetStorageCredentials();
            CloudBlockBlob cf = new CloudBlockBlob(uri, sc);
            var boolResult = cf.DeleteIfExistsAsync().Result;
        }

        public static AzureFileDownloadViewModel DownloadFile(Uri uri)
        {
            StorageCredentials sc = GetStorageCredentials();
            CloudBlockBlob cf = new CloudBlockBlob(uri, sc);
            
            cf.FetchAttributesAsync().Wait();

            AzureFileDownloadViewModel vm = new AzureFileDownloadViewModel();

            long fileByteLength = cf.Properties.Length;
            vm.Name = cf.Name;
            vm.Content = new byte[fileByteLength];
            var boolResult = cf.DownloadToByteArrayAsync(vm.Content, 0).Result;

            if (cf.Name.ToUpperInvariant().EndsWith("pdf".ToUpperInvariant()))
            {
                vm.ContentType = "application/pdf";
                vm.Extension = ".pdf";
            }
            else if (cf.Name.ToUpperInvariant().EndsWith("xlsm".ToUpperInvariant()))
            {
                vm.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                vm.Extension = "xlsm";
            }

            return vm;
                
        }

        public static List<CloudBlockBlob> ListAllFiles()
        {
            var dirs = new List<CloudFileDirectory>();
            CloudBlobContainer inputContainer = GetCloudDirectoryShare();
            var inputBlobs = inputContainer.ListBlobsSegmentedAsync(null).Result.Results;
            CloudBlobContainer outputContainer = GetCloudDirectoryShare(false);
            var outputBlobs = outputContainer.ListBlobsSegmentedAsync(null).Result.Results;

            List<CloudBlockBlob> allBlobs = new List<CloudBlockBlob>();
            allBlobs.AddRange(inputBlobs.Select(s => s as CloudBlockBlob));
            allBlobs.AddRange(outputBlobs.Select(s => s as CloudBlockBlob));

            allBlobs = allBlobs.Where(a => a != null).ToList();

            return allBlobs;
        }
    }
}
