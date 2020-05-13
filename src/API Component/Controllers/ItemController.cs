using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DefCan.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Web.UI.DataVisualization;
using System.Text;
using System.Drawing;
using System.Web.Mvc.Html;
using System.Web.UI.DataVisualization.Charting;

namespace DefCan.Controllers
{
    public class ItemController : Controller
    {

        public ActionResult ChartFromEF()
        {
            using (DbModelShop dbModelShops = new DbModelShop())
            {
                var data = dbModelShops.Items.ToList();
                var chart = new Chart();
                var area = new ChartArea();
                chart.ChartAreas.Add(area);
                var series = new Series();
                foreach(var item in data)
                {
                    series.Points.AddXY(item.Name, item.AmountPurchased);
                }
                series.Label = "#PERCENT{P0}";
                //series.Font = new Font("Arial",, 8.0f, FontStyle.Bold);
                area.AxisX.Title = "Item Name";
                area.AxisY.Title = "Amount Purchased";
                series.ChartType = SeriesChartType.Column; //Decides which chart type
                series["PieLabelStyle"] = "Outside";
                chart.Series.Add(series);
                var returnStream = new MemoryStream();
                chart.ImageType = ChartImageType.Jpeg;
                chart.SaveImage(returnStream);
                returnStream.Position = 0;
                return new FileStreamResult(returnStream, "image/png");



            }
        }

        // GET: Item/Index
        public ActionResult Index()
        {
            IEnumerable<Item> ItemList;
            HttpResponseMessage responseMessage = GlobalVariables.WebApiClient.GetAsync("Item").Result;
            ItemList = responseMessage.Content.ReadAsAsync<IEnumerable<Item>>().Result;

            return View(ItemList);
            //using (DbModelShop dbModelShops = new DbModelShop())
            //{
              //  return View(dbModelShops.Items.ToList());
            //}

            
        }

        public async Task<ActionResult> Search(string searchString)//Search by name
        {

            using (DbModelShop dbModelShops = new DbModelShop())
            {
                var item = from m in dbModelShops.Items
                           select m;

                if (!String.IsNullOrEmpty(searchString))
                {
                    item = item.Where(s => s.Name.Contains(searchString));
                }

                return View(await item.ToListAsync());
            }
        }

        public ActionResult Search2(string searcher)//Actually Item Search
        {
            IEnumerable<Item> obj = null;
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("https://localhost:44303/api");

            var consumeapi = hc.GetAsync("CustomerSearch?searcher=" +searcher);//check ths if it fails
            consumeapi.Wait();

            var readdata = consumeapi.Result;
            if (readdata.IsSuccessStatusCode)
            {
                var DisplayData = readdata.Content.ReadAsAsync<IList<Item>>();
                DisplayData.Wait();
                obj = DisplayData.Result;
            }
            return View(obj);

        }
        //This searches items based on the image entered
        public async Task<ActionResult> Search3(HttpPostedFileBase file9, Item items,DefCan.Models.Item ItemModel /*, string SearchString*/)
        {
            string SearchString="";
            


            if (file9 != null)
            {
                
                file9.SaveAs(HttpContext.Server.MapPath("~/Images/") + file9.FileName);
                ItemModel.AslPicUrl = file9.FileName;
                SearchString = ItemModel.AslPicUrl;
              

            }
            
            using (DbModelShop dbModelShops = new DbModelShop())
            {
                var item = from m in dbModelShops.Items
                           select m;

               

                if (!String.IsNullOrEmpty(SearchString))
          
                {
                    item = item.Where(s => s.AslPicUrl.Contains(SearchString));
                }

                return View(await item.ToListAsync());
            }
        }

