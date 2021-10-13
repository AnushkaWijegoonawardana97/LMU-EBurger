using LMU_EBurger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMU_EBurger.Controllers
{
    public class HomeController : Controller
    {
        EBurgerAppDBEntities DB = new EBurgerAppDBEntities();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }


        // Customer Registration Page View Controller Function
        public ActionResult Registration()
        {
            return View();
        }

        // POST : Save Category Recordes To The Database Categories File
        [HttpPost]
        public ActionResult NewRegistration(Customer customer)
        {
            try
            {
                User user = new User();
                user.Username = customer.Username;
                user.Password = customer.Password;
                user.AccessLevel = "Customer";
                user.ProfileImage = "/Content/img/avatar1.jpg";

                // Save User Admin as a user in the user table
                DB.Users.Add(user);
                DB.SaveChanges();

                // Getting the user id of the newly created user
                int latestUserId = user.UserID;

                Customer customer1 = new Customer();
                customer1.FirstName = customer.FirstName;
                customer1.LastName = customer.LastName;
                customer1.Email = customer.Email;
                customer1.Phone = customer.Phone;
                customer1.Phone = customer.FullAddress;
                customer1.Phone = customer.District;
                customer1.Phone = customer.City;
                customer1.Phone = customer.ZipCode;
                customer1.UserId = latestUserId;

                // Save Admin User Into AdminUser Tabel
                DB.Customers.Add(customer1);
                DB.SaveChanges();

                // REDIRECTING TO THE CATEGORY LIST PAGE
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Customer Tabel", "Create"));
            }
        }

        //Login Page View Controller Function
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
            if (ModelState.IsValid)
            {
                using (DB)
                {
                    var obj = DB.Users.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)).FirstOrDefault();

                    if (obj != null)
                    {
                        Session["UserId"] = obj.UserID.ToString();
                        Session["Username"] = obj.Username.ToString();

                        return RedirectToAction("Index");
                    }
                }
                return View(objUser);
            }

            return View();
        }
    }
}