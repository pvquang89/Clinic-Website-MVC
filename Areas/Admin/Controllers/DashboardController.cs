using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Extension;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    [SessionAuthorize("Admin", "Manager")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
