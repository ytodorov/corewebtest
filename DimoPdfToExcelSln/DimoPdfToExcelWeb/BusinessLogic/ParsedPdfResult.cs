using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class ParsedPdfResult
    {
        public List<ParsedPdfRow> DictWithValuesBS { get; set; }
        public List<ParsedPdfRow> DictWithValuesPL { get; set; }

        public ParsedPdfResult()
        {
            DictWithValuesBS = new List<ParsedPdfRow>();

            DictWithValuesPL = new List<ParsedPdfRow>();
        }

    }
}
