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
    [RoutePrefix("api/AppUser")]
    public class AppUserController : ApiController
    {
        RADBContext db = new RADBContext();

        [HttpGet]
        public IQueryable<AppUser> GetAllUsers()
        {
            return db.AppUsers;
        }

        [HttpGet]
        [Route("GetUser/{id}")]
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult GetUser(int id)
        {
            AppUser user = db.AppUsers.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("CreateUser")]
        public IHttpActionResult CreateUser(AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.AppUsers.Add(user);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return Content(HttpStatusCode.Conflict, user);
            }

            return Ok("Success");
        }

        [HttpPut]
        [Route("ChangeUser/{id}")]
        public IHttpActionResult ChangeUser(int id, AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExist(id))
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

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            AppUser user = db.AppUsers.Where(e => e.Id.Equals(id)).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            db.AppUsers.Remove(user);
            db.SaveChanges();

            return Ok();
        }

        private bool UserExist(int id)
        {
            return db.AppUsers.Count(e => e.Id.Equals(id)) > 0;
        }
    }
}
