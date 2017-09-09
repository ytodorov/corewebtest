using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DimoPdfToExcelWeb.Models;
using Kendo.Mvc.UI;
using DimoPdfToExcelWeb.BusinessLogic;
using Kendo.Mvc.Extensions;
using System.IO;
using DimoPdfToExcelWeb.Extensions;
using Microsoft.AspNetCore.Http;

namespace DimoPdfToExcelWeb.Controllers
{
    public class GridController : Controller
    {
        public ActionResult BsRows_Read([DataSourceRequest] DataSourceRequest request)
        {
            var bsRows = Mappings.HungarianBsRows;
            var result = Json(bsRows.ToDataSourceResult(request));
            return result;
        }

        public ActionResult PlRows_Read([DataSourceRequest] DataSourceRequest request)
        {
            var plRows = Mappings.HungarianPlRows;
            var result = Json(plRows.ToDataSourceResult(request));
            return result;
        }

        public ActionResult AzureFiles_Read([DataSourceRequest] DataSourceRequest request)
        {
            var files = AzureFilesUtils.ListAllFiles();

            var resultList = new List<AzureCloudFileViewModel>();

            foreach (var file in files)
            {
                AzureCloudFileViewModel model = new AzureCloudFileViewModel();
                model.Uri = file.Uri;
                model.FileName = file.Name;
                model.DirectoryName = file.Parent.Name;
                model.Extension = Path.GetExtension(file.Name);
                model.Length = file.Properties.Length;
                resultList.Add(model);
                var test = model.SafeUri;
            }



            var result = Json(resultList.ToDataSourceResult(request));
            return result;
        }


        public ActionResult AzureFiles_Destroy([DataSourceRequest] DataSourceRequest request,
           AzureCloudFileViewModel azureCloudFileViewModel)
        {
            AzureFilesUtils.DeleteFileByUri(azureCloudFileViewModel.Uri);
            return Json(new[] { azureCloudFileViewModel }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Download(string safeUri)
        {
            var realUri = safeUri.DecodeBase64Safe();
            Uri uri = new Uri(realUri);
            var res = AzureFilesUtils.DownloadFile(uri);

            return File(res.Content, res.ContentType, res.Name);
        }


    }
}
