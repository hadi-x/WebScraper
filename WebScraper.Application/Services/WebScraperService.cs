using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebScraper.Application.Interfaces;
using WebScraper.Domain.Entities;

namespace WebScraper.Application.Services
{
    public class WebScraperService : IWebScraperService
    {
        private readonly HttpClient _httpClient;
        public WebScraperService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<ImageInfo>> ExtractImagesAsync(string html, string baseUrl)
        {
            var images = new List<ImageInfo>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var imageNodes = htmlDoc.DocumentNode.SelectNodes("//img");

            if (imageNodes != null)
            {
                foreach (var img in imageNodes)
                {
                    var src = img.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(src))
                    {
                        try
                        {
                            var absoluteUrl = new Uri(new Uri(baseUrl), src).ToString();
                            var extension = System.IO.Path.GetExtension(src).ToLower();
                            var size = await GetImageSizeAsync(absoluteUrl);

                            images.Add(new ImageInfo { Url = absoluteUrl, Extension = extension, Size = size });
                        }
                        catch (Exception)
                        {
                            // اگر خطایی رخ داد، تصویر را بدون حجم اضافه کنیم
                            images.Add(new ImageInfo { Url = src, Extension = System.IO.Path.GetExtension(src).ToLower(), Size = 0 });
                        }
                    }
                }
            }
            return images;
        }

        public LinkData ExtractLinks(string html, string baseUrl)
        {
            var linkData = new LinkData();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes != null)
            {
                var baseUri = new Uri(baseUrl);

                foreach (var link in linkNodes)
                {
                    var href = link.GetAttributeValue("href", "").Trim();

                    if (!string.IsNullOrEmpty(href))
                    {
                        var absoluteUri = new Uri(baseUri, href);

                        if (absoluteUri.Host == baseUri.Host)
                        {
                            linkData.InternalLinks.Add(absoluteUri.ToString());
                        }
                        else
                        {
                            linkData.ExternalLinks.Add(absoluteUri.ToString());
                        }
                    }
                }
            }

            // حذف لینک‌های تکراری
            linkData.InternalLinks = linkData.InternalLinks.Distinct().ToList();
            linkData.ExternalLinks = linkData.ExternalLinks.Distinct().ToList();

            return linkData;
        }

        private async Task<long> GetImageSizeAsync(string imageUrl)
        {
            try
            {
                using var response = await _httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.Headers.ContentLength ?? 0;
                }
            }
            catch
            {
                // اگر دریافت حجم ممکن نبود، مقدار 0 برگردانیم
            }
            return 0;
        }
    }
}
