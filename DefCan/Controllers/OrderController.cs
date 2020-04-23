using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DefCan.Models;
using System.Data.Entity;

namespace DefCan.Controllers
{
    public class OrderController : Controller
    {
        

        // GET: Order/Index
        public ActionResult Index()
        {
            using (DbModelShop dbModelShops = new DbModelShop())
            {


                return View(dbModelShops.Items.ToList());
            }
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        

        private int isExisting(int id)
        {
            List<OrderedItem> Invoice = (List<OrderedItem>)Session["Invoice"];
            for (int i = 0; i < Invoice.Count; i++)
            {
                if (Invoice[i].Item.ItemID == id)
                {
                    return i;
                }
            }    
                return -1;
            
        }


        public ActionResult Delete(int id)
        {
            int index = isExisting(id);
            List<OrderedItem> Invoice = (List<OrderedItem>)Session["Invoice"];
            Invoice.RemoveAt(index);
            Session["Invoice"] = Invoice;
            return View("Invoice");
        }


        // GET: Order/Invoice
        public ActionResult Order(int id)
        {
            using (DbModelShop dbModelShops = new DbModelShop())
            {
                if (Session["Invoice"] == null)
                {
                    List<OrderedItem> Invoice = new List<OrderedItem>();
                    Invoice.Add(new OrderedItem(dbModelShops.Items.Find(id),1));
                    Session["Invoice"] = Invoice;
                }
                else
                {
                    List<OrderedItem> Invoice = (List<OrderedItem>)Session["Invoice"];
                    int index = isExisting(id);
                    if (index == -1)
                    {
                        Invoice.Add(new OrderedItem(dbModelShops.Items.Find(id), 1));
                    }
                    else
                    {
                        Invoice[index].Quantity++;
                        Session["Invoice"] = Invoice;
                    }
                }
                return View("invoice");
            }
        }

        public ActionResult Checkout(int id)
        {


            using (DbModelShop dbModelShops = new DbModelShop())
            {
                return View(dbModelShops.Items.Where(x => x.ItemID == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Checkout(double grandTotal, float balance,Customer customer,Item item)
        {
            try
            {

                customer.Balance = customer.Balance - (float)grandTotal;

                using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
                {
                    dbModelCustomer.Entry(customer).State = EntityState.Modified;
                    dbModelCustomer.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {


                return View();
            }
        }
    }
    public class OrderedItem
    {
        private Item item = new Item();
        private int quantity;

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public OrderedItem()
        {

        }

        public OrderedItem(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }

}
