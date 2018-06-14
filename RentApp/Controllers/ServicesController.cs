﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RentApp.Persistance;
using System.Web;
using Newtonsoft.Json;
using System.Data.Entity.Validation;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Services")]
    public class ServicesController : ApiController
    {
        private RADBContext db;

        public ServicesController(DbContext context)
        {
            db = context as RADBContext;
        }

        [HttpGet]
        public IQueryable<Service> GetServices()
        {
            return db.Services;
        }

        [HttpGet]
        [Route("GetService/{id}")]
        [ResponseType(typeof(Service))]
        public IHttpActionResult GetService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        [HttpPut]
        [Route("PutService/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.Id)
            {
                return BadRequest();
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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
        [Route("PostService")]
        [ResponseType(typeof(Service))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult PostService()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var user = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            //if (user == null)
            //{
            //    return BadRequest("You're not log in.");
            //}

            Service newService = new Service();
            var httpRequest = HttpContext.Current.Request;

            try
            {
                newService = JsonConvert.DeserializeObject<Service>(httpRequest.Form[0]);
            }
            catch (JsonSerializationException)
            {
                return BadRequest(ModelState);
            }

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
                        return BadRequest();
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

            db.Services.Add(newService);

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

            return CreatedAtRoute("DefaultApi", new { id = newService.Id }, newService);
        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
            db.SaveChanges();

            return Ok(service);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.Id == id) > 0;
        }
    }
}