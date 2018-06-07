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
        [Route("GetUser")]
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult GetUser()
        {
            var user = new AppUser {
                Id = 1,
                FullName = "User1 User1",
                Adress = "Adress1",
                CanCreateService = true,
                DateOfBirth = DateTime.Now,
                Image = "",
                Verified = true
            };

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
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

            return Ok();
        }

        [HttpPut]
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
        public IHttpActionResult Delete(int id)
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
