using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Vehicle")]
    public class VehicleController : ApiController
    {
        RADBContext db = new RADBContext();

        [HttpGet]
        public IQueryable<Vehicle> GetVehicles()
        {
            return db.Vehicles;
        }

        [HttpGet]
        [Route("GetVehicle/{id}")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(int id)
        {
            //Vehicle vehicle = db.Vehicles.Include("Service").FirstOrDefault(v => v.Id == id);
            Vehicle vehicle = db.Vehicles.Find(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        //[Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("CreateVehicle")]
        public IHttpActionResult CreateVehicle(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var user = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            //if (user == null)
            //{
            //    return BadRequest("404");
            //}

            Service service = db.Services.Where(s => s.Id == vehicle.ServiceId).FirstOrDefault();

            if (service == null)
            {
                return BadRequest("404");
            }

            try
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return Content(HttpStatusCode.Conflict, vehicle);
            }

            return Ok("Success");
        }
    }
}
