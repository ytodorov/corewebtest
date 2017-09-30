using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using DimoPdfToExcelWeb.Models;
using Xfinium.Pdf;
using Xfinium.Pdf.Graphics;
using Xfinium.Pdf.Content;
using OfficeOpenXml;
using DimoPdfToExcelWeb.BusinessLogic;
using System.Globalization;
using System.Threading;
using DimoPdfToExcelWeb.Extensions;

namespace DimoPdfToExcelWeb.Controllers
{
    public class UploadController : Controller
    {
        public IHostingEnvironment HostingEnvironment { get; set; }

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }

        public static string lastPhysicalPath = string.Empty;

        public IActionResult Excel()
        {

            //var document = ...
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = "test.pdf",

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };


            Response.Headers.Add("Content-Disposition", cd.ToString());
            //Response.AppendHeader("Content-Disposition", cd.ToString());
            //return File(System.IO.File.ReadAllBytes(lastPhysicalPath), "application/pdf");

            string sWebRootFolder = HostingEnvironment.WebRootPath;

            // Decide country File type

            string outputExcelFilePath = Utils.GetExcelOutputFilePath(sWebRootFolder, lastPhysicalPath);

            var test = HttpContext.Session.GetInt32("one");
            HttpContext.Session.SetInt32("one", 1);
            test = HttpContext.Session.GetInt32("one");


            CompanyPdfMetaData cpmd = Utils.GetCompanyPdfMetaData(lastPhysicalPath);
            string fileNameInAzure = $"From {cpmd.StartPeriodOfReport.Day}_{cpmd.StartPeriodOfReport.Month}_{cpmd.StartPeriodOfReport.Year} to {cpmd.EndPeriodOfReport.Day}_{cpmd.EndPeriodOfReport.Month}_{cpmd.EndPeriodOfReport.Year}.xlsm";
            string url =
                AzureFilesUtils.UploadFile(cpmd.CompanyName, fileNameInAzure, outputExcelFilePath);
            HttpContext.Session.SetString("excelUrl", url);
            //var result = PhysicalFile(outputExcelFilePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //	application/vnd.ms-excel.sheet.macroEnabled.12
            var result = PhysicalFile(outputExcelFilePath, "application/vnd.ms-excel.sheet.macroEnabled.12");
            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(outputExcelFilePath)
            }.ToString();

            var result2 = url.ToString();//.EncodeBase64Safe();
            return Json(result2);

            //return result;

            // тест за сваляне
        }

        public ActionResult ChunkSave(IEnumerable<IFormFile> files, string metaData)
        {
            string dirPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
                

            if (metaData == null)
            {
                return Save(files);
            }

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(metaData));

            JsonSerializer serializer = new JsonSerializer();
            ChunkMetaData chunkData;
            using (StreamReader streamReader = new StreamReader(ms))
            {
                chunkData = (ChunkMetaData)serializer.Deserialize(streamReader, typeof(ChunkMetaData));
            }

            string path = String.Empty;
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    path = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", chunkData.FileName);

                    //AppendToFile(path, file);
                }
            }

            FileResultData fileBlob = new FileResultData();
            fileBlob.uploaded = chunkData.TotalChunks - 1 <= chunkData.ChunkIndex;
            fileBlob.fileUid = chunkData.UploadUid;

            return Json(fileBlob);
        }

        public ActionResult GetLastPdfUrl()
        {
            var url = HttpContext.Session.GetString("url");
            return Json(url);
        }

        public ActionResult GetLastExcelUrl()
        {
            Thread.Sleep(5000);
            var url = HttpContext.Session.GetString("excelUrl");
            if (!string.IsNullOrEmpty(url))
            {
                return Json(url);
            }
            else
            {
                return Json(string.Empty);
            }
            
        }


        public ActionResult Save(IEnumerable<IFormFile> files)
        {
            try
            {
                // The Name of the Upload component is "files"
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                        // Some browsers send file names with full path.
                        // We are only interested in the file name.
                        var fileName = Path.GetFileName(fileContent.FileName.Trim('"'));
                     
                        var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", fileName);
                        var extension = Path.GetExtension(physicalPath);


                        // The files are not actually saved in this demo
                        //file.SaveAs(physicalPath);

                        using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            lastPhysicalPath = physicalPath;
                        }

                        CompanyPdfMetaData cpmd = Utils.GetCompanyPdfMetaData(physicalPath);
                        string fileNameInAzure = $"From {cpmd.StartPeriodOfReport.Day}_{cpmd.StartPeriodOfReport.Month}_{cpmd.StartPeriodOfReport.Year} to {cpmd.EndPeriodOfReport.Day}_{cpmd.EndPeriodOfReport.Month}_{cpmd.EndPeriodOfReport.Year}{extension}";

                        var url = AzureFilesUtils.UploadFile(cpmd.CompanyName, fileNameInAzure, physicalPath);
                        HttpContext.Session.SetString("url", url);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Return an empty string to signify success
            return Content("");


        }


        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        

    }
}