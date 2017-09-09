using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.Models
{
    public class AzureCloudFileViewModel
    {
        public Uri Uri { get; set; }

        public string DirectoryName { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public long Length { get; set; }
    }
}
