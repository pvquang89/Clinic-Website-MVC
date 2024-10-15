using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPhongKham.Models
{
    public class AccountRole
    {
        [Key]
        public int AccountId { get; set; }
        [Key]
        public int RoleId { get; set; }

        //FK
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
