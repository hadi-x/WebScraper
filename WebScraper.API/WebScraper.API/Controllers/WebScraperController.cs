using Microsoft.AspNetCore.Mvc;
using WebScraper.Application.DTOs;
using WebScraper.Application.Interfaces;

namespace WebScraper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebScraperController : ControllerBase
    {
  
        private readonly IWebScraperService _webScraperService;
        public WebScraperController(  IWebScraperService webScraperService)
        {
            _webScraperService = webScraperService;
        }

        [HttpPost("scrape")]
        public async Task<IActionResult> ScrapeWebsite([FromBody] UrlRequest request)
        {
            if (string.IsNullOrEmpty(request.Url))
                return BadRequest("URL is required.");

            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var validUrl) ||
                (validUrl.Scheme != Uri.UriSchemeHttp && validUrl.Scheme != Uri.UriSchemeHttps))
            {
                return BadRequest("Invalid URL. Please provide a valid URL with http:// or https://");
            }

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync(validUrl);
                var images = await _webScraperService.ExtractImagesAsync(response, request.Url);
                var links = _webScraperService.ExtractLinks(response, request.Url);

                return Ok(new
                {
                    Url = request.Url,
                    Images = images,
                    Links = new
                    {
                        Internal = links.InternalLinks,
                        External = links.ExternalLinks
                    }
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error fetching URL: {ex.Message}");
            }
        }

    }



}
