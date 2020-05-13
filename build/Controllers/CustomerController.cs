using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DefCan.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net.Http;

namespace DefCan.Controllers
{
    public class CustomerController : Controller
    {

        
        [Authorize]
        // GET: Customer/Index
        public ActionResult Index()
        {
            IEnumerable<Customer> CustList;
            HttpResponseMessage ResponseMessage = GlobalVariables.WebApiClient.GetAsync("Customer").Result;
            CustList = ResponseMessage.Content.ReadAsAsync<IEnumerable<Customer>>().Result;
            return View(CustList);
            //using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            //{

              //  return View(dbModelCustomer.Customers.ToList());
            //}
            
        }

   
        // Post: Customer/Autherize
        //[HttpPost]
        public ActionResult Autherize(DefCan.Models.Customer CustomerModel)//Password Log in Verification
        {
            Session.Clear();
            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.Password == CustomerModel.Password).FirstOrDefault();


               // var ManagerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == "ADMIN1" && x.Password == "IHEARCUP$1").FirstOrDefault();

                //if (ManagerDetails == null)
                //{
                  //  if (CustomerDetails == null)
                    //{
                      //  CustomerModel.LoginErrorMessage = "Wrong Username or Password.";
                        //return View("Autherize", CustomerModel);
                        //RedirectToAction("Autherize");
                   // }
                    //else
                    //{

