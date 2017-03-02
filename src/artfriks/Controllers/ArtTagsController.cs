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
    public class ArtTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtTagsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ArtTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArtTags.ToListAsync());
        }

        // GET: ArtTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artTag = await _context.ArtTags.SingleOrDefaultAsync(m => m.Id == id);
            if (artTag == null)
            {
                return NotFound();
            }

            return View(artTag);
        }

        // GET: ArtTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArtTags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tag")] ArtTag artTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artTag);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(artTag);
        }

        // GET: ArtTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artTag = await _context.ArtTags.SingleOrDefaultAsync(m => m.Id == id);
            if (artTag == null)
            {
                return NotFound();
            }
            return View(artTag);
        }

        // POST: ArtTags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tag")] ArtTag artTag)
        {
            if (id != artTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtTagExists(artTag.Id))
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
            return View(artTag);
        }

        // GET: ArtTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artTag = await _context.ArtTags.SingleOrDefaultAsync(m => m.Id == id);
            if (artTag == null)
            {
                return NotFound();
            }

            return View(artTag);
        }

        // POST: ArtTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artTag = await _context.ArtTags.SingleOrDefaultAsync(m => m.Id == id);
            _context.ArtTags.Remove(artTag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ArtTagExists(int id)
        {
            return _context.ArtTags.Any(e => e.Id == id);
        }
    }
}
