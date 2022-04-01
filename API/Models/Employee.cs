using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public string NIK { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        [Required]
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public virtual Account Accounts { get; set; }
       

    }
    public enum Gender
    {
        Male,
        Female
    }
}
