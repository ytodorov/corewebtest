using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DimoPdfToExcelWeb.Controllers
{
    public class ConvertController : BaseController
    {
        [Authorize]
        public override IActionResult Index()
        {
            return base.Index();
        }
    }
}
