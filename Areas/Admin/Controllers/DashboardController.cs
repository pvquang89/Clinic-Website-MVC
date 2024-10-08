using Microsoft.AspNetCore.Mvc;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            var user = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(user))
            {
                TempData["Error"] = "Bạn cần đăng nhập để vào dashbroad";
                return RedirectToAction("Login", "User", new { area = "Admin" });
            }
            ViewBag.Name = user;
            return View("Index");
        }

       
    }
}
