using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class ExcelInputData
    {
        /// <summary>
        /// excel row number / sum
        /// </summary>
        public Dictionary<int, int> BsValues { get; set; } = new Dictionary<int, int>();

        public Dictionary<int, int> PlValues { get; set; } = new Dictionary<int, int>();


    }
}
