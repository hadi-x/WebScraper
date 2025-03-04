using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.Domain.Entities;

namespace WebScraper.Application.Interfaces
{
    public interface IWebScraperService
    {
        Task<List<ImageInfo>> ExtractImagesAsync(string html, string baseUrl);
        LinkData ExtractLinks(string html, string baseUrl);
    }
}
