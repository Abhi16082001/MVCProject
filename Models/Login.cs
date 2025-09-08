using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FirstTasks.Models
{
    [Table("Login")]
    public class Login
    {
     [Key]
        [Required(ErrorMessage = "Username is Required !")]
        public string Username {  get; set; }
        [Required(ErrorMessage = "Password is Required !")]
        public string Password { get; set; }    
    }
}