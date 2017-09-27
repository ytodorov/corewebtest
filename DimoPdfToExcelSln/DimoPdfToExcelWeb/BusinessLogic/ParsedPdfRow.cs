using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class ParsedPdfRow
    {
        /// <summary>
        /// Number: 001, 034, 109
        /// </summary>
        public string Number { get; set; }

        public string Name { get; set; }

        public int PreviousYear { get; set; }

        public int CurrentYear { get; set; }

        public override string ToString()
        {
            string result = $"{Number} {Name} {PreviousYear} {CurrentYear}";
            return result;
        }
    }
}
