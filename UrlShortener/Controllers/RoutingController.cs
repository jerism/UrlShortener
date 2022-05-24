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
        public IActionResult RouteToOriginalUrl(string uniqueIdentifer)
        {
            var result = _urlService.GetUrl(uniqueIdentifer);

            if (string.IsNullOrEmpty(result))
            {
                ViewBag.Error = $"Could not find matching link for unique identifer '{uniqueIdentifer}' please check the url.";
                return View("Error");
            }

            return new RedirectResult(result);
        }
    }
}
