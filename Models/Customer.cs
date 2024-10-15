using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPhongKham.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Họ tên")]
        [Required(ErrorMessage ="Chưa nhập họ tên ")]
        public string HoTen { get; set; }

        [DisplayName("Ngày sinh")]
        [Required(ErrorMessage = "Chưa nhập ngày sinh ")]
        public DateTime? NgaySinh { get; set; }

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Chưa nhập số điện thoại ")]
        public string SoDienThoai { get; set; }

        [DisplayName("Số tiền")]
        public float? SoTien { get; set; }

        public bool? Active { get; set; }

        //FK
        public int? idDiaChi { get; set; }

        //nav properties
        //references properties
        [ForeignKey("idDiaChi")] //tức khoá ngoại idDiaChi ở bảng này là khoá ở bảng DiaChi
        [DisplayName("Địa chỉ")]
        public Address? DiaChi { get; set; }

        public List<Order>? DonHangs { get; set; }

    }
}
