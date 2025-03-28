using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Models;
using WebPhongKham.Models.ViewModel;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IVnpayService _vnPayService;
        private readonly AppDbContext _dbContext;


        public CheckoutController(IVnpayService vnPayService, AppDbContext dbContext)
        {
            _vnPayService = vnPayService;
            _dbContext = dbContext;
        }

        [HttpGet]
        //[Authorize]
        public IActionResult Upgrade()
        {
            var model = new UpgradeAccountViewModel
            {
                PackageName = "Premium",
                Price = 50000, // 50 VND
                Description = "Nâng cấp tài khoản để sử dụng các tính năng cao cấp."
            };
            return View(model);
        }

        [HttpPost]
        //[Authorize]
        public IActionResult UpgradeAccount()
        {
            var username = HttpContext.Session.GetString("Username");
            var tick = DateTime.Now.Ticks.ToString();
            var paymentRequest = new VnPaymentRequest
            {
                OrderId = int.Parse(tick.Substring(0, 9)),
                FullName = username ?? "Khách",
                Description = "Nâng cấp tài khoản Premium",
                Amount = 50000, // 50,000 VND
                CreatedDate = DateTime.Now
            };

            string paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest);
            Console.WriteLine($"Redirecting to: {paymentUrl}");
            return Redirect(paymentUrl);
        }

        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExcute(Request.Query);

            if (response.Success && response.VnPayResponseCode == "00")
            {
                TempData["Success"] = "Thanh toán thành công!";
            }
            else
            {
                TempData["Error"] = "Thanh toán thất bại!";
            }
            return RedirectToAction("Index", "Home");
        }


    }
}
