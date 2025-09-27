using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace FirstTasks.Models
{
    [Table("Employee")]
    public class Employee
    {
        public int Eid { get; set; }

        [Required(ErrorMessage = "Full Name is required !")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone is required !")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Number must have 10 digits only !")]
        public string Phone { get; set; }
        [Key]
        [Required(ErrorMessage = "Email is required !")]
        [EmailAddress(ErrorMessage = "Invalid Email Address !")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Gender is required !")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "City is required !")]
        public string City { get; set; }
        [Required(ErrorMessage = "DOB is required !")]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Designation is required !")]
        public string Designation {  get; set; }
        [Required(ErrorMessage = "Department is required !")]
        public string Department { get; set; }

        public string Manager { get; set; }

    }

    public class Employee2
    {
        [Display(Name = "Enter Date")]
        public DateTime EnterDate { get; set; }
    }
}