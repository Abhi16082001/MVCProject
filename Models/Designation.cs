using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FirstTasks.Models
{
    [Table("Designation")]
    public class Designation
    {
        [Key]
        public decimal DsgID { get; set; }
        public string Dsg_name { get; set; }
    }
}