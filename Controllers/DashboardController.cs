using LMU_EBurger.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LMU_EBurger.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        // EXPORTING THE ENTITY MODEL
        EBurgerAppDBEntities DB = new EBurgerAppDBEntities();

        // ==================== Categories ====================

        // GET : Create Category View
        [HttpGet]
        public ActionResult NewCategory(Category category)
        {
            return View();
        }

        // GET : CATEGORY LIST
        [HttpGet]
        public ActionResult Categories()
        {
            return View(DB.Categories.ToList());
        }

        // POST : Save Category Recordes To The Database Categories File
        [HttpPost]
        public ActionResult SaveCategory(Category category)
        {
            try
            {
                // STORING IMAGE IN TO THE LOCAL FORLDER & CREATING FILE PATH TO STORE IN THE DB
                string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                string extension = Path.GetExtension(category.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                category.Images = "/Assets/" + fileName;
                fileName = Path.Combine(Server.MapPath("/Assets/"), fileName);
                category.ImageFile.SaveAs(fileName);

                // SAVING DATA INTO THE CATERORY TABEL
                DB.Categories.Add(category);
                DB.SaveChanges();

                ModelState.Clear();

                // REDIRECTING TO THE CATEGORY LIST PAGE
                return RedirectToAction("Categories");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Categories Tabel", "Create"));
            }
        }

        // GET: Category/Edit/5
        public ActionResult EditCategory(int id)
        {
            using(DB)
            {
                return View(DB.Categories.Where(Category => Category.CategoryID == id).FirstOrDefault());
            }
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult EditCategory(int id, Category category)
        {
            try
            {
                // TODO: Add update logic here
                using(DB)
                {
                    // STORING IMAGE IN TO THE LOCAL FORLDER & CREATING FILE PATH TO STORE IN THE DB
                    string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                    string extension = Path.GetExtension(category.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    category.Images = "/Assets/" + fileName;
                    fileName = Path.Combine(Server.MapPath("/Assets/"), fileName);
                    category.ImageFile.SaveAs(fileName);

                    DB.Entry(category).State = EntityState.Modified;
                    DB.SaveChanges();
                }
                return RedirectToAction("Categories");
            }   
            catch
            {
                return View("EditCategory");
            }
        }

        // GET: Category/DeleteCategory/5
        public ActionResult DeleteCategory(int id)
        {
            using (DB)
            {
                return View(DB.Categories.Where(Category => Category.CategoryID == id).FirstOrDefault());
            }
        }

        // POST: Category/DeleteCategory/5
        [HttpPost]
        public ActionResult DeleteCategory(int id, Category category)
        {
            try
            {
                // TODO: Add update logic here`
                using (DB)
                {
                    Category category1 = DB.Categories.Where(Category => Category.CategoryID == id).FirstOrDefault();
                    DB.Categories.Remove(category1);
                    DB.SaveChanges();
                }
                return RedirectToAction("Categories");
            }
            catch
            {
                return View("Categories");
            }
        }

        // ==================== Menus ====================

        // GET : Menu LIST
        [HttpGet]
        public ActionResult Menus()
        {
            return View(DB.Menus.ToList());
        }

        // GET : Create Menu View
        [HttpGet]
        public ActionResult NewMenu(Menu menu)
        {
            var categorylist = DB.Categories.ToList();
            SelectList list = new SelectList(categorylist, "CategoryID", "Name");
            ViewBag.categorylistname = list;

            return View();
        }


        // POST : Save Menu Recordes To The Database Menus File
        [HttpPost]
        public ActionResult SaveMenu(Menu menu)
        {
            try
            {
                // STORING IMAGE IN TO THE LOCAL FORLDER & CREATING FILE PATH TO STORE IN THE DB
                string fileName = Path.GetFileNameWithoutExtension(menu.ImageFile.FileName);
                string extension = Path.GetExtension(menu.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                menu.Images = "/Assets/" + fileName;
                fileName = Path.Combine(Server.MapPath("/Assets/"), fileName);
                menu.ImageFile.SaveAs(fileName);

                // SAVING DATA INTO THE Menu TABEL
                DB.Menus.Add(menu);
                DB.SaveChanges();

                ModelState.Clear();

                // REDIRECTING TO THE MENU LIST PAGE
                return RedirectToAction("Menus");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Menus Tabel", "Create"));
            }
        }

        // GET: Category/Edit/5
        public ActionResult EditMenu(int id)
        {
            var categorylist = DB.Categories.ToList();
            SelectList list = new SelectList(categorylist, "CategoryID", "Name");
            ViewBag.categorylistname = list;

            using (DB)
            {
                return View(DB.Menus.Where(Menu => Menu.MenuID == id).FirstOrDefault());
            }
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult EditMenu(int id, Menu menu)
        {
            try
            {
                // TODO: Add update logic here
                using (DB)
                {
                    // STORING IMAGE IN TO THE LOCAL FORLDER & CREATING FILE PATH TO STORE IN THE DB
                    string fileName = Path.GetFileNameWithoutExtension(menu.ImageFile.FileName);
                    string extension = Path.GetExtension(menu.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    menu.Images = "/Assets/" + fileName;
                    fileName = Path.Combine(Server.MapPath("/Assets/"), fileName);
                    menu.ImageFile.SaveAs(fileName);

                    DB.Entry(menu).State = EntityState.Modified;
                    DB.SaveChanges();
                }
                return RedirectToAction("Menus");
            }
            catch
            {
                return View("EditMenu");
            }
        }

        // GET: Menu/DeleteCategory/5
        public ActionResult DeleteMenu(int id)
        {
            using (DB)
            {
                return View(DB.Menus.Where(Menu => Menu.MenuID == id).FirstOrDefault());
            }
        }

        // POST: Menu/DeleteCategory/5
        [HttpPost]
        public ActionResult DeleteMenu(int id, Menu menu)
        {
            try
            {
                // TODO: Add update logic here`
                using (DB)
                {
                    Menu menu1 = DB.Menus.Where(Menu => Menu.MenuID == id).FirstOrDefault();
                    DB.Menus.Remove(menu1);
                    DB.SaveChanges();
                }
                return RedirectToAction("Menus");
            }
            catch
            {
                return View("Menus");
            }
        }
    }
}