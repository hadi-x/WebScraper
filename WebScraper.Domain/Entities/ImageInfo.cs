using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper.Domain.Entities
{
    public class ImageInfo
    {
        public string Url { get; set; }      
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}
