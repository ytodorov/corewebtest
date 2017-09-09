using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DimoPdfToExcelWeb.Extensions;

namespace DimoPdfToExcelWeb.Models
{
    public class AzureCloudFileViewModel
    {
        public Uri Uri { get; set; }

        public string SafeUri
        {
            get
            {
                var result = Uri.ToString().EncodeBase64Safe();
                return result;
            }
            set
            {

            }
        }

        public string DirectoryName { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public long Length { get; set; }
    }
}
