using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstTasks.Models
{
    public class SetPassword
    {
        [Required(ErrorMessage = "Entering Generated Password is Mandatory !")]
        public string oldpwd { get; set; }
        [Required(ErrorMessage = "Creation of New Password is Mandatory !")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,20}$",
    ErrorMessage = "Password must be 6-20 characters, include uppercase, lowercase, number, and special character")]
        public string newpwd { get; set; }
        [Required(ErrorMessage = "Confirming Password is Mandatory !")]
        [Compare("newpwd", ErrorMessage = "Passwords are not Matching !")]
        public string cpwd { get; set; }
        public string Email { get; set; }

    }
}