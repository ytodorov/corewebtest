using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimoPdfToExcelWeb.Models
{
    public class AzureFileDownloadViewModel
    {
        public string Name { get; set; }

        public byte[] Content { get; set; }

        public string Extension { get; set; }

        public string ContentType { get; set; }

    }
}
