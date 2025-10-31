using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller // Inherit from controller class
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public IActionResult Index() // ViewResult inherits from ActionResult and ActionResult implements IActionResult
        {
            var analyticsData = _analyticsService.GetAnalyticsData();

            return View(analyticsData); 
        }


    }
}
