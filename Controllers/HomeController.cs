using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Numerics;
using WebPhongKham.Models;

namespace WebPhongKham.Controllers
{
    public class HomeController : Controller
    {
        public static List<Doctor> ListDoctor = new List<Doctor>()
        {
            new Doctor{
                Id=1,
                HoTen="Linh",
                DiaChi = "Ha Noi",
                NgaySinh = new DateTime(2002,12,12),
                ChuyenKhoa= "Răng hàm mặt",
                LinkFace=""
            },
            new Doctor()
            {
                Id = 2,
                HoTen = "Quang",
                DiaChi = "Vinh Phuc",
                NgaySinh = new DateTime(2002, 12, 12),
                ChuyenKhoa = "Tiêu hoá",
                LinkFace = ""
            },
            new Doctor()
            {
                Id = 3,
                HoTen = "Ngọc",
                DiaChi = "Thanh Hoa",
                NgaySinh = new DateTime(2002, 12, 12),
                ChuyenKhoa = "Tiêu hoá",
                LinkFace = ""
            },
            new Doctor()
            {
                Id = 4,
                HoTen = "Hà",
                DiaChi = "Ha Noi",
                NgaySinh = new DateTime(2002, 12, 12),
                ChuyenKhoa = "Thần kinh",
                LinkFace = ""
            }
        };

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Doctor(string hoTen, string chuyenKhoa, string diaChi)
        {
            var query = ListDoctor.AsQueryable(); // Sử dụng IQueryable để dễ dàng kết hợp các điều kiện

            if (!string.IsNullOrEmpty(hoTen))
            {
                query = query.Where(d => d.HoTen.ToLower().Contains(hoTen.ToLower()));
            }
            if (!string.IsNullOrEmpty(chuyenKhoa))
            {
                query = query.Where(d => d.ChuyenKhoa.ToLower().Contains(chuyenKhoa.ToLower()));
            }
            if (!string.IsNullOrEmpty(diaChi))
            {
                query = query.Where(d => d.DiaChi.ToLower().Contains(diaChi.ToLower()));
            }

            // Chuyển đổi kết quả cuối cùng thành danh sách
            var result = query.ToList();

            // Truyền toàn bộ danh sách vào ViewBag để sử dụng
            ViewBag.FullListDoctor = ListDoctor;

            return View(result);
        }
    }
}
