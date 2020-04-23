using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DefCan.Models;
using System.Data.Entity;

namespace DefCan.Controllers
{
    public class CustomerController : Controller
    {

        

        // GET: Customer/Index
        public ActionResult Index()
        {
            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                return View(dbModelCustomer.Customers.ToList());
            }
            
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                return View (dbModelCustomer.Customers.Where(x => x.CustomerID == id).FirstOrDefault());
            }

            
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {



                using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
                {

                    if (dbModelCustomer.Customers.Any(x => x.EmailAddress == customer.EmailAddress))
                    {
                        ViewBag.DuplicateMessage = "Email Address already exist";
                        return View("Create", customer);
                    }

                    dbModelCustomer.Customers.Add(customer);
                    dbModelCustomer.SaveChanges();
                }
                // TODO: Add insert logic here
               // ModelState.Clear();
               // ViewBag.SuccessMessage = "Registration Successful";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                return View(dbModelCustomer.Customers.Where(x => x.CustomerID == id).FirstOrDefault());
            }
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Customer customer)
        {
            try
            {
                using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
                {
                    dbModelCustomer.Entry(customer).State = EntityState.Modified;
                    dbModelCustomer.SaveChanges();
                }

                    // TODO: Add update logic here

                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                return View(dbModelCustomer.Customers.Where(x => x.CustomerID == id).FirstOrDefault());
            }
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using(DbModelCustomers dbModelCustomer = new DbModelCustomers())
                {
                    Customer customer = dbModelCustomer.Customers.Where(x => x.CustomerID == id).FirstOrDefault();
                    dbModelCustomer.Customers.Remove(customer);
                    dbModelCustomer.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Autherize(DefCan.Models.Customer CustomerModel)
        {
            
                using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
                {
                    var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.Password == CustomerModel.Password).FirstOrDefault();
                    if (CustomerDetails == null)
                    {
                        CustomerModel.LoginErrorMessage = "Wrong Username or Password.";
                        return View("Autherize", CustomerModel);
                        //RedirectToAction("Autherize");
                    }
                    else
                    {
                        Session["CustomerID"] = CustomerDetails.CustomerID;
                        return RedirectToAction("Index", "Home"); //CHange this to where u wanna redirect them when it's right
                    }



                }
            
           
        }

    }
}
