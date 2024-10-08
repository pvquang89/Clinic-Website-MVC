using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Extension;
using WebPhongKham.Models;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(string user, string pass)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                ViewBag.Error = "Username và password không được để trống.";
                return View();          
            }

            var acc = _context.Accounts.FirstOrDefault(tk => tk.TenTaiKhoan == user && tk.MatKhau==pass);
            if(acc == null)
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu";
                return View();
            }

            // Lưu thông tin đăng nhập vào session
            HttpContext.Session.SetString("UserName", acc.TenTaiKhoan);
            HttpContext.Session.SetString("Password", acc.MatKhau);
            return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
            //return Redirect("/Admin/Dashboard/Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account taikhoan)
        {
            if (!ModelState.IsValid)
            {
                return View(taikhoan);
            }
            var tk = _context.Accounts.FirstOrDefault(tk => tk.TenTaiKhoan == taikhoan.TenTaiKhoan);
            if (tk != null)
            {
                // Tạo danh sách gợi ý tên tài khoản
                var suggestions = GenerateUsernameSuggestions(taikhoan.TenTaiKhoan);
                ViewBag.Suggestions = suggestions;
                //ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại.");
                return View(taikhoan);
            }
            _context.Accounts.Add(taikhoan);
            _context.SaveChanges();

            return RedirectToAction("Login", new { area = "Admin" });
        }


        // Hàm tạo gợi ý tên tài khoản
        private List<string> GenerateUsernameSuggestions(string existingUsername)
        {
            var suggestions = new List<string>();
            for (int i = 1; i <= 3; i++)
            {
                suggestions.Add($"{existingUsername}{new Random().Next(10,100)}");
            }
            return suggestions;
        }



  


    }
}
