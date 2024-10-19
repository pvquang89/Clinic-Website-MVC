using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Extension;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    [SessionAuthorize("Admin", "Manager", "Viewer")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NoAccess()
        {
            return View();
        }

    }
}
