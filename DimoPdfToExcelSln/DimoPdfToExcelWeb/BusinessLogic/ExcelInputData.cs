using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class ExcelInputData
    {
        public Dictionary<string, int> BsValues { get; set; } = new Dictionary<string, int>();

        public Dictionary<string, int> PlValues { get; set; } = new Dictionary<string, int>();


    }
}
