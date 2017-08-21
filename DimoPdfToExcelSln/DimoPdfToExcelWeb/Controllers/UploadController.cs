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

            return Export();

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



                        // The files are not actually saved in this demo
                        //file.SaveAs(physicalPath);

                        using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            lastPhysicalPath = physicalPath;
                        }

                        using (Stream stream = System.IO.File.OpenRead(physicalPath))
                        {
                            // Load the input file.
                            PdfFixedDocument document = new PdfFixedDocument(stream);

                            PdfRgbColor penColor = new PdfRgbColor();
                            PdfPen pen = new PdfPen(penColor, 0.5);
                            Random rnd = new Random();
                            byte[] rgb = new byte[3];

                            PdfContentExtractor ce = new PdfContentExtractor(document.Pages[0]);
                            PdfTextFragmentCollection tfc = ce.ExtractTextFragments();
                            for (int i = 0; i < tfc.Count; i++)
                            {
                                rnd.NextBytes(rgb);
                                penColor.R = rgb[0];
                                penColor.G = rgb[1];
                                penColor.B = rgb[2];

                                PdfPath boundingPath = new PdfPath();
                                boundingPath.StartSubpath(tfc[i].FragmentCorners[0].X, tfc[i].FragmentCorners[0].Y);
                                boundingPath.AddLineTo(tfc[i].FragmentCorners[1].X, tfc[i].FragmentCorners[1].Y);
                                boundingPath.AddLineTo(tfc[i].FragmentCorners[2].X, tfc[i].FragmentCorners[2].Y);
                                boundingPath.AddLineTo(tfc[i].FragmentCorners[3].X, tfc[i].FragmentCorners[3].Y);
                                boundingPath.CloseSubpath();

                                document.Pages[0].Graphics.DrawPath(pen, boundingPath);
                            }

                            // Do your work with the document inside the using statement.
                        }


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

        public IActionResult Export()
        {
            string sWebRootFolder = HostingEnvironment.WebRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
                //First add the headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Gender";
                worksheet.Cells[1, 4].Value = "Salary (in $)";

                //Add values
                worksheet.Cells["A2"].Value = 1000;
                worksheet.Cells["B2"].Value = "Jon";
                worksheet.Cells["C2"].Value = "M";
                worksheet.Cells["D2"].Value = 5000;

                worksheet.Cells["A3"].Value = 1001;
                worksheet.Cells["B3"].Value = "Graham";
                worksheet.Cells["C3"].Value = "M";
                worksheet.Cells["D3"].Value = 10000;

                worksheet.Cells["A4"].Value = 1002;
                worksheet.Cells["B4"].Value = "Jenny";
                worksheet.Cells["C4"].Value = "F";
                worksheet.Cells["D4"].Value = 5000;

                package.Save(); //Save the workbook.
            }
            var result = PhysicalFile(Path.Combine(sWebRootFolder, sFileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
            {
                FileName = file.Name
            }.ToString();

            return result;
        }

    }
}