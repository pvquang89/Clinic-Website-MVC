using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using WebPhongKham.Extension;
using WebPhongKham.Models;
using WebPhongKham.Repositories;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    [SessionAuthorize("Admin", "Manager")]
    public class CustomerController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Address> _addressRepository;

        public CustomerController(AppDbContext context, IRepository<Customer> customerRepository, IRepository<Address> addressRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
        }

        public IActionResult ListCustomer(int page = 1)
        {
            var listCustomer = _customerRepository.GetAll(c => c.DiaChi);

            const int pageSize = 10;
            if (page < 1)
                page = 1;
            int rescCount = listCustomer.Count();
            var pager = new Pager(rescCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = listCustomer.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            return View(data);
        }

        public IActionResult Search(string searchString)
        {
            var data = _customerRepository.GetAll(c => c.DiaChi);
            //nếu ô tìm kiếm null thì trả về all danh sách
            if (String.IsNullOrEmpty(searchString))
                return View("ListCustomer", data.ToList());

            var result = data.Where(kh => kh.HoTen.Contains(searchString) ||
                                kh.DiaChi.TenTinh.Contains(searchString)).ToList();

            // Nếu không có kết quả, gửi thông báo
            if (!result.Any())
                ViewBag.Message = "Không tìm thấy kết quả nào.";

            return View("ListCustomer", result);
        }

        public IActionResult CreateCustomer()
        {
            GetAddressList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            GetAddressList();
            if (!ModelState.IsValid)
                return View(customer);
            try
            {
                _customerRepository.Insert(customer);
                _customerRepository.Save();
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
            //id giờ là kiểu nullable nên có thể null
            if (id == null)
                return Content("Không có id trong request");
            var customer = _customerRepository.GetById(id.Value); // Dùng id.Value để lấy giá trị thực của id
            if (customer == null)
                return Content($"Không tìm thấy kháchh hàng có id {id}");
            GetAddressList();
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(int id, Customer customer)
        {
            //kiểm tra id từ url với id từ form
            if (id != customer.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                _customerRepository.Update(customer);
                _customerRepository.Save();
                return RedirectToAction("ListCustomer");
            }
            GetAddressList();
            return View(customer);
        }

        public void GetAddressList()
        {
            ViewBag.idDiaChi = new SelectList(_addressRepository.GetAll().ToList(), "Id", "TenTinh");
        }
    }
}
