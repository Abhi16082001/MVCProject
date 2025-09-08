using FirstTasks.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstTasks.Controllers
{
    public class EmployeeController : Controller
    {
        
        // GET: Employee
        public ActionResult AddEmp()
        {
            using (var db = new AppDbContext())
            {
                ViewBag.Departments = db.Departments.ToList();
                ViewBag.Designations = db.Designations.ToList(); }
            return View("AddEmp");
        }

        public ActionResult EditEmp(int id)
        {
            using (var db = new AppDbContext())
            {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.Designations = db.Designations.ToList();
                var employee = db.Employees.FirstOrDefault(e => e.Eid == id);
                if (employee == null) return HttpNotFound();
                employee.Eid = id;

                return View("EditEmp",employee);
            }
        }


        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString))

                {
                    string manager = Session["ManagerPhone"] as string;

                    con.Open();

                    SqlCommand cmd = new SqlCommand("ManageEmployee", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action","INSERT");
                    cmd.Parameters.AddWithValue("@Name", emp.Name);
                    cmd.Parameters.AddWithValue("@Gender", emp.Gender);
                    cmd.Parameters.AddWithValue("@Email", emp.Email);
                    cmd.Parameters.AddWithValue("@Phone", emp.Phone);
                    cmd.Parameters.AddWithValue("@DOB", emp.DOB);
                    cmd.Parameters.AddWithValue("@City", emp.City);
                    cmd.Parameters.AddWithValue("@Department", emp.Department);
                    cmd.Parameters.AddWithValue("@Designation", emp.Designation);
                    cmd.Parameters.AddWithValue("@Manager", manager);


                    cmd.ExecuteNonQuery();
                    con.Close();

                }
                return Json(new { status = true, message = "Inserted Successfully" });
            }
            else
            {
                using (var db = new AppDbContext())
                {
                    ViewBag.Departments = db.Departments.ToList();
                    ViewBag.Designations = db.Designations.ToList();
                } 
                return Json(new { status = false, message = "Validation Errors !!" });
            }


        }


        public ActionResult Logout()
        {
            Session.Remove("UserData");
            Session.Remove("ManagerPhone");
            Session.Remove("Count");
            Session.Remove("Departments");
            Session.Remove("Designations");
            return RedirectToRoute(new { controller = "Home", action = "Login" });
        }




        [HttpPost]
        public ActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString))

                {                   
                    con.Open();

                    SqlCommand cmd = new SqlCommand("ManageEmployee", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@Name", emp.Name);
                    cmd.Parameters.AddWithValue("@Gender", emp.Gender);
                    cmd.Parameters.AddWithValue("@Email", emp.Email);
                    cmd.Parameters.AddWithValue("@Phone", emp.Phone);
                    cmd.Parameters.AddWithValue("@DOB", emp.DOB);
                    cmd.Parameters.AddWithValue("@City", emp.City);
                    cmd.Parameters.AddWithValue("@Department", emp.Department);
                    cmd.Parameters.AddWithValue("@Designation", emp.Designation);
                    cmd.Parameters.AddWithValue("@Eid", emp.Eid);


                    cmd.ExecuteNonQuery();
                    con.Close();

                }
                return Json(new { status = true, message = "Updated Successfully" });
            }
            else
            {
                using (var db = new AppDbContext())
                {
                    ViewBag.Departments = db.Departments.ToList();
                    ViewBag.Designations = db.Designations.ToList();
                }
                return Json(new { status = false, message = "Validation Errors !!" });
            }


        }




 
        public ActionResult Delete(int id)
        {
           
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString))

                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("ManageEmployee", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@Eid", id);
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            return Json(new { status = true, message = "Deleted Successfully" });



        }


       public PartialViewResult LoadMore(string Phone)
        {
            
            int n = Convert.ToInt32(Session["rows"])+1;
            Session["rows"] = n;
            using (var db = new AppDbContext()) {

                var emp= db.Employees.Where(e => e.Manager == Phone).Take(n).ToList();
                return PartialView("Pview",emp);

            }
        }


    }
}
 /*

  */