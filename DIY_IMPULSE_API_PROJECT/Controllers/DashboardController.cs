using Microsoft.AspNetCore.Mvc;

namespace DIY_IMPULSE_API_PROJECT.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
