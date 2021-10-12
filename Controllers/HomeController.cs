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