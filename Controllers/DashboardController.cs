using LMU_EBurger.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMU_EBurger.Controllers
{
    public class DashboardController : Controller
    {
        EBurgerAppDBEntities DB = new EBurgerAppDBEntities();
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

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
                // TODO: Add update logic here
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
    }
}