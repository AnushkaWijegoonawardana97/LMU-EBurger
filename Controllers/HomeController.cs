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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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
                using (EBurgerAppDBEntities db = new EBurgerAppDBEntities())
                {
                    var obj = db.Users.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)).FirstOrDefault();

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