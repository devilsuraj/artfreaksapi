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
    public class MediaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MediaController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Media
        public async Task<IActionResult> Index()
        {
            return View(await _context.Mediums.ToListAsync());
        }

        // GET: Media/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medium = await _context.Mediums.SingleOrDefaultAsync(m => m.Id == id);
            if (medium == null)
            {
                return NotFound();
            }

            return View(medium);
        }

        // GET: Media/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Media/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mediums")] Medium medium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medium);
                await _context.SaveChangesAsync();
                return RedirectToAction("/Index");
            }
            return View(medium);
        }

        // GET: Media/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medium = await _context.Mediums.SingleOrDefaultAsync(m => m.Id == id);
            if (medium == null)
            {
                return NotFound();
            }
            return View(medium);
        }

        // POST: Media/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mediums")] Medium medium)
        {
            if (id != medium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediumExists(medium.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("/Index");
            }
            return View(medium);
        }

        // GET: Media/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medium = await _context.Mediums.SingleOrDefaultAsync(m => m.Id == id);
            if (medium == null)
            {
                return NotFound();
            }

            return View(medium);
        }

        // POST: Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medium = await _context.Mediums.SingleOrDefaultAsync(m => m.Id == id);
            _context.Mediums.Remove(medium);
            await _context.SaveChangesAsync();
            return RedirectToAction("/Index");
        }

        private bool MediumExists(int id)
        {
            return _context.Mediums.Any(e => e.Id == id);
        }
    }
}
