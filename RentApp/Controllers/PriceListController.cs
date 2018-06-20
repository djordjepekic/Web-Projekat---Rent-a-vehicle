using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RentApp.Controllers
{
    [RoutePrefix("api/PriceList")]
    public class PriceListController : ApiController
    {
        RADBContext db = new RADBContext();

        public IQueryable<PriceListItem> GetAllPriceListItems()
        {
            return db.PriceListItems;
        }
    }
}
