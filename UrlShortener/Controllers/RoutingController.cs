using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [Route("")]
    public class RoutingController : Controller
    {
        private readonly IUrlService _urlService;
        public RoutingController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet]
        [Route("/{uniqueIdentifer}")]
        public async Task<IActionResult> RouteToOriginalUrl(string uniqueIdentifer)
        {
            var result = _urlService.GetUrl(uniqueIdentifer);
            return new OkObjectResult(result);
        }
    }
}
