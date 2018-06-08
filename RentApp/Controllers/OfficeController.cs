using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Office")]
    public class OfficeController : ApiController
    {
        RADBContext db = new RADBContext();

        public IQueryable<Office> GetAllOffices()
        {
            return db.Offices;
        }

        [HttpGet]
        [Route("GetOffice/{id}")]
        [ResponseType(typeof(Office))]
        public IHttpActionResult GetOffice(int id)
        {
            Office office = db.Offices.Find(id);

            if (office == null)
            {
                return NotFound();
            }

            return Ok(office);
        }
    }
}
