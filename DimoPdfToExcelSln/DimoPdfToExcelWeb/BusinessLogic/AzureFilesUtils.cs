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
            var invalidChars = Path.GetInvalidFileNameChars();

            foreach (var invalidChar in invalidChars)
            {
                directoryName = directoryName.Replace(invalidChar.ToString(), string.Empty);
                fileNameWithExtension = fileNameWithExtension.Replace(invalidChar.ToString(), string.Empty);
            }

            CloudFileDirectory rootFileDirectory = GetCloudDirectoryShare();

            CloudFileDirectory companyCloudDirectory = rootFileDirectory.GetDirectoryReference(directoryName);
            var boolResult = companyCloudDirectory.CreateIfNotExistsAsync().Result;
            CloudFile cloudFileShare = companyCloudDirectory.GetFileReference(fileNameWithExtension);
            boolResult = cloudFileShare.DeleteIfExistsAsync().Result;
            cloudFileShare.UploadFromFileAsync(path).Wait();
        }

        public static void DeleteFile(string directoryName, string fileNameWithExtension, string path)
        {
            CloudFileDirectory rootFileDirectory = GetCloudDirectoryShare();
            CloudFileDirectory companyCloudDirectory = rootFileDirectory.GetDirectoryReference(directoryName);
            CloudFile cloudFileShare = companyCloudDirectory.GetFileReference(fileNameWithExtension);
            var boolResult = cloudFileShare.DeleteIfExistsAsync().Result;
        }

        public static void DeleteFileByUri(Uri uri)
        {
            StorageCredentials sc = new StorageCredentials("yordansto" + "rageaccount",
               "WHN5k4wFTmFmiuzFaWkB4N646yYE9PjrOpiyx7j5iWe3XC" + "GVgi/5ja8jT9LGiIXsvaLB9DYDpUenu7/NQJVZWA==");
            CloudFile cf = new CloudFile(uri, sc);
            var boolResult = cf.DeleteIfExistsAsync().Result;
        }

        public static List<CloudFile> ListAllFiles()
        {
            var dirs = new List<CloudFileDirectory>();

            

            CloudFileDirectory inputDirectory = GetCloudDirectoryShare();

            

            dirs.Add(inputDirectory);
            CloudFileDirectory outputDirectory = GetCloudDirectoryShare(false);
            dirs.Add(outputDirectory);
            List<CloudFile> cloudFileList = new List<CloudFile>();
            foreach (CloudFileDirectory cloudFileDirectory in dirs)
            {
                FileContinuationToken fct = new FileContinuationToken();

                FileResultSegment fileResultSegment = cloudFileDirectory.ListFilesAndDirectoriesSegmentedAsync(fct).Result;
                
                List<IListFileItem> list = fileResultSegment.Results.ToList();
           
                foreach (var l in list)
                {
                    CloudFile cf = l as CloudFile;
                    if (cf != null)
                    {
                        cloudFileList.Add(cf);
                    }
                    else
                    {
                        CloudFileDirectory cfd = l as CloudFileDirectory;
                        if (cfd != null)
                        {
                            var subFiles = cfd.ListFilesAndDirectoriesSegmentedAsync(fct).Result.Results.ToList();
                            foreach (var sunItem in subFiles)
                            {
                                var dummy = sunItem as CloudFile;
                                if (dummy != null)
                                {
                                    cloudFileList.Add(dummy);
                                }
                            }
                        }
                    }
                }
            }

           
            return cloudFileList;
        }




    }
}
