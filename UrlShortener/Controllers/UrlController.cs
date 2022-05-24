using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [Route("url")]
    public class UrlController : Controller
    {
        private readonly IUrlService _urlService;
        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddUrl([FromQuery] string url)
        {
            var result = await _urlService.CreateUrlAsync(url);
            return new OkObjectResult(result);
        }
    }
}
