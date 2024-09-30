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

        public OrderController(AppDbContext context)
        {
            _context = context;
            _fileUploadHelper = new FileUploadHelper();
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
            var donHangs = _context.DonHangs.Include(d => d.KhachHang).AsQueryable();
            switch (sortOrder)
            {
                case "date_desc":
                    donHangs = donHangs.OrderByDescending(dh => dh.NgayDatHang);
                    break;
                case "price_asc":
                    donHangs = donHangs.OrderBy(dh => dh.TongTienSanPham);
                    break;
                case "price_desc":
                    donHangs = donHangs.OrderByDescending(dh => dh.TongTienSanPham);
                    break;
                default:
                    donHangs = donHangs.OrderBy(dh => dh.NgayDatHang); // Sắp xếp mặc định
                    break;
            }
            return View(donHangs.ToPagedList(page ?? 1, pageSize));
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return Content("Không tìm thấy");
            }

            var donHang = await _context.DonHangs
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
            ViewData["idKhachHang"] = new SelectList(_context.KhachHangs, "Id", "HoTen");
            return View();
        }

        // POST: Admin/Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonHang donHang, IFormFile? hinhAnh)
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
            ViewData["idKhachHang"] = new SelectList(_context.KhachHangs, "Id", "HoTen");
            return View(donHang);
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return Content("Không có");

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
                return Content("Không có thông tin sản phẩm");

            ViewData["idKhachHang"] = new SelectList(_context.KhachHangs, "Id", "HoTen", donHang.idKhachHang);
            return View(donHang);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DonHang donHang, IFormFile? hinhAnh)
        {
            if (id != donHang.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (hinhAnh != null)
                    {
                        var fileName = $"order_{donHang.Id}";
                        var filePath = await _fileUploadHelper.UploadFileAsync(hinhAnh, "img/order", fileName);
                        donHang.HinhAnh = filePath;
                    }
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["idKhachHang"] = new SelectList(_context.KhachHangs.ToList(), "Id", "HoTen", donHang.idKhachHang);
            return View(donHang);
        }

        // GET: Admin/Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.KhachHang)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.Id == id);
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
            var result = _context.DonHangs
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
