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
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
