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

        [HttpGet]
        [Route("GetPriceList/{id}")]
        [ResponseType(typeof(PriceListItem))]
        public IHttpActionResult GetPriceListItem(int id)
        {
            var pri = db.PriceListItems.Where(x => x.VehicleId == id);

            if (pri == null)
            {
                return NotFound();
            }

            return Ok(pri);
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

        [HttpPost]
        [Route("Reservation")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles = "AppUser")]
        public IHttpActionResult Reservation()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ReservationModel reservationModel = new ReservationModel();
            PriceList priceList = new PriceList();
            var httpRequest = HttpContext.Current.Request;
            RAIdentityUser user = null;
            try
            {
                reservationModel = JsonConvert.DeserializeObject<ReservationModel>(httpRequest.Form[0]);

                using (var context = new RADBContext())
                {                 
                    user = context.Users
                                    .Where(b => b.UserName == reservationModel.UserName)
                                    .FirstOrDefault();
                    context.Dispose();
                }

                foreach(AppUser u in db.AppUsers)
                {
                    if(u.Id == user.AppUserId)
                    {
                        priceList.UserId = u.Id;
                        priceList.User = u;
                    }
                }

                priceList.TimeOfReservation = reservationModel.TimeOfReservation;
                priceList.TimeToReturn = reservationModel.TimeToReturn;
                priceList.TakeOfficeId = reservationModel.TakeOfficeId;
                priceList.ReturnOfficeId = reservationModel.ReturnOfficeId;
                priceList.TakeOffice = db.Offices.Find(reservationModel.TakeOfficeId);
                priceList.ReturnOffice = db.Offices.Find(reservationModel.ReturnOfficeId);

                using (var context = new RADBContext())
                {
                    Vehicle v = context.Vehicles
                                    .Where(b => b.Id == reservationModel.VehicleId)
                                    .FirstOrDefault();

                    v.Available = false;
                    context.Entry(v).State = EntityState.Modified;
                    context.SaveChanges();
                    context.Dispose();
                }
            }
            catch (JsonSerializationException)
            {
                return BadRequest(ModelState);
            }

            db.PriceLists.Add(priceList);

            PriceList changePriceList = new PriceList();
            changePriceList = db.PriceLists.Find(priceList.Id);
            using (var context = new RADBContext())
            {
                PriceListItem pi = context.PriceListItems
                                .Where(b => b.VehicleId == reservationModel.VehicleId)
                                .FirstOrDefault();

                pi.PriceList = changePriceList;
                pi.PriceListId = changePriceList.Id;
                context.Entry(pi).State = EntityState.Modified;
                context.Dispose();
            }

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

            

            return Ok("Success");
        }
    }
}
