using Microsoft.EntityFrameworkCore;

namespace WebPhongKham.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<DiaChi> DiaChis { get; set; }
        public DbSet<LoaiTinh> LoaiTinhs { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<TinTuc> TinTuc { get; set;}



    }
}
