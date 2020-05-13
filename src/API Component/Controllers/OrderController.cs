using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DefCan.Models;
using System.Data.Entity;
using System.Net.Http;


namespace DefCan.Controllers
{
    public class OrderController : Controller
    {
        

        // GET: Order/Index
        public ActionResult Index()
        {
            IEnumerable<Item> ItemList;
            HttpResponseMessage responseMessage = GlobalVariables.WebApiClient.GetAsync("Item").Result;
            ItemList = responseMessage.Content.ReadAsAsync<IEnumerable<Item>>().Result;

            return View(ItemList);
            // using (DbModelShop dbModelShops = new DbModelShop())
            //{


            // return View(dbModelShops.Items.ToList());
            //}
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
        public ActionResult Order(int id, Item item)
        {
            using (DbModelShop dbModelShops = new DbModelShop())
            {
                if (Session["Invoice"] == null)
                {
                    List<OrderedItem> Invoice = new List<OrderedItem>();
                    Invoice.Add(new OrderedItem(dbModelShops.Items.Find(id),1));
                    item.AmountPurchased++;//Testing if it works
                    
                    Session["Invoice"] = Invoice;
                }
                else
                {
                    List<OrderedItem> Invoice = (List<OrderedItem>)Session["Invoice"];
                    int index = isExisting(id);
                    if (index == -1)
                    {

                        Invoice.Add(new OrderedItem(dbModelShops.Items.Find(id), 1));
                        item.AmountPurchased++;
                    }
                    else
                    {
                        item.AmountPurchased++;
                        Invoice[index].Quantity++;
                        Session["Invoice"] = Invoice;
                    }
                }
               

                return View("invoice");
            }
        }

        public ActionResult Checkout(int id)
        {
            using (DbModelCustomers dbModelCustomers = new DbModelCustomers())

            //using (DbModelShop dbModelShops = new DbModelShop())
            {
                return View(dbModelCustomers.Customers.Where(x => x.CustomerID == id).FirstOrDefault());
            }
        } 
        [HttpPost]
        public ActionResult Checkout(int id, double grandTotal, float balance,Customer customer,Item item)
        {
            try
            {
                //double GrandTotal;

                //GrandTotal = grandTotal;
                balance = customer.Balance;
                //balance -= (float)grandTotal;
                //customer.Balance = balance;
                balance -= (float)grandTotal;
                var cust = new Customer() { CustomerID = id, Balance = balance };



                using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
                {
                    dbModelCustomer.Customers.Attach(cust);
                    dbModelCustomer.Entry(cust).Property(x => x.Balance).IsModified = true; //.State = EntityState.Modified;
                    dbModelCustomer.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {


                return View();
            }
        }
       // public ActionResult Search(DefCan.Models.Item item)
        //{
          //  using (DbModelShop dbModelShop = new DbModelShop())
           // {
             //   var ItemDetails = dbModelShop.Items.Where(x => x.Name == item.Name).FirstOrDefault();

               // if (ItemDetails == null)
                //{
                //    //item.LoginErrorMessage = "Wrong Username or Password.";
                  //  return View("Search", item);
                  //  //RedirectToAction("Autherize");
                //}
                //else
                //{

                  //  Session["CustomerID"] = item.ItemID;
                    //return View(dbModelShop.Items.ToList());
                    //return RedirectToAction("Index", "Item"); //CHange this to where u wanna redirect them when it's right
                //}


                //return View(dbModelShop.Items.Where(x => x.Name == item.Name).FirstOrDefault());
                
            //}

        //}


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
