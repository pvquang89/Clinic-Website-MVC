using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPhongKham.Extension;
using WebPhongKham.Models;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {

        public readonly AppDbContext _context;
        public CustomerController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult ListCustomer(int page = 1)
        {
            var listCustomer = _context.Customers.
                            Include(kh => kh.DiaChi). //eager loading, tải luôn entity địa chỉ 
                            ToList();

            const int pageSize = 10;
            if (page < 1)
                page = 1;
            int rescCount = listCustomer.Count();
            var pager = new Pager(rescCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = listCustomer.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            //return View("ListCustomer", listCustomer);
            return View(data);
        }

        public IActionResult Search(string searchString)
        {
            //nếu ô tìm kiếm null thì trả về all danh sách
            if (String.IsNullOrEmpty(searchString))
                return View("ListCustomer", _context.Customers.ToList());

            var result = _context.Customers
                        .Include(kh => kh.DiaChi)
                        .Where(kh => kh.HoTen.Contains(searchString) ||
                               kh.DiaChi.TenTinh.Contains(searchString)).ToList();

            // Nếu không có kết quả, gửi thông báo
            if (result == null || result.Count == 0)
                ViewBag.Message = "Không tìm thấy kết quả nào.";

            return View("ListCustomer", result);
        }

        public IActionResult CreateCustomer()
        {
            var diaChiList = _context.Addresses.ToList();
            ViewBag.idDiaChi = new SelectList(diaChiList, "Id", "TenTinh");
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            ViewBag.idDiaChi = new SelectList(_context.Addresses, "Id", "TenTinh");
            if (!ModelState.IsValid)
                return View(customer);
            try
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("ListCustomer", new { Area = "Admin" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Đã có lỗi xảy ra khi thêm khách hàng.");
                return View(customer);
            }
        }

        //Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return Content("Không có");
            var customer = _context.Customers.Find(id);
            if (customer == null)
                return Content($"Không tìm thấy kháchh hàng có id {id}");
            ViewBag.idDiaChi = new SelectList(_context.Addresses, "Id", "TenTinh");
            return View(customer);
        }


    }
}