        public async Task<ActionResult> Search4(HttpPostedFileBase file10, Item items, DefCan.Models.Item ItemModel /*, string SearchString*/)
        {
            string SearchString="";


            if (file10 != null)
            {

                file10.SaveAs(HttpContext.Server.MapPath("~/Audio/") + file10.FileName);
                ItemModel.Audio = file10.FileName;
                SearchString = ItemModel.Audio;

            }

            using (DbModelShop dbModelShops = new DbModelShop())
            {
                var item = from m in dbModelShops.Items
                           select m;



                if (!String.IsNullOrEmpty(SearchString))

                {
                    item = item.Where(s => s.Audio.Contains(SearchString));
                }

                return View(await item.ToListAsync());
            }
        }

        /* public async Task<ActionResult> Search3(DefCan.Models.Item ItemModel, HttpPostedFileBase file2, Item items)//image password verification
         {

             using (DbModelShop dbModelShop = new DbModelShop())
             {

                 var item = from m in dbModelShop.Items
                            select m;

                 if (file2 != null)
                 {
                     file2.SaveAs(HttpContext.Server.MapPath("~/Images/") + file2.FileName);
                     ItemModel.AslPicUrl = file2.FileName;
                 }

                 //&& x.ImageID == CustomerModel.ImageID    file3.FileName//Swap back for file3.FileName if fails
                 //var CustomerDetails = dbModelCustomer.Customers.Where(x => x.EmailAddress == CustomerModel.EmailAddress && x.ImageID == CustomerModel.ImageID).FirstOrDefault();
                 var ItemDetails = dbModelShop.Items.Where(x => x.AslPicUrl == ItemModel.AslPicUrl).FirstOrDefault();

                 // if (CustomerModel.ImageID == "nerg_irl.jpg")
                 ///{
                 if (ItemDetails == null)
                 {
                    // ItemModel.LoginErrorMessage = "Wrong Username or ImageID.";
                     return View("Search3", ItemModel);
                     //RedirectToAction("Autherize");
                 }
                 else
                 {

                     if (!String.IsNullOrEmpty(ItemModel.AslPicUrl))
                     {
                         //item = item.Where(s => s.AslPicUrl.Contains(ItemModel.AslPicUrl));
                         item = item.Where(s => s.AslPicUrl.Contains("As"));

                     }

                     return View(await item.ToListAsync());

                     //Session["CustomerID"] = CustomerDetails.CustomerID;
                     //return RedirectToAction("Index", "Order"); //CHange this to where u wanna redirect them when it's right
                 }

                 //}
                 // return View("Autherize3", CustomerModel);

             }


         }*/


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
            
            return View(new Item());
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(Item item, HttpPostedFileBase file, HttpPostedFileBase file2, HttpPostedFileBase file5)
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

                if (file5 != null)
                {
                    file5.SaveAs(HttpContext.Server.MapPath("~/Audio/") + file5.FileName);
                    item.Audio = file5.FileName;
                }

                HttpResponseMessage responseMessage = GlobalVariables.WebApiClient.PostAsJsonAsync("Item", item).Result;
                //using (DbModelShop dbModelShops = new DbModelShop())
                //{
                //  dbModelShops.Items.Add(item);
                //dbModelShops.SaveChanges();
                //return RedirectToAction("Index");
                //}
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
            if (id == 0)
                {
                return View(new Item());
            }
            else
            {
                HttpResponseMessage responseMessage = GlobalVariables.WebApiClient.GetAsync("Item/" + id.ToString()).Result;
                return View(responseMessage.Content.ReadAsAsync<Item>().Result);
            }


            //using (DbModelShop dbModelShops = new DbModelShop())
            //{
              //  return View(dbModelShops.Items.Where(x => x.ItemID == id).FirstOrDefault());
            //}
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Item item, HttpPostedFileBase file, HttpPostedFileBase file2, HttpPostedFileBase file5)
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

                if (file5 != null)
                {
                    file5.SaveAs(HttpContext.Server.MapPath("~/Audio/") + file5.FileName);
                    item.Audio = file5.FileName;
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
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Item/" + id.ToString()).Result;
            
            return RedirectToAction("Index");
            //using (DbModelShop dbModelShops = new DbModelShop())
            //{
            //  return View(dbModelShops.Items.Where(x => x.ItemID == id).FirstOrDefault());
            //}
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
