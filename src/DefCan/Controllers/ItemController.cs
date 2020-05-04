using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DefCan.Models;
using System.Data.Entity;

namespace DefCan.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item/Index
        public ActionResult Index()
        {

            using (DbModelShop dbModelShops = new DbModelShop())
            {
                return View(dbModelShops.Items.ToList());
            }

            
        }

        // GET: Item/Details/5
        public ActionResult Details(int id)
        {
            using(DbModelShop dbModelShops = new DbModelShop())
            {
                return View(dbModelShops.Items.Where(x => x.ItemID == id).FirstOrDefault());
            }

            
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(Item item, HttpPostedFileBase file, HttpPostedFileBase file2)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + file.FileName);
                    item.PicUrl = file.FileName;
                    
                }

                if (file2 != null)
                {
                    file2.SaveAs(HttpContext.Server.MapPath("~/Images/") + file2.FileName);
                    item.AslPicUrl = file2.FileName;
                }

                using (DbModelShop dbModelShops = new DbModelShop())
                {
                    dbModelShops.Items.Add(item);
                    dbModelShops.SaveChanges();
                    //return RedirectToAction("Index");
                }
                // }
                //using (DbModelShop dbModelShops = new DbModelShop())
                //{
                //  dbModelShops.Items.Add(item);
                //dbModelShops.SaveChanges();
                //}
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View(item);
            }
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {
           

            using (DbModelShop dbModelShops = new DbModelShop())
            {
                return View(dbModelShops.Items.Where(x => x.ItemID == id).FirstOrDefault());
            }
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Item item, HttpPostedFileBase file, HttpPostedFileBase file2)
        {
            try
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + file.FileName);
                    item.PicUrl = file.FileName;
                }

                if (file2 != null)
                {
                    file2.SaveAs(HttpContext.Server.MapPath("~/Images/") + file2.FileName);
                    item.AslPicUrl = file2.FileName;
                }

                using (DbModelShop dbModelShops = new DbModelShop())
                {
                    dbModelShops.Entry(item).State = EntityState.Modified;
                    dbModelShops.SaveChanges();
                }

                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View(item);
            }
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            using (DbModelShop dbModelShops = new DbModelShop())
            {
                return View(dbModelShops.Items.Where(x => x.ItemID == id).FirstOrDefault());
            }
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (DbModelShop dbModelShops = new DbModelShop())
                {
                    Item item = dbModelShops.Items.Where(x => x.ItemID == id).FirstOrDefault();
                    dbModelShops.Items.Remove(item);
                    dbModelShops.SaveChanges();
                }


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

 
    }
}
