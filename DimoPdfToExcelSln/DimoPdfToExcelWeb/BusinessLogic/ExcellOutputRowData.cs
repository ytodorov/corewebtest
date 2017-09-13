using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class ExcellOutputRowData
    {
        public int RowNumber { get; set; }

        public int CurrentYear { get; set; }

        public int PreviousYear { get; set; }

        public override string ToString()
        {
            var result = $"{RowNumber} {CurrentYear} {PreviousYear}";
            return result;
        }

    }
}
