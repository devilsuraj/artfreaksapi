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
    public class homesectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public homesectionsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: homesections
        public async Task<IActionResult> Index()
        {
            return View(await _context.homesection.ToListAsync());
        }

        // GET: homesections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homesection = await _context.homesection.SingleOrDefaultAsync(m => m.Id == id);
            if (homesection == null)
            {
                return NotFound();
            }

            return View(homesection);
        }

        // GET: homesections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: homesections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Image,Sectiontype,TextonButton,Title,Title2")] homesection homesection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homesection);
                await _context.SaveChangesAsync();
                return RedirectToAction("/Index");
            }
            return View(homesection);
        }

        // GET: homesections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homesection = await _context.homesection.SingleOrDefaultAsync(m => m.Id == id);
            if (homesection == null)
            {
                return NotFound();
            }
            return View(homesection);
        }

        // POST: homesections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Image,Sectiontype,TextonButton,Title,Title2")] homesection homesection)
        {
            if (id != homesection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homesection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!homesectionExists(homesection.Id))
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
            return View(homesection);
        }

        // GET: homesections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homesection = await _context.homesection.SingleOrDefaultAsync(m => m.Id == id);
            if (homesection == null)
            {
                return NotFound();
            }

            return View(homesection);
        }

        // POST: homesections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homesection = await _context.homesection.SingleOrDefaultAsync(m => m.Id == id);
            _context.homesection.Remove(homesection);
            await _context.SaveChangesAsync();
            return RedirectToAction("/Index");
        }

        private bool homesectionExists(int id)
        {
            return _context.homesection.Any(e => e.Id == id);
        }
    }
}