                      //  Session["CustomerID"] = CustomerDetails.CustomerID;
                        //return RedirectToAction("Index", "Order"); //CHange this to where u wanna redirect them when it's right
                    //}
                //}
                //else
                //{
                  //  return RedirectToAction("Index", "Customer");
                //}
                //if( CustomerModel.EmailAddress == "ADMIN1" && CustomerModel.Password == "IHEARCUP$1")
                //{
                  //  RedirectToAction("Index", "Customer");
                //}
                if (CustomerDetails == null)
                {
                    CustomerModel.LoginErrorMessage = "Wrong Username or Password.";
                    return View("Autherize", CustomerModel);
                    //return View("Autherize10", "Customer");
                    //RedirectToAction("Autherize");
                }
                else
                {

                    Session["CustomerID"] = CustomerDetails.CustomerID;
                    return RedirectToAction("Index", "Order"); //CHange this to where u wanna redirect them when it's right
                }



            }


        }
        int count1 = 0;//Testing a counter
        public ActionResult Autherize2(DefCan.Models.Customer CustomerModel2)//Second password verification
        {
            

            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {

                

                var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel2.EmailAddress && x.Password == CustomerModel2.Password).FirstOrDefault();
                //Testing balance adjustment


                if (CustomerDetails == null)
                {
                    CustomerModel2.LoginErrorMessage = "Wrong Username or Password.";
                    count1 += 1;
                    if (count1 > 2)
                    {
                        return RedirectToAction("Index", "Order");
                    }
                    return View("Autherize2", CustomerModel2);
                    //RedirectToAction("Autherize");
                }
                else
                {
                 //   Session["GT"] = GT;
                    
                    Session["CustomerID"] = CustomerDetails.CustomerID;

                    //return RedirectToAction("Index", "Order"); //CHange this to where u wanna redirect them when it's right
                    return RedirectToAction("OrderProcess", "Customer", new { id = CustomerDetails.CustomerID });
                }



            }


        }

       //public ActionResult Autherize3()
       //{
         //   return View();
        //}

        //[HttpPost]

        public ActionResult Autherize3(DefCan.Models.Customer CustomerModel, HttpPostedFileBase file3)//image password verification
        {

            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {

                if (file3 != null)
                {
                    file3.SaveAs(HttpContext.Server.MapPath("~/Images/") + file3.FileName);
                    CustomerModel.ImageID = file3.FileName;
                }

                //&& x.ImageID == CustomerModel.ImageID    file3.FileName//Swap back for file3.FileName if fails
                //var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.ImageID == CustomerModel.ImageID).FirstOrDefault();
                var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.ImageID == CustomerModel.ImageID).FirstOrDefault();

                // if (CustomerModel.ImageID == "nerg_irl.jpg")
                ///{
                if (CustomerDetails == null)
                    {
                        CustomerModel.LoginErrorMessage = "Wrong Username or ImageID.";
                        return View("Autherize3", CustomerModel);
                        //RedirectToAction("Autherize");
                    }
                    else
                    {

                        Session["CustomerID"] = CustomerDetails.CustomerID;
                        return RedirectToAction("Index", "Order"); //CHange this to where u wanna redirect them when it's right
                    }

                //}
               // return View("Autherize3", CustomerModel);

            }


        }

        public ActionResult Autherize4(DefCan.Models.Customer CustomerModel)//Admin Log in
        {

            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == "ADMIN1" && x.Password == CustomerModel.Password).FirstOrDefault();


               
                if (CustomerDetails == null)
                {
                    CustomerModel.LoginErrorMessage = "Wrong Username or Password.";
                    return View("Autherize4", CustomerModel);
                    //RedirectToAction("Autherize");
                }
                else
                {

                    Session["CustomerID"] = CustomerDetails.CustomerID;
                    return RedirectToAction("Index", "Item"); //CHange this to where u wanna redirect them when it's right
                }



            }


        }


        public ActionResult Autherize5()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Autherize5(Customer ObjCustomer)
        {
            if (ModelState.IsValid)
            {
                using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
                {
                    var obj = dbModelCustomer.Customers.Where(a => a.EmailAddress.Equals(ObjCustomer.EmailAddress) && a.Password.Equals(ObjCustomer.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["CustomerID"] = obj.CustomerID.ToString();
                        Session["EmailAddress"] = obj.EmailAddress.ToString();
                        return RedirectToAction("Index","Order");
                    }
                }
            }
            return View(ObjCustomer);
        }


        public ActionResult Autherize6(DefCan.Models.Customer CustomerModel, HttpPostedFileBase file4)//image password for second verification
        {

            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                if (file4 != null)
                {
                    file4.SaveAs(HttpContext.Server.MapPath("~/Images/") + file4.FileName);
                    CustomerModel.ImageID = file4.FileName;
                }
                //&& x.ImageID == CustomerModel.ImageID    file3.FileName//Swap back for file3.FileName if fails
                var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.ImageID == CustomerModel.ImageID).FirstOrDefault();


                // if (CustomerModel.ImageID == "nerg_irl.jpg")
                ///{
                if (CustomerDetails == null)
                {
                    CustomerModel.LoginErrorMessage = "Wrong Username or ImageID.";
                    return View("Autherize6", CustomerModel);
                    //RedirectToAction("Autherize");
                }
                else
                {

                    Session["CustomerID"] = CustomerDetails.CustomerID;
                    return RedirectToAction("OrderProcess", "Customer"); //CHange this to where u wanna redirect them when it's right
                }

                //}
                // return View("Autherize3", CustomerModel);

            }


        }

        public ActionResult Autherize7(DefCan.Models.Customer CustomerModel, HttpPostedFileBase file4)//audio password verification
        {

            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                if (file4 != null)
                {
                    file4.SaveAs(HttpContext.Server.MapPath("~/Audio/") + file4.FileName);
                    CustomerModel.AudioID = file4.FileName;
                }

                //&& x.ImageID == CustomerModel.ImageID    file3.FileName//Swap back for file3.FileName if fails
                // var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.AudioID == CustomerModel.AudioID).FirstOrDefault();
                var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.AudioID == CustomerModel.AudioID).FirstOrDefault();

                // if (CustomerModel.ImageID == "nerg_irl.jpg")
                ///{
                if (CustomerDetails == null)
                {
                    CustomerModel.LoginErrorMessage = "Wrong Username or AudioID.";
                    return View("Autherize7", CustomerModel);
                    //RedirectToAction("Autherize");
                }
                else
                {

                    Session["CustomerID"] = CustomerDetails.CustomerID;
                    return RedirectToAction("Index", "Order"); //CHange this to where u wanna redirect them when it's right
                }

                //}
                // return View("Autherize3", CustomerModel);

            }


        }

        public ActionResult Autherize8(DefCan.Models.Customer CustomerModel, HttpPostedFileBase file4)//second audio password verification
        {

            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                if (file4 != null)
                {
                    file4.SaveAs(HttpContext.Server.MapPath("~/Audio/") + file4.FileName);
                    CustomerModel.AudioID = file4.FileName;
                }

                //&& x.ImageID == CustomerModel.ImageID    file3.FileName//Swap back for file3.FileName if fails
                // var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.AudioID == CustomerModel.AudioID).FirstOrDefault();
                var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.AudioID == CustomerModel.AudioID).FirstOrDefault();

                // if (CustomerModel.ImageID == "nerg_irl.jpg")
                ///{
                if (CustomerDetails == null)
                {
                    CustomerModel.LoginErrorMessage = "Wrong Username or AudioID.";
                    return View("Autherize8", CustomerModel);
                    //RedirectToAction("Autherize");
                }
                else
                {

                    Session["CustomerID"] = CustomerDetails.CustomerID;
                    return RedirectToAction("OrderProcess", "Customer"); //CHange this to where u wanna redirect them when it's right
                }

                //}
                // return View("Autherize3", CustomerModel);

            }


        }


        public ActionResult Autherize10(DefCan.Models.Customer CustomerModel)//Error Page
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
                    return RedirectToAction("Index", "Order"); //CHange this to where u wanna redirect them when it's right
                }



            }


        }

        public ActionResult BalanceChange(int id)
        {
            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                return View(dbModelCustomer.Customers.Where(x => x.CustomerID == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult BalanceChange(Customer customer)
        {
            float GT = Convert.ToInt32(Session["GT"]);

            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                var CheckingAccount = dbModelCustomer.Customers.Find(customer.CustomerID);//customer.CustomerID



                //customer.Balance = customer.Balance - GT;
                customer.Balance = 1000;
                dbModelCustomer.Entry(customer).State = EntityState.Modified;
                dbModelCustomer.SaveChanges();
                //dbModelCustomer.Customers.Add(customer);
                //dbModelCustomer.SaveChanges();
                return RedirectToAction("Autherize", "Customer");
                
                //return View();
            }
        }
        
        public ActionResult OrderProcess()
        {
            return View();
        }

        /*
        public ActionResult Experiment(int id)
        {
            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {
                return View(dbModelCustomer.Customers.Where(x => x.CustomerID == id).FirstOrDefault());
            }
        }

        
        [HttpPost]
        public ActionResult Experiment(Customer customer)
        {
            try
            {
                using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
                {
                    dbModelCustomer.Entry(customer).State = EntityState.Modified;
                    dbModelCustomer.SaveChanges();
                }

                // TODO: Add update logic here

                return RedirectToAction("Index","Customer");
            }
            catch
            {
                return View();
            }
        }

    */

            //Secondary!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /* public ActionResult Experiment(Customer customer)
        {
             

            using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
            {

                var CurrentCustomer = dbModelCustomer.Customers.FirstOrDefault(p => p.CustomerID = customer.CustomerID);


                return View(dbModelCustomer.Customers.Where(x => x.CustomerID == id).FirstOrDefault());
            }
       }
       */

           

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
            return View(new Customer());//Added DbModelCustomer
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Customer customer, HttpPostedFileBase file3,DefCan.Models.Customer custo, HttpPostedFileBase file4)
        {
            
            try
            {
                
                if (file3 != null)
                {
                    file3.SaveAs(HttpContext.Server.MapPath("~/Images/") + file3.FileName);
                    customer.ImageID = file3.FileName;
                }

                if (file4 != null)
                {
                    file4.SaveAs(HttpContext.Server.MapPath("~/Audio/") + file4.FileName);
                    customer.AudioID = file4.FileName;
                }

                HttpResponseMessage ResponseMessage = GlobalVariables.WebApiClient.PostAsJsonAsync("Customer", customer).Result;
                //Good code below !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //using (DbModelCustomers dbModelCustomer = new DbModelCustomers())
                //{

                  //if (dbModelCustomer.Customers.Any(x => x.EmailAddress == customer.EmailAddress))
                //{
                  //     ViewBag.DuplicateMessage = "Email Address already exist";
                    //    return View("Create", customer);
                    //}

                    //dbModelCustomer.Customers.Add(customer);
                    //dbModelCustomer.SaveChanges();
                //}
                //Good code above!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
                // TODO: Add insert logic here
               // ModelState.Clear();
               // ViewBag.SuccessMessage = "Registration Successful";
                return RedirectToAction("Autherize");
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

        
        

    }
}
