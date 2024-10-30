using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPhongKham.Extension;
using WebPhongKham.Models;
using WebPhongKham.Repositories;
using X.PagedList;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[SessionAuthorize("Admin", "Manager")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        private readonly FileUploadHelper _fileUploadHelper;
        private readonly IRepository<Order> _orderRepository;

        public OrderController(AppDbContext context, FileUploadHelper fileUploadHelper, IRepository<Order> orderRepository)
        {
            _context = context;
            _fileUploadHelper = fileUploadHelper;
            _orderRepository = orderRepository;
        }

        // GET: Admin/Order
        //Phần này chưa xử lý được khi sang trang khác vẫn được sắp xếp 
        public async Task<IActionResult> Index(int? page, string sortOrder)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.DateSort = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            //ViewBag.PriceSort = sortOrder == "price_asc" ? "price_desc" : "price_asc";

            //page : trang bắt đầu 
            //page size : số bản ghi 1 trang
            int pageSize = 5;
            var orders = _orderRepository.GetAll(o => o.KhachHang).AsQueryable();
            switch (sortOrder)
            {
                case "date_desc":
                    orders = orders.OrderByDescending(dh => dh.NgayDatHang);
                    break;
                case "price_asc":
                    orders = orders.OrderBy(dh => dh.TongTienSanPham);
                    break;
                case "price_desc":
                    orders = orders.OrderByDescending(dh => dh.TongTienSanPham);
                    break;
                default:
                    orders = orders.OrderBy(dh => dh.NgayDatHang); // Sắp xếp mặc định
                    break;
            }
            return View(orders.ToPagedList(page ?? 1, pageSize));
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return Content("Không có id trong URL");

            var order = await _orderRepository.GetAll(o => o.KhachHang!).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: Admin/Order/Create
        public IActionResult Create()
        {
            GetCustomerInfo();
            return View();
        }

        // POST: Admin/Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order donHang, IFormFile? hinhAnh)
        {
            // Kiểm tra nếu dữ liệu hợp lệ
            if (ModelState.IsValid)
            {
                // Thêm đơn hàng vào database
                _context.Add(donHang);
                await _context.SaveChangesAsync();

                //xử lý hình ảnh nếu có
                if (hinhAnh != null)
                {
                    var fileName = $"order_{donHang.Id}";
                    var filePath = await _fileUploadHelper.UploadFileAsync(hinhAnh, "img/order", fileName);
                    donHang.HinhAnh = filePath;
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                // Sau khi thêm thành công, chuyển hướng về trang Index
                return RedirectToAction(nameof(Index));
            }
            GetCustomerInfo();
            return View(donHang);
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return Content("Không có tham số id trong URL");

            var order = await _orderRepository.GetByIdAsync(id.Value);
            if (order == null)
                return NotFound("Không thấy thông tin sản phẩm");

            GetCustomerInfo();
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int id, Order order, IFormFile? hinhAnh)
        {
            if (id != order.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existingOrder = await _orderRepository.GetByIdAsync(id);
                if (existingOrder == null) return NotFound();

                // Xử lý hình ảnh nếu có ảnh mới
                if (hinhAnh != null && hinhAnh.Length > 0)
                {
                    // Xóa ảnh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(existingOrder.HinhAnh))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingOrder.HinhAnh.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Sử dụng _fileUploadHelper để upload ảnh mới
                    var fileName = $"order_{order.Id}";
                    var filePath = await _fileUploadHelper.UploadFileAsync(hinhAnh, "img/order", fileName);
                    order.HinhAnh = filePath;
                }
                else
                {
                    // Giữ nguyên ảnh cũ nếu không có ảnh mới
                    order.HinhAnh = existingOrder.HinhAnh;
                }

                _context.Entry(existingOrder).CurrentValues.SetValues(order);
                await _orderRepository.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }



        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var order = _orderRepository.GetById(id);
                if (order != null)
                {
                    _orderRepository.Delete(order);
                    _orderRepository.Save();
                    return Json(new { success = true });
                }

                return Json(new { success = false });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }


        public IActionResult Search(string keywordSearching, int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = 10;

            // Nếu ô tìm kiếm trống thì trả về toàn bộ danh sách đơn hàng
            if (string.IsNullOrEmpty(keywordSearching))
            {
                return RedirectToAction(nameof(Index));
            }

            // Lọc kết quả dựa trên từ khóa tìm kiếm
            var result = _orderRepository.GetAll(o=>o.KhachHang!)
                                 .Where(dh => dh.DiaChi.Contains(keywordSearching) ||
                                              dh.ThongTinSanPham.Contains(keywordSearching))
                                 .ToList();

            if (!result.Any())
            {
                ViewBag.Error = "Không tìm thấy đơn hàng";
                //return View("Index", result.ToPagedList(pageNumber, pageSize)); // Trả về dạng phân trang
            }

            // Trả về kết quả tìm kiếm với phân trang
            return View("Index", result.ToPagedList(pageNumber, pageSize));
        }


        public void GetCustomerInfo() => ViewData["idKhachHang"] = new SelectList(_context.Customers, "Id", "HoTen");

    }
}
