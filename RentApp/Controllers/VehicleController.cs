using Newtonsoft.Json;
using RentApp.Models;
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
using System.Web.Http.OData;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Vehicle")]
    public class VehicleController : ApiController
    {
        RADBContext db = new RADBContext();

        [HttpGet]
        [Route("GetVehicles")]
        public IQueryable<Vehicle> GetVehicles()
        {
            return db.Vehicles;
        }

        [HttpGet]
        [Route("GetAvailableVehicles")]
        public IQueryable<Vehicle> GetAvailableVehicles()
        {
            return db.Vehicles.Where(x => x.Available == true);
        }

        /// <summary>
        /// WORK IN PROGRESS
        /// </summary>
        [HttpGet]
        public IHttpActionResult GetPaged(int pageNo, int pageSize)
        {
            // Determine the number of records to skip
            int skip = (pageNo - 1) * pageSize;

            // Get total number of records
            int total = db.Vehicles.Where(x => x.Available==true).Count();

            // Select the customers based on paging parameters

            //SPUSTITI NA NIVO REPOZITORIJUMA
            var vehicles = db.Vehicles.Where(x => x.Available == true)
                .OrderBy(c => c.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            // Return the list of customers
            return Ok(new PagedResult<Vehicle>(vehicles, pageNo, pageSize, total));
        }

        [EnableQuery]
        [HttpGet]
        [Route("GetVehicle/{id}")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpPost]
        [Route("PostVehicle")]
        [ResponseType(typeof(Vehicle))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult PostVehicle()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Vehicle newVehicle = new Vehicle();
            var httpRequest = HttpContext.Current.Request;

            try
            {
                newVehicle = JsonConvert.DeserializeObject<Vehicle>(httpRequest.Form[0]);
                newVehicle.Service = db.Services.Find(newVehicle.ServiceId);
                newVehicle.VehicleType = db.VehicleTypes.Find(newVehicle.VehicleTypeId);
            }
            catch (JsonSerializationException)
            {
                return BadRequest(ModelState);
            }

            db.Vehicles.Add(newVehicle);

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

        [HttpPost]
        [Route("PostVehicleImage")]
        public IHttpActionResult PostVehicleImage()
        {
            var httpRequest = HttpContext.Current.Request;

            foreach (string file in httpRequest.Files)
            {
                Console.WriteLine(file);
                var postedFile = httpRequest.Files[file];

                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();

                    if (!AllowedFileExtensions.Contains(extension))
                    {
                        return BadRequest("File extension not allowed!.");
                    }
                    else
                    {
                        var filePath = HttpContext.Current.Server.MapPath("~/Content/" + postedFile.FileName);
                        //ZALEPITI IME DATOTEKE ZA VOZILO
                        //npr. vozilo.img = "Content/" + postedFile.FileName
                        postedFile.SaveAs(filePath);
                    }
                }
            }

            return Ok("Success");
        }

        [HttpPost]
        [Route("ChangeVehicle")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult ChangeVehicle()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Vehicle changeVehicle = null; 
            int id;
            var httpRequest = HttpContext.Current.Request;

            try
            {
                id = JsonConvert.DeserializeObject<Int32>(httpRequest.Form[0]);
                changeVehicle = db.Vehicles.Find(id);
                changeVehicle.Available = (changeVehicle.Available == true) ? false : true;
            }
            catch (JsonSerializationException)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Entry(changeVehicle).State = EntityState.Modified;
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

        private bool VehicleExist(int id)
        {
            return db.Vehicles.Count(e => e.Id == id) > 0;
        }
    }
}
