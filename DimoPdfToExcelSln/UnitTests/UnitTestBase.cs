using DimoPdfToExcelWeb.BusinessLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests
{
    public class UnitTestBase
    {
        public List<string> HungarianFileNames { get; set; } = new List<string>();
        public List<string> SerbianFileNames { get; set; } = new List<string>();
        public List<string> CroatiaFileNames { get; set; } = new List<string>();

        public List<string> SlovenianFileNames { get; set; } = new List<string>();

        public List<string> AllFileNames { get; set; } = new List<string>();


        public string WwwRootFolder { get; set; }

        public UnitTestBase()
        {
            var rootSolution = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var directoryWithPdfs = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot", "Files");

            string[] files = Directory.GetFiles(directoryWithPdfs);

            foreach (var fullFilePath in files)
            {
                string fileName = Path.GetFileName(fullFilePath);
                string ext = Path.GetExtension(fullFilePath);
                if (ext.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (fileName.ToUpperInvariant().Contains("hungarian".ToUpperInvariant()))
                    {
                        HungarianFileNames.Add(fullFilePath);
                    }
                    if (fileName.ToUpperInvariant().Contains("serbian".ToUpperInvariant()))
                    {
                        SerbianFileNames.Add(fullFilePath);
                    }
                    if (fileName.ToUpperInvariant().Contains("croatian".ToUpperInvariant()))
                    {
                        CroatiaFileNames.Add(fullFilePath);
                    }
                    if (fileName.ToUpperInvariant().Contains("slovenian".ToUpperInvariant()))
                    {
                        SlovenianFileNames.Add(fullFilePath);
                    }
                }
            }

            AllFileNames.AddRange(HungarianFileNames);
            AllFileNames.AddRange(SerbianFileNames);

            WwwRootFolder = Path.Combine(rootSolution, "DimoPdfToExcelWeb", "wwwroot");
            Utils.PopulateHungarianMappingDictionaries(WwwRootFolder);
            Utils.PopulateSerbianMappingDictionaries(WwwRootFolder);
            Utils.PopulateCroatianMappingDictionaries(WwwRootFolder);

        }
    }
}
