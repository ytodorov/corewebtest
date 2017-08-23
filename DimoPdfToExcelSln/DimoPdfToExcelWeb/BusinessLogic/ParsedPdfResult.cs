using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class ParsedPdfResult
    {
        public Dictionary<string, int> DictWithValuesBS { get; set; }
        public Dictionary<string, int> DictWithValuesPL { get; set; }

        public ParsedPdfResult()
        {
            DictWithValuesBS = new Dictionary<string, int>();
            DictWithValuesPL = new Dictionary<string, int>();
        }

    }
}
