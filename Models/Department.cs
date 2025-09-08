using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FirstTasks.Models
{
    [Table("Department")]
    public class Department
    {
     
        [Key]
        public decimal Did { get; set; }
        public string Dname { get; set; }
    }
}