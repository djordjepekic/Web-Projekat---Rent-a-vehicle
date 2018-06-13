using RentApp.Models;
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
        public IHttpActionResult GetPaged(int pageNo, int pageSize)
        {
            // Determine the number of records to skip
            int skip = (pageNo - 1) * pageSize;

            // Get total number of records
            int total = db.Vehicles.Count();

            // Select the customers based on paging parameters

            //SPUSTITI NA NIVO REPOZITORIJUMA
            var vehicles = db.Vehicles
                .OrderBy(c => c.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            // Return the list of customers
            return Ok(new PagedResult<Vehicle>(vehicles, pageNo, pageSize, total));
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

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("CreateVehicle")]
        public IHttpActionResult CreateVehicle(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            if (user == null)
            {
                return BadRequest("Not logged in!");
            }

            Service service = db.Services.Where(s => s.Id == vehicle.ServiceId).FirstOrDefault();

            if (service == null)
            {
                return BadRequest("404: Service not found!");
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
