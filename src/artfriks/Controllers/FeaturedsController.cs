using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using artfriks.Data;
using artfriks.Models;

namespace artfriks.Controllers
{
    public class FeaturedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeaturedsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Featureds
        public async Task<IActionResult> Index()
        {
            return View(await _context.Featured.ToListAsync());
        }

        // GET: Featureds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featured = await _context.Featured.SingleOrDefaultAsync(m => m.Id == id);
            if (featured == null)
            {
                return NotFound();
            }

            return View(featured);
        }

        // GET: Featureds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Featureds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Text")] Featured featured)
        {
            if (ModelState.IsValid)
            {
                _context.Add(featured);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(featured);
        }

        // GET: Featureds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featured = await _context.Featured.SingleOrDefaultAsync(m => m.Id == id);
            if (featured == null)
            {
                return NotFound();
            }
            return View(featured);
        }

        // POST: Featureds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Text")] Featured featured)
        {
            if (id != featured.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(featured);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeaturedExists(featured.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(featured);
        }

        // GET: Featureds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featured = await _context.Featured.SingleOrDefaultAsync(m => m.Id == id);
            if (featured == null)
            {
                return NotFound();
            }

            return View(featured);
        }

        // POST: Featureds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var featured = await _context.Featured.SingleOrDefaultAsync(m => m.Id == id);
            _context.Featured.Remove(featured);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FeaturedExists(int id)
        {
            return _context.Featured.Any(e => e.Id == id);
        }
    }
}
