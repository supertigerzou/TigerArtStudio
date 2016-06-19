using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using webSite.Models;

namespace webSite.Controllers
{
    public class MorningNightSharingsController : Controller
    {
        private ApplicationDbContext _context;

        public MorningNightSharingsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: MorningNightSharings
        public IActionResult Index()
        {
            return View(_context.MorningNightSharings.ToList());
        }

        // GET: MorningNightSharings/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            MorningNightSharing morningNightSharing = _context.MorningNightSharings.Single(m => m.ID == id);
            if (morningNightSharing == null)
            {
                return HttpNotFound();
            }

            return View(morningNightSharing);
        }

        // GET: MorningNightSharings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MorningNightSharings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MorningNightSharing morningNightSharing)
        {
            if (ModelState.IsValid)
            {
                _context.MorningNightSharings.Add(morningNightSharing);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(morningNightSharing);
        }

        // GET: MorningNightSharings/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            MorningNightSharing morningNightSharing = _context.MorningNightSharings.Single(m => m.ID == id);
            if (morningNightSharing == null)
            {
                return HttpNotFound();
            }
            return View(morningNightSharing);
        }

        // POST: MorningNightSharings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MorningNightSharing morningNightSharing)
        {
            if (ModelState.IsValid)
            {
                _context.Update(morningNightSharing);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(morningNightSharing);
        }

        // GET: MorningNightSharings/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            MorningNightSharing morningNightSharing = _context.MorningNightSharings.Single(m => m.ID == id);
            if (morningNightSharing == null)
            {
                return HttpNotFound();
            }

            return View(morningNightSharing);
        }

        // POST: MorningNightSharings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            MorningNightSharing morningNightSharing = _context.MorningNightSharings.Single(m => m.ID == id);
            _context.MorningNightSharings.Remove(morningNightSharing);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
