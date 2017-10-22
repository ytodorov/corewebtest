using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DimoPdfToExcelWeb.Models;
using System.Text;
using DimoPdfToExcelWeb.BusinessLogic;

namespace DimoPdfToExcelWeb.Controllers
{
    public class HomeController : BaseController
    {
        [RouteAttribute(".well-known/pki-validation/F3092F6DCEB9E2E4CE1BAB2C240B5B3B.txt", Name = "F3092F6DCEB9E2E4CE1BAB2C240B5B3B.txt")]
        public IActionResult Ssl()
        {
            var res = Content("D22F8819780A3AE559D19EF6C9AA970D10623EF06E4214E55D9C23124856F1AE comodoca.com 59ecf87c53455");
            return res;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DebugInfo()
        {
            StringBuilder sb = new StringBuilder();

            var bsRows = Mappings.HungarianBsRows;

            var bsGr = bsRows.GroupBy(b => b.GoesToRowTitle);

            foreach (var g in bsGr)
            {
                sb.Append(g.Key);
                sb.Append("<br />");
                double sum = 0;

                foreach (var item in g)
                {
                    sb.Append(item);
                    sum += item.CurrentYear;
                    sb.Append("<br />");
                }
                sb.Append($"Sum: {sum}");
                sb.Append("<br />");
                sb.Append("<br />");
            }
            var plRows = Mappings.HungarianPlRows;
            var plGr = plRows.GroupBy(b => b.GoesToRowTitle);

            foreach (var g in plGr)
            {
                sb.Append(g.Key);
                sb.Append("<br />");
                double sum = 0;

                foreach (var item in g)
                {
                    sb.Append(item);
                    sum += item.CurrentYear;
                    sb.Append("<br />");
                }
                sb.Append($"Sum: {sum}");
                sb.Append("<br />");
                sb.Append("<br />");
            }

            var str = sb.ToString();
            var result = Json(str);
            return result;
        }
    }
}