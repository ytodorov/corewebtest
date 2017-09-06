using System.Collections.Generic;

namespace DimoPdfToExcelWeb.BusinessLogic
{
    public static class Mappings
    {      

        public static List<FinancialRow> HungarianBsRows { get; set; } = new List<FinancialRow>();

        public static List<FinancialRow> HungarianPlRows { get; set; } = new List<FinancialRow>();

        public static List<FinancialRow> SerbianBsRows { get; set; } = new List<FinancialRow>();

        public static List<FinancialRow> SerbianPlRows { get; set; } = new List<FinancialRow>();

        public static List<FinancialRow> CroatiaBsRows { get; set; } = new List<FinancialRow>();

        public static List<FinancialRow> CroatiaPlRows { get; set; } = new List<FinancialRow>();

        public static List<FinancialRow> GetFreshList(List<FinancialRow> source)
        {
            List<FinancialRow> result = new List<FinancialRow>();
            foreach (var item in source)
            {
                var newItem = item.Clone() as FinancialRow;
                result.Add(newItem);
            }
            return result;
        }
    }
}
