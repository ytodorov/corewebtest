using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class ParsedPdfResult
    {
        public List<ParsedPdfRow> DictWithValuesBS { get; set; } = new List<ParsedPdfRow>();
        public List<ParsedPdfRow> DictWithValuesPL { get; set; } = new List<ParsedPdfRow>();

        public List<FinancialRow> BsRows { get; set; } = new List<FinancialRow>();

        public List<FinancialRow> PlRows { get; set; } = new List<FinancialRow>();

        public ParsedPdfResult()
        {
        }
    }
}