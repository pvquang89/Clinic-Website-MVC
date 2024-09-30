using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPhongKham.Models
{
    public class DonHang
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Thời gian")]
        public DateTime NgayDatHang { get; set;}
        [DisplayName("SĐT")]
        public string DienThoai { get; set; }
        [DisplayName("Thông tin")]
        public string ThongTinSanPham { get; set; }
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }
        [DisplayName("Tổng tiền")]
        public float TongTienSanPham { get; set; }
        [DisplayName("Ship")]
        public float PhiShip { get; set; }

        // Thuộc tính Avatar lưu đường dẫn hình ảnh
        [DisplayName("Hình Ảnh")]
        public string? HinhAnh { get; set; }

        //FK
        public int idKhachHang { get; set; }

        //
        [ForeignKey("idKhachHang")]
        public KhachHang? KhachHang { get; set;}


    }
}
