using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPhongKham.Extension;       
using WebPhongKham.Models;
using X.PagedList;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        private readonly FileUploadHelper _fileUploadHelper;

        public OrderController(AppDbContext context, FileUploadHelper fileUploadHelper)
        {
            _context = context;
            _fileUploadHelper = fileUploadHelper;
        }

        // GET: Admin/Order
        //Phần này chưa xử lý được khi sang trang khác vẫn được sắp xếp 
        public async Task<IActionResult> Index(int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSort = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.PriceSort = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            //page : trang bắt đầu 
            //số bản ghi 1 trang
            int pageSize = 10;
            var Orders = _context.Orders.Include(d => d.KhachHang).AsQueryable();
            switch (sortOrder)
            {
                case "date_desc":
                    Orders = Orders.OrderByDescending(dh => dh.NgayDatHang);
                    break;
                case "price_asc":
                    Orders = Orders.OrderBy(dh => dh.TongTienSanPham);
                    break;
                case "price_desc":
                    Orders = Orders.OrderByDescending(dh => dh.TongTienSanPham);
                    break;
                default:
                    Orders = Orders.OrderBy(dh => dh.NgayDatHang); // Sắp xếp mặc định
                    break;
            }
            return View(Orders.ToPagedList(page ?? 1, pageSize));
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return Content("Không tìm thấy");
            }

            var donHang = await _context.Orders
                .Include(d => d.KhachHang)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: Admin/Order/Create
        public IActionResult Create()
        {
            ViewData["idKhachHang"] = new SelectList(_context.Customers, "Id", "HoTen");
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
            ViewData["idKhachHang"] = new SelectList(_context.Customers, "Id", "HoTen");
            return View(donHang);
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return Content("Không có");

            var donHang = await _context.Orders.FindAsync(id);
            if (donHang == null)
                return Content("Không có thông tin sản phẩm");

            ViewData["idKhachHang"] = new SelectList(_context.Customers, "Id", "HoTen", donHang.idKhachHang);
            return View(donHang);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //chưa xử lý đc update ảnh, lúc được lúc không 
        public async Task<IActionResult> Edit(Order order, IFormFile? hinhAnh)
        {
            if (ModelState.IsValid)
            {
                var existOrder = await _context.Orders.FindAsync(order.Id);
                if (existOrder == null) return NotFound();
                //giữ nguyên hình ảnh cũ nếu không có hình ảnh mới được update
                if (hinhAnh != null)
                {
                    var fileName = $"order_{order.Id}";
                    var filePath = await _fileUploadHelper.UploadFileAsync(hinhAnh, "img/order", fileName);
                    existOrder.HinhAnh = filePath;
                }
                // Cập nhật các thuộc tính khác của đơn hàng
                existOrder.NgayDatHang = order.NgayDatHang;
                existOrder.DienThoai = order.DienThoai;
                existOrder.ThongTinSanPham = order.ThongTinSanPham;
                existOrder.DiaChi = order.DiaChi;
                existOrder.TongTienSanPham = order.TongTienSanPham;
                existOrder.PhiShip = order.PhiShip;
                existOrder.idKhachHang = order.idKhachHang; // Cập nhật idKhachHang nếu cần

                _context.Update(existOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["idKhachHang"] = new SelectList(_context.Customers.ToList(), "Id", "HoTen", order.idKhachHang);
            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var order = _context.Orders.Find(id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }

                return Json(new { success = false});
            }
            catch (Exception)
            {
                return Json(new { success = false});
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
            var result = _context.Orders
                                 .Include(dh => dh.KhachHang)
                                 .Where(dh => dh.DiaChi.Contains(keywordSearching) ||
                                              dh.ThongTinSanPham.Contains(keywordSearching))
                                 .ToList();

            if (result == null || result.Count() == 0)
            {
                ViewBag.Error = "Không tìm thấy đơn hàng";
                //return View("Index", result.ToPagedList(pageNumber, pageSize)); // Trả về dạng phân trang
            }

            // Trả về kết quả tìm kiếm với phân trang
            return View("Index", result.ToPagedList(pageNumber, pageSize));
        }

    }
}
