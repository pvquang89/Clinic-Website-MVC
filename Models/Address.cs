using System.ComponentModel.DataAnnotations.Schema;

namespace WebPhongKham.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string TenTinh { get; set; }

        //FK
        public int? idLoaiTinh { get; set; }

        //navigation properties
        [ForeignKey("idLoaiTinh")]
        public ProvinceType LoaiTinh { get; set; }

    }
}
