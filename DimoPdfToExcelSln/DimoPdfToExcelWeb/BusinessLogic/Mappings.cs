using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public static class Mappings
    {
        //public static Dictionary<string, string> BsDict { get; set; } = new Dictionary<string, string>();

        //public static Dictionary<string, string> PlDict { get; set; } = new Dictionary<string, string>();

        //public static List<string> ExcelBsTitles { get; set; } = new List<string>();

        //public static List<string> ExcelPlTitles { get; set; } = new List<string>();

        public static List<FinancialRow> HungarianBsRows { get; set; } = new List<FinancialRow>();

        public static List<FinancialRow> HungarianPlRows { get; set; } = new List<FinancialRow>();


    }
}
