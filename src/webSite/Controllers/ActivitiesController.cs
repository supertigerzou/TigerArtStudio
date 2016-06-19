using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using webSite.Models;
using System.Threading;

namespace webSite.Controllers
{
    public class ActivityContent { public string phone; }

    [Produces("application/json")]
    [Route("api/Activities")]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Activities
        [HttpGet]
        public string GetApplicationUser()
        {
            return "0001";
        }

        // GET: api/Activities/5
        [HttpGet("{id}", Name = "GetApplicationUser")]
        public IActionResult GetApplicationUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return Ok(applicationUser);
        }

        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public IActionResult PutApplicationUser(string id, [FromBody] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            if (id != applicationUser.Id)
            {
                return HttpBadRequest();
            }

            _context.Entry(applicationUser).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
                {
                    return HttpNotFound();
                }
                else
                {
                    throw;
                }
            }

            return new HttpStatusCodeResult(StatusCodes.Status204NoContent);
        }

        // POST: api/Activities
        [HttpPost]
        public string PostActivity([FromBody] ActivityContent activityContent)
        {
            Thread.Sleep(3000);
            return "0001";
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public IActionResult DeleteApplicationUser(string id)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            _context.ApplicationUser.Remove(applicationUser);
            _context.SaveChanges();

            return Ok(applicationUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Count(e => e.Id == id) > 0;
        }
    }
}