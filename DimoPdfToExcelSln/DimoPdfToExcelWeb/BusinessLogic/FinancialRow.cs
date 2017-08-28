﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class FinancialRow
    {
        // BS or PL
        public string Type { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public string GoesTo { get; set; }

        public double PreviousYear { get; set; }

        public double CurrentYear { get; set; }

        public override string ToString()
        {
            string result = $"{Type} {Number} {Name} --goes to-- {GoesTo} {PreviousYear} {CurrentYear}";
            return result;
        }
    }
}