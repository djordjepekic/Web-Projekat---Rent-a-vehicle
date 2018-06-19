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
    [RoutePrefix("api/VehicleType")]
    public class VehicleTypeController : ApiController
    {
        private RADBContext db;

        public VehicleTypeController(DbContext context)
        {
            db = context as RADBContext;
        }

        [HttpGet]
        public IQueryable<VehicleType> GetVehicleTypes()
        {
            return db.VehicleTypes;
        }

        [HttpGet]
        [Route("GetVehicleType/{id}")]
        [ResponseType(typeof(VehicleType))]
        public IHttpActionResult GetVehicleType(int id)
        {
            VehicleType type = db.VehicleTypes.Find(id);
            if (type == null)
            {
                return NotFound();
            }

            return Ok(type);
        }

        [HttpPut]
        [Route("PutVehicleType/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicleType(int id, VehicleType type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != type.Id)
            {
                return BadRequest();
            }

            db.Entry(type).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostVehicleType")]
        [ResponseType(typeof(VehicleType))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PostVehicleType()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VehicleType newType = new VehicleType();
            var httpRequest = HttpContext.Current.Request;

            try
            {
                newType = JsonConvert.DeserializeObject<VehicleType>(httpRequest.Form[0]);
            }
            catch (JsonSerializationException)
            {
                return BadRequest(ModelState);
            }

            db.VehicleTypes.Add(newType);

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

        private bool TypeExist(int id)
        {
            return db.VehicleTypes.Count(e => e.Id == id) > 0;
        }

    }
}
