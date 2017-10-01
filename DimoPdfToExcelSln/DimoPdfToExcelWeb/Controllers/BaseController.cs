using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.Controllers
{
    public class BaseController : Controller
    {
        public virtual IActionResult Index()
        {
            return View();
        }
    }
}
