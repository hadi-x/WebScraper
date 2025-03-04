using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.Application.Services;

namespace WebScraper.Tests
{
    public class WebScraperServiceTests
    {
        private readonly WebScraperService _webScraperService;

        public WebScraperServiceTests()
        {
            var httpClient = new HttpClient();
            _webScraperService = new WebScraperService(httpClient);
        }

        [Fact]
        public async Task ExtractImagesAsync_ShouldReturnImages_WhenHtmlContainsImages()
        {
            // Arrange
            string html = "<html><body><img src='https://example.com/image1.jpg' /><img src='https://example.com/image2.png' /></body></html>";
            string baseUrl = "https://example.com";

            // Act
            var images = await _webScraperService.ExtractImagesAsync(html, baseUrl);

            // Assert
            Assert.NotEmpty(images);
            Assert.Equal(2, images.Count);
            Assert.Contains(images, img => img.Url == "https://example.com/image1.jpg");
            Assert.Contains(images, img => img.Url == "https://example.com/image2.png");
        }

        [Fact]
        public void ExtractLinks_ShouldReturnLinks_WhenHtmlContainsAnchors()
        {
            // Arrange
            string html = "<html><body><a href='/internal'>Internal</a><a href='https://external.com'>External</a></body></html>";
            string baseUrl = "https://example.com";

            // Act
            var links = _webScraperService.ExtractLinks(html, baseUrl);

            // Assert
            Assert.NotEmpty(links.InternalLinks);
            Assert.NotEmpty(links.ExternalLinks);

            Assert.Contains("https://example.com/internal", links.InternalLinks);

            Assert.Contains(links.ExternalLinks, link => link == "https://external.com" || link == "https://external.com/");
        }
    }
}
