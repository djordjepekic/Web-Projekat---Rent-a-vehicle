using Newtonsoft.Json;
using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        [Route("PostOffice")]
        [ResponseType(typeof(Office))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult PostOffice()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Office newOffice = new Office();
            var httpRequest = HttpContext.Current.Request;

            try
            {
                newOffice = JsonConvert.DeserializeObject<Office>(httpRequest.Form[0]);
                newOffice.Service = db.Services.Find(newOffice.ServiceId);
            }
            catch (JsonSerializationException)
            {
                return BadRequest(ModelState);
            }

            db.Offices.Add(newOffice);

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
        [Route("PostOfficeImage")]
        [ResponseType(typeof(Office))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult PostOfficeImage()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                        //ZALEPITI IME DATOTEKE ZA SERVICE
                        //npr. service.img = "Content/" + postedFile.FileName
                        postedFile.SaveAs(filePath);
                    }
                }
            }

            return Ok("Success");
        }
    }
}
