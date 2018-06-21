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
        [Authorize(Roles = "Admin")]
        [Route("GetAllManagers")]
        public List<AppUser> GetAllManagers()
        {
            List<AppUser> appUsers = new List<AppUser>();

            //include zbog lazy loading-a
            var role = db.Roles.Include(x => x.Users).Where(r => r.Name.Equals("Manager")).FirstOrDefault();

            foreach (var user in role.Users)
            {
                if (user.RoleId.Equals(role.Id))
                {
                    var netUser = db.Users.Where(x => x.Id == user.UserId).FirstOrDefault();
                    AppUser manager = db.AppUsers.Where(x => x.Id == netUser.AppUserId).FirstOrDefault();
                    appUsers.Add(manager);
                }
            }

            return appUsers;
        }

        [HttpPost]
        [Route("ChangeManagerStatus")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ChangeManagerStatus()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AppUser changeUser = null;
            int id;
            var httpRequest = HttpContext.Current.Request;

            try
            {
                id = JsonConvert.DeserializeObject<Int32>(httpRequest.Form[0]);
                changeUser = db.AppUsers.Find(id);
                changeUser.CanCreateService = (changeUser.CanCreateService == true) ? false : true;
            }
            catch (JsonSerializationException)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Entry(changeUser).State = EntityState.Modified;
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
