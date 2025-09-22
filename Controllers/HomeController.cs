using FirstTasks.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FirstTasks.Controllers
{
    public class HomeController : Controller
    {


        private string conString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        // GET: Home
        public ActionResult Index()
        {
            SqlConnection.ClearAllPools();
            return View("View");
        }

        public ActionResult Login()
        {
            return View("Login");
        }
        public ActionResult TestValidation()
        {
            return View("TestValidation");
        }


        public ActionResult Register()
        {
            return View("Register");
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            using (var db = new AppDbContext())
            {
                Session["rows"] = 1;
                var user = Session["UserData"] as User;
                var employees2 = db.Employees.Where(e => e.Manager == user.Phone).Take(1).ToList();
                var employees = db.Employees.Where(e => e.Manager == user.Phone).ToList();
                Dictionary<string,int> counts=new Dictionary<string, int>();
                counts.Add("DeptCount",db.Departments.Count());
                counts.Add("DesgCount", db.Designations.Count());
                counts.Add("TotEmpCount", db.Employees.Count());
                counts.Add("EmpCount",employees.Count());
                Session["Count"] = counts; Session["Departments"] = db.Departments.Select(e=>e.Dname).ToList();
                Session["Designations"]=db.Designations.Select(e=>e.Dsg_name).ToList();
                var vm = new DashModel
            {
                User = user,
                Employees = employees2
            };
            return View("Dashboard",vm);
            }
        }

        public ActionResult ChangePwd()
        {
            return View("ChangePwd");
        }

        public ActionResult VerifyEmail()
        {
            return View("VerifyEmail");
        }


        [HttpPost]
        public ActionResult SendOTP(User user, HttpPostedFileBase profile)
        {
            Session["UserData"] = user;
            var random = new Random();
            string otp = random.Next(100000, 999999).ToString();
  
            try
            {
                if (ModelState.IsValid)
                {
                    Session["OTP"] = otp;
                    if (profile != null && profile.ContentLength > 0) {

                        string uploadDir = Server.MapPath("~/Profiles");
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        string fileName = user.Fname+user.Phone+ Path.GetExtension(profile.FileName);
                        string filePath = Path.Combine(uploadDir, fileName);
                        profile.SaveAs(filePath);
                        user.Profile = "/Profiles/" + fileName;

                    }
                    SendEmail(user.Email, otp);
                    return Json(new { status = true, message = "OTP sent to your Email for Verification !" });
                }


                return Json(new { status = false, message = "Validation Errors !!" });

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = $"Error Sending OTP : {ex.Message} !!" });
              
            }

        }



      
        public bool GenerateCred()
        {
            User user =Session["UserData"] as User;
            string Phone = user.Phone;
            string Email = user.Email;
            var random = new Random();
            string pwd = user.Fname.Substring(0, 3) + user.Lname.Substring(0, 3) +"@"+ Phone.Substring(0,5);
            Session["OldPwd"] = pwd;
            try
            {                         
                    SendCred(Email,Phone,pwd);
                return true;                      
            }
            catch (Exception ex)
            {
                return false;
                
            }

        }


        private void SendEmail(string toemail, string otp)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("abhishekrajput16082001@gmail.com", "pmhkwmjxmzuydmst"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("abhishekrajput16082001@gmail.com"),
                Subject = "Your OTP Code",
                Body = $"Your OTP is: {otp}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(toemail);
            smtpClient.Send(mailMessage);
        }



        private void SendCred(string toemail, string Phone, string Pwd)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("abhishekrajput16082001@gmail.com", "pmhkwmjxmzuydmst"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("abhishekrajput16082001@gmail.com"),
                Subject = "Your Login Credentials :",
                Body = $"You are successfully registered ! \n These are your credentials: \n UserID: {Phone}\nPassword: {Pwd}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(toemail);
            smtpClient.Send(mailMessage);
        }


        [HttpPost]
        public ActionResult VerifyOTP(string otp)
        {
            try
            {
                string StoredOtp = Session["OTP"] as string;
                int NumStoredOtp = Convert.ToInt32(StoredOtp);
                int NumEnteredOtp = Convert.ToInt32(otp.Trim());
                if (NumStoredOtp == NumEnteredOtp)
                {
                    var user = Session["UserData"] as User;
                    if (user != null)
                    {
                        using (var db = new AppDbContext())
                        {
                            var login = new Login
                            {
                                Username = user.Phone,
                                Password = "dummy"
                            };
                            db.Login.Add(login);
                            db.Users.Add(user);
                            db.SaveChanges();
                        }
                    }

                    bool status = GenerateCred();
                    if (status)
                    {
                        Session.Remove("OTP");
                        return Json(new { status = true, message = "Email Verification Done ! \n Your Credentials sent to your Email !" });
                    }
                    else return Json(new { status = false, message = "Email Verification Done ! \n But some error occured while sending Credentials to your Email !" });
                }

                else
                {
                    return Json(new { status = false, message = "Invaid OTP !" });

                }
            }
            catch(Exception ex)
            {
                return Json(new { status = false, message = $"Exception Occured : {ex.Message} !" });
            }
            
        }


        [HttpPost]
        public ActionResult VerifyPwd(SetPassword sp )
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    string opwd = Session["OldPwd"] as string;

                    if (ModelState.IsValid)
                    {
                        User user = Session["UserData"] as User;
                        var email = user.Email.Trim();
                        var usr = db.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());


                        var lgn= db.Login.FirstOrDefault(u => u.Username == usr.Phone);

                        if (usr != null)
                        { 
                            if (sp?.oldpwd == opwd) // compare old with system-generated one
                            {
                                lgn.Password = sp.newpwd;
                                db.SaveChanges();                              

                                Session.Remove("OldPwd");
                                Session["UserData"] = usr;
                                Session["ManagerPhone"] = usr.Phone;

                                return Json(new { status = true, message = "Password Changed \n Registration Done Successfylly !" });
                            }

                            return Json(new { status = false, message = "Your Old Password and System Generated Password\n are not matching  !" });
                        }

                       return Json(new { status = false, message = "User Not Found !" });
                    }
                    else
                    {
                        return Json(new { status = false, message = "Validatin Errors !" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = $"Error: {ex.Message} !" });
            }

        }


        

        public User Getuser(string phone)
        {
            User user = null;

            using (SqlConnection con = new SqlConnection(conString))

            {
                string query = "SELECT Fname,Lname,Email,Gender,City,DOB,Profile FROM Users WHERE Phone = @phone";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@phone", phone);

                con.Open();
                using(SqlDataReader dr = cmd.ExecuteReader()) {

                    if (dr.Read())
                    {
                        user = new User
                        {
                            Fname = dr["Fname"].ToString(),
                            Lname = dr["Lname"].ToString(),
                            Email = dr["Email"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            City = dr["City"].ToString(),
                            DOB = Convert.ToDateTime(dr["DOB"]),
                            Profile = dr["Profile"].ToString(),
                            Phone = phone

                        };

                    }
                }
                con.Close();
            }
           

            return user;
        }

        public string Getpass(string usrname)
        {
            string Password="";
            using (SqlConnection con = new SqlConnection(conString))

            {
                string query = "SELECT Password FROM Login WHERE Username = @usrname";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@usrname", usrname);
   Password = cmd.ExecuteScalar().ToString();                                                              
                con.Close();
            }
            return Password;
        }

        [HttpPost]
        public ActionResult Authentication(Login lgn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = Getuser(lgn.Username);


                    if (user != null)
                    {
                    string fpss = Getpass(lgn.Username);
                        if (fpss == lgn.Password)
                        {
                            Session["ManagerPhone"] = user.Phone;
                            Session["UserData"] = user;

                            FormsAuthentication.SetAuthCookie(lgn.Username, false);
                            return Json(new { status = true, message = "Logged In Successfully !!" });


                        }
                        else
                        {
                            return Json(new { status = false, message = "User Found but Password is Wrong !!" });

                        }
                    }
                    else
                    {
                        return Json(new { status = false, message = "User doesn't Exist!!" });

                    }
                }
                else
                {
                    return Json(new { status = false, message = "Validation Errors !!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = $"Exception : {ex.Message} !!" });
            }
        }


    }
    }
