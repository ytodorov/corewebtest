using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
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

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static override bool Equals(object obj)
        //{
        //    throw new Exception("Assertion does not implement Equals, use Ensure or Require");
        //}

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static new bool ReferenceEquals(object objA, object objB)
        //{
        //    throw new Exception("Assertion does not implement ReferenceEquals, use Ensure or Require");
        //}

        public static CloudFileDirectory GetCloudDirectoryShare(bool inputFilesShare = true)
        {
            StorageCredentials sc = new StorageCredentials("yordansto" + "rageaccount",
               "WHN5k4wFTmFmiuzFaWkB4N646yYE9PjrOpiyx7j5iWe3XC" + "GVgi/5ja8jT9LGiIXsvaLB9DYDpUenu7/NQJVZWA==");
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(sc, true);

            // Create a CloudFileClient object for credentialed access to Azure File storage.
            CloudFileClient fileClient = cloudStorageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare share = fileClient.GetShareReference("dimo");
            var boolResult = share.CreateIfNotExistsAsync().Result;
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
            CloudFileDirectory resultDir = null;
            if (inputFilesShare)
            {
                resultDir = rootDir.GetDirectoryReference("DimoInputPdfFiles");
            }
            else
            {
                resultDir = rootDir.GetDirectoryReference("DimoOutputExcelFiles");
            }
            boolResult = resultDir.CreateIfNotExistsAsync().Result;
            return resultDir;
        }

        public static void UploadFile(string directoryName, string fileNameWithExtension, Stream stream)
        {
            CloudFileDirectory cloudFileDirectory = GetCloudDirectoryShare();

            CloudFile cloudFileShare = cloudFileDirectory.GetFileReference(fileNameWithExtension);
            var boolResult = cloudFileShare.DeleteIfExistsAsync().Result;
            cloudFileShare.UploadFromStreamAsync(stream).Wait();
            
        }

        public static void UploadFile(string directoryName, string fileNameWithExtension, string path)
        {
            CloudFileDirectory cloudFileDirectory = GetCloudDirectoryShare();

            CloudFile cloudFileShare = cloudFileDirectory.GetFileReference(fileNameWithExtension);
            var boolResult = cloudFileShare.DeleteIfExistsAsync().Result;
            cloudFileShare.UploadFromFileAsync(path).Wait();
        }

        public static void DeleteFile(string directoryName, string fileNameWithExtension, string path)
        {
            CloudFileDirectory cloudFileDirectory = GetCloudDirectoryShare();

            CloudFile cloudFileShare = cloudFileDirectory.GetFileReference(fileNameWithExtension);
            var boolResult = cloudFileShare.DeleteIfExistsAsync().Result;
        }

        public static List<CloudFile> ListAllFiles()
        {
            CloudFileDirectory cloudFileDirectory = GetCloudDirectoryShare();
            FileContinuationToken fct = new FileContinuationToken();

            FileResultSegment fileResultSegment = cloudFileDirectory.ListFilesAndDirectoriesSegmentedAsync(fct).Result;

            List<IListFileItem> list = fileResultSegment.Results.ToList();
            List<CloudFile> cloudFileList = new List<CloudFile>();
            foreach (var l in list)
            {
                CloudFile cf = l as CloudFile;
                if (cf != null)
                {
                    cloudFileList.Add(cf);
                }                
            }
            return cloudFileList;


        }




    }
}
