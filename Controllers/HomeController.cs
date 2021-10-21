using LMU_EBurger.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;


namespace LMU_EBurger.Controllers
{
    public class HomeController : Controller
    {
        private readonly EBurgerAppDBEntities1 DB = new EBurgerAppDBEntities1();

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

        public ActionResult Order()
        {
            if (Session["UserId"] != null && Session["AccessLevel"].Equals("Customer"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        [HttpPost]
        [ValidateInput(true)]
        public ActionResult CreateNewOrder(Order order)
        {
            
            Order order1 = new Order
            {
                OrderType = order.OrderType,
                CreateAt = @DateTime.Today.ToString("D"),
                SubTotal = "580",
                DeliveryFee = "100",
                Total = "600",
                OrderStatus = "Order Confirmed",
                CustomerID = order.CustomerID,
                MenuItems = "1 x 2 |",
            };
            DB.Orders.Add(order1);
            DB.SaveChanges();

            OrderDelivery orderDelivery = new OrderDelivery
            {
                OrderID = order1.OrderID,
                FullAddress = order.FullAddress,
                District = order.District,
                City = order.City,
                ZIpCode = order.ZipCode,
            };
            DB.OrderDeliveries.Add(orderDelivery);
            DB.SaveChanges();

            Session["Cart"] = null;

            return RedirectToAction("OrderConfirmed", "Home");
        }

        // GET : CATEGORY LIST
        [HttpGet]
        public ActionResult OrderHistroy()
        {
            return View(DB.Orders.ToList());
        }

        public ActionResult OrderConfirmed()
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

        // Add Menu Items To Cart
        public ActionResult AddToCart(int MenuID)
        {
            if(Session["Cart"] == null)
            {
                List<CartItem> cartItems = new List<CartItem>();
                cartItems.Add(new CartItem { Menu = DB.Menus.Where(Menu => Menu.MenuID == MenuID).FirstOrDefault(), Quantity = 1  });
                Session["Cart"] = cartItems;
            }
            else
            {
                List<CartItem> cartItems = (List<CartItem>)Session["Cart"];
                int index = isExist(MenuID);
                if(index != -1)
                {
                    cartItems[index].Quantity++;
                } else
                {
                    cartItems.Add(new CartItem { Menu = DB.Menus.Where(Menu => Menu.MenuID == MenuID).FirstOrDefault(), Quantity = 1 });
                }
                Session["Cart"] = cartItems;
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveFromCart(int MenuID)
        {
            List<CartItem> cartItems = (List<CartItem>)Session["Cart"];
            int index = isExist(MenuID);
            cartItems.RemoveAt(index);
            Session["Cart"] = cartItems;
            return Redirect(Request.UrlReferrer.ToString());
        }

        private int isExist(int MenuID)
        {
            List<CartItem> cartItems = (List<CartItem>)Session["Cart"];
            for (int i = 0; i < cartItems.Count; i++)
                if (cartItems[i].Menu.MenuID.Equals(MenuID))
                    return i;
            return -1;
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
                        Session["CustomerID"] = DB.Customers.Where(Customer => Customer.UserId.Equals(user.UserID)).FirstOrDefault().CustomerID;
                    }


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
                        if(Session["AccessLevel"].Equals("Customer"))
                        {
                            Session["CustomerID"]  = DB.Customers.Where(Customer => Customer.UserId.Equals(obj.UserID)).FirstOrDefault().CustomerID;
                        }

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

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }
    }
}