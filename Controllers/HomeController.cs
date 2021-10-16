using LMU_EBurger.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace LMU_EBurger.Controllers
{
    public class HomeController : Controller
    {
        private readonly EBurgerAppDBEntities DB = new EBurgerAppDBEntities();


        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();
            model.Categories = GetCategories();
            model.Menus = GetMenus();
            return View(model);
        }

        private List<Menu> GetMenus()
        {
            List<Menu> menuList = DB.Menus.Take(6).ToList();
            return menuList;
        }

        private List<Category> GetCategories()
        {
            List<Category> categoryList = DB.Categories.ToList();
            return categoryList;
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }


        public ActionResult Menu()
        {
            dynamic menumodel = new ExpandoObject();
            menumodel.Categories = GetCategories();
            menumodel.Menus = GetMenus();
            return View(menumodel);
        }

        public ActionResult MenuDetails(int id)
        {
            using (DB)
            {
                return View(DB.Menus.Where(Menu => Menu.MenuID == id).FirstOrDefault());
            }
        }

        // Customer Registration Page View Controller Function
        public ActionResult Registration()
        {
            return View();
        }

        // POST : Save Category Recordes To The Database Categories File
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult NewRegistration(Customer customer)
        {
            try
            {
                if (customer.Password == customer.confirmPassword)
                {
                    User user = new User
                    {
                        Username = customer.Username,
                        Password = customer.Password,
                        AccessLevel = "Customer",
                        ProfileImage = "/Content/img/avatar1.jpg"
                    };

                    // Save User Admin as a user in the user table
                    DB.Users.Add(user);
                    DB.SaveChanges();

                    // Getting the user id of the newly created user
                    int latestUserId = user.UserID;

                    Customer customer1 = new Customer
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Phone = customer.Phone,
                        FullAddress = customer.FullAddress,
                        District = customer.District,
                        City = customer.City,
                        ZipCode = customer.ZipCode,
                        UserId = latestUserId
                    };

                    // Save Admin User Into AdminUser Tabel
                    DB.Customers.Add(customer1);
                    DB.SaveChanges();

                    // Createing a session values with the user details
                    Session["UserId"] = user.UserID.ToString();
                    Session["Username"] = user.Username;
                    Session["AccessLevel"] = user.AccessLevel;

                    if (Session["AccessLevel"].Equals("Customer"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        if (Session["AccessLevel"].Equals("Admin"))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Registration");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Registration");
                }

            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Customer Tabel", "Create"));
            }
        }

        private void elseif()
        {
            throw new NotImplementedException();
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
                    User obj = DB.Users.Where(model => model.Username.Equals(objUser.Username) && model.Password.Equals(objUser.Password)).FirstOrDefault();

                    if (obj != null)
                    {
                        Session["UserId"] = obj.UserID.ToString();
                        Session["Username"] = obj.Username;
                        Session["AccessLevel"] = obj.AccessLevel;

                        if (Session["AccessLevel"].Equals("Customer"))
                        {
                            return RedirectToAction("Index", "Home");
                        }

                        if (Session["AccessLevel"].Equals("Admin"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }
                }
                return View(objUser);
            }

            return View();
        }
    }
}