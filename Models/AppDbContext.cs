using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FirstTasks.Models
{
    public class AppDbContext:DbContext
    {

        public AppDbContext() : base("ConString") { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Login> Login {  get; set; }

    }
}