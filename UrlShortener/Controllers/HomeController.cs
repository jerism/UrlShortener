using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUrlService _urlService;
        public HomeController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string url)
        {
            if (!_urlService.ValidateUrl(url))
            {
                ViewBag.Error = "Url is not valid - must being with https:// or http://";
                return View("Index");
            }

            var result = await _urlService.CreateUrlAsync(url);
            ViewBag.MinifiedLink = result;
            return View("Index");
        }
    }
}
