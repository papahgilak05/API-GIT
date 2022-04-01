using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public string NIK { get; set; }
        public string password { get; set; }
        public int OTP { get; set; }
        public DateTime ExpiredToken { get; set; }
        public bool IsUsed { get; set; }
        public virtual Employee Employees { get; set; }
        public virtual Profiling Profilings { get; set; }
        public virtual ICollection<AccountRole> AccountRole { get; set; }
    }
}
