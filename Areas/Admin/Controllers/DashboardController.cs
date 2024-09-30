using Microsoft.AspNetCore.Mvc;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
      public ActionResult Index()
        {
            return View("Index");
        }
    }
}
