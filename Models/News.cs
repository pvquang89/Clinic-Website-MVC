using System.ComponentModel.DataAnnotations;

namespace WebPhongKham.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string? TomTat { get; set; }
        public string? HinhAnh { get; set; }
        public string NoiDung { get; set; }
        public DateTime ThoiGianDang { get; set; }
        public string TacGia { get; set; }
        public string? LinkSeo { get; set; }

    }
}
