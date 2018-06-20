using Newtonsoft.Json;
using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

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

        [HttpPost]
        [Route("ChangeVehiclePrice")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult ChangeVehiclePrice()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var httpRequest = HttpContext.Current.Request;
            PriceListItem newPriceListItem = new PriceListItem();
            PriceListItem changePriceListItem = null;

            try
            {
                newPriceListItem = JsonConvert.DeserializeObject<PriceListItem>(httpRequest.Form[0]);
            }
            catch (JsonSerializationException)
            {
                return BadRequest(ModelState);
            }

            foreach (PriceListItem pi in db.PriceListItems)
            {
                if (pi.VehicleId == newPriceListItem.VehicleId)
                {
                    changePriceListItem = pi;
                    break;
                }
            }

            if(changePriceListItem != null)
            {
                try
                {
                    changePriceListItem.Price = newPriceListItem.Price;
                }
                catch (JsonSerializationException)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    db.Entry(changePriceListItem).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException)
                {
                    return BadRequest(ModelState);
                }
                catch (DbUpdateException)
                {
                    return BadRequest(ModelState);
                }
            }
            else
            {
                try
                {
                    newPriceListItem.Vehicle = db.Vehicles.Find(newPriceListItem.VehicleId);
                }
                catch (JsonSerializationException)
                {
                    return BadRequest(ModelState);
                }

                db.PriceListItems.Add(newPriceListItem);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException)
                {
                    return BadRequest(ModelState);
                }
                catch (DbUpdateException)
                {
                    return BadRequest(ModelState);
                }

            }
            
            return Ok("Success");
        }
    }
}
