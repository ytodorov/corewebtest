using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DimoPdfToExcelWeb.Extensions;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class FinancialRow : ICloneable
    {
        // BS or PL
        public string Type { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public string GoesToRowTitle { get; set; }

        public List<int> GoesToRowNumber { get; set; } = new List<int>();

        public string GoesToRowNumberString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in GoesToRowNumber)
                {
                    sb.Append($"{item} ");
                }
                return sb.ToString();
            }
            set
            {

            }
        }



        public string Sign { get; set; } = "+";

        public double PreviousYear { get; set; }

        public double CurrentYear { get; set; }

        public object Clone()
        {
            var res = base.MemberwiseClone();
            return res;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in GoesToRowNumber)
            {
                sb.Append($"{item} ");
            }
            string result = $"{Type} {Number} {Name} --goes to row:{sb.ToString()} prev:{PreviousYear} curr:{CurrentYear}";
            return result;
        }
    }
}
