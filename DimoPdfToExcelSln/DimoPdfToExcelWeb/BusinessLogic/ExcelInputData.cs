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
        public List<ExcellOutputRowData> BsValues { get; set; } = new List<ExcellOutputRowData>();

        public List<ExcellOutputRowData> PlValues { get; set; } = new List<ExcellOutputRowData>();


    }
}
