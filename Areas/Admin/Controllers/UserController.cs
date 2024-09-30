using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Models;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public readonly AppDbContext _context;

        public UserController(AppDbContext appDbContext)
        {
            _context = appDbContext;
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

            var dsTaiKhoan = _context.TaiKhoans.FirstOrDefault(tk => tk.TenTaiKhoan == user && tk.MatKhau==pass);
            if(dsTaiKhoan == null)
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu";
                return View();
            }
            //return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
            return Redirect("/Admin/Dashboard/Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(TaiKhoan taikhoan)
        {
            if (!ModelState.IsValid)
            {
                return View(taikhoan);
            }
            var tk = _context.TaiKhoans.FirstOrDefault(tk => tk.TenTaiKhoan == taikhoan.TenTaiKhoan);
            if (tk != null)
            {
                // Tạo danh sách gợi ý tên tài khoản
                var suggestions = GenerateUsernameSuggestions(taikhoan.TenTaiKhoan);
                ViewBag.Suggestions = suggestions;
                //ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại.");
                return View(taikhoan);
            }
            _context.TaiKhoans.Add(taikhoan);
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
