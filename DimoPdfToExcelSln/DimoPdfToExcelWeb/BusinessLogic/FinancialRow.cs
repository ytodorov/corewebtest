using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class FinancialRow : ICloneable
    {
        // BS or PL
        public string Type { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public string GoesToRowTitle { get; set; }

        public int GoesToRowNumber { get; set; }

        public double PreviousYear { get; set; }

        public double CurrentYear { get; set; }

        public object Clone()
        {
            var res = base.MemberwiseClone();
            return res;
        }

        public override string ToString()
        {
            string result = $"{Type} {Number} {Name} --goes to row:{GoesToRowNumber} prev:{PreviousYear} curr:{CurrentYear}";
            return result;
        }
    }
}
