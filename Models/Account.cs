using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPhongKham.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Chưa điền tên người dùng")]
        public string TenTaiKhoan { get; set; }
        [Required(ErrorMessage = "Chưa điền mật khẩu")]
        public string MatKhau { get; set; }
        [Required(ErrorMessage = "Chưa điền họ tên đầy đủ")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Chưa điền số điện thoại")]
        public string SoDienThoai { get; set; }
        public string? HinhAnh { get; set; }

        [Required(ErrorMessage = "Chưa nhập xác nhận mật khẩu")]
        [NotMapped]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu nhập lại chưa khớp")]
        public string XacNhanMatKhau { get; set; }

        //
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}
