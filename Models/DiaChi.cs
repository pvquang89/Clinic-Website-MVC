using System.ComponentModel.DataAnnotations.Schema;

namespace WebPhongKham.Models
{
    public class DiaChi
    {
        public int Id { get; set; }
        public string TenTinh { get; set; }

        //FK
        public int? idLoaiTinh { get; set; }

        //navigation properties
        [ForeignKey("idLoaiTinh")]
        public LoaiTinh LoaiTinh { get; set; }

    }
}
