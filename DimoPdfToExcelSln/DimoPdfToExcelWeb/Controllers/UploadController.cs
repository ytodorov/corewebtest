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
            return File(System.IO.File.ReadAllBytes(lastPhysicalPath), "application/pdf");

            // тест за сваляне
        }

        public ActionResult ChunkSave(IEnumerable<IFormFile> files, string metaData)
        {
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