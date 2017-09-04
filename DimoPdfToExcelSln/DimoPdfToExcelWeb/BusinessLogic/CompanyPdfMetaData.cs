using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public class CompanyPdfMetaData
    {
        public string CompanyName { get; set; }

        public string CompanyRegistrationNumber { get; set; }

        public string CompanyTaxNumber { get; set; }

        public DateTime StartPeriodOfReport { get; set; }

        public DateTime EndPeriodOfReport { get; set; }
    }
}
