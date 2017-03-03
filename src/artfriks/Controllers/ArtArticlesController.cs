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
    public class ArtArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtArticlesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ArtArticles
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArtArticles.ToListAsync());
        }

        // GET: ArtArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artArticles = await _context.ArtArticles.SingleOrDefaultAsync(m => m.Id == id);
            if (artArticles == null)
            {
                return NotFound();
            }

            return View(artArticles);
        }

        // GET: ArtArticles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArtArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AddedDate,Article,Author,IsPublished,PublishTime,Title")] ArtArticles artArticles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artArticles);
                await _context.SaveChangesAsync();
                return RedirectToAction("/Index");
            }
            return View(artArticles);
        }

        // GET: ArtArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artArticles = await _context.ArtArticles.SingleOrDefaultAsync(m => m.Id == id);
            if (artArticles == null)
            {
                return NotFound();
            }
            return View(artArticles);
        }

        // POST: ArtArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AddedDate,Article,Author,IsPublished,PublishTime,Title")] ArtArticles artArticles)
        {
            if (id != artArticles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artArticles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtArticlesExists(artArticles.Id))
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
            return View(artArticles);
        }

        // GET: ArtArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artArticles = await _context.ArtArticles.SingleOrDefaultAsync(m => m.Id == id);
            if (artArticles == null)
            {
                return NotFound();
            }

            return View(artArticles);
        }

        // POST: ArtArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artArticles = await _context.ArtArticles.SingleOrDefaultAsync(m => m.Id == id);
            _context.ArtArticles.Remove(artArticles);
            await _context.SaveChangesAsync();
            return RedirectToAction("/Index");
        }

        private bool ArtArticlesExists(int id)
        {
            return _context.ArtArticles.Any(e => e.Id == id);
        }
    }
}
