﻿using System.ComponentModel.DataAnnotations;

namespace WebPhongKham.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }

        //
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}
