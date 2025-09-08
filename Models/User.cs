using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FirstTasks.Models
{
    public class User
    {
        [Required(ErrorMessage = "First Name is required !")]
        public string Fname { get; set; }

        public string Lname { get; set; }
        [Required(ErrorMessage = "Phone is required !")]
        [StringLength(10,MinimumLength =10,ErrorMessage ="Mobile Number must have 10 digits only !")]
        public string Phone { get; set; }
        [Key]
        [Required(ErrorMessage ="Email is Required !")]
        [EmailAddress(ErrorMessage ="Invalid Email Address !")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Gender is Required !")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "City is Required !")]
        public string City { get; set; }
        [Required(ErrorMessage = "DOB is Required !")]
        public DateTime DOB { get; set; }
        public string Profile { get; set; }

    }
}