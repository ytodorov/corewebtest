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

    }
}
