using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DefCanApi.Controllers
{
    public class CustomerSearchController : ApiController//Actually Item Search!!!!!!!
    {
        private Models.DbModel db = new Models.DbModel();
        public IHttpActionResult GetCustName(string searcher)
        {

            Models.DbModel db = new Models.DbModel();
            var result = db.Items.Where(x => x.Name.StartsWith(searcher) || searcher == null).ToList();
            return Ok(result);
    }


    }
}

