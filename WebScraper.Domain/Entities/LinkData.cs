using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper.Domain.Entities
{
    public class LinkData
    {
        public List<string> InternalLinks { get; set; } = new List<string>();
        public List<string> ExternalLinks { get; set; } = new List<string>();
    }
}
