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
    public class CatgoryhomesectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatgoryhomesectionsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Catgoryhomesections
        public async Task<IActionResult> Index()
        {
            return View(await _context.Catgoryhomesection.ToListAsync());
        }

        // GET: Catgoryhomesections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catgoryhomesection = await _context.Catgoryhomesection.SingleOrDefaultAsync(m => m.Id == id);
            if (catgoryhomesection == null)
            {
                return NotFound();
            }

            return View(catgoryhomesection);
        }

        // GET: Catgoryhomesections/Create
        public IActionResult Create()
        {
            ViewBag.Catgories = GetCategory(_context);
            ViewBag.Tags = GetTags(_context);
            return View();
        }

        // POST: Catgoryhomesections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CatId,Image,Text")] Catgoryhomesection catgoryhomesection)
        {
            ViewBag.Catgories = GetCategory(_context);
            ViewBag.Tags = GetTags(_context);
            if (ModelState.IsValid)
            {
                _context.Add(catgoryhomesection);
                await _context.SaveChangesAsync();
                return RedirectToAction("/Index");
            }
            return View(catgoryhomesection);
        }

        // GET: Catgoryhomesections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Catgories = GetCategory(_context);
            ViewBag.Tags = GetTags(_context);
            if (id == null)
            {
                return NotFound();
            }

            var catgoryhomesection = await _context.Catgoryhomesection.SingleOrDefaultAsync(m => m.Id == id);
            if (catgoryhomesection == null)
            {
                return NotFound();
            }
            return View(catgoryhomesection);
        }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetCategory(ApplicationDbContext _context)
        {
            var model = _context.Categories.ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Title });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetTags(ApplicationDbContext _context)
        {
            var model = _context.ArtTags.Where(x => x.Type == "Style").ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Tag });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
        // POST: Catgoryhomesections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CatId,Image,Text")] Catgoryhomesection catgoryhomesection)
        {
            ViewBag.Catgories = GetCategory(_context);
            ViewBag.Tags = GetTags(_context);
            if (id != catgoryhomesection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catgoryhomesection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatgoryhomesectionExists(catgoryhomesection.Id))
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
            return View(catgoryhomesection);
        }

        // GET: Catgoryhomesections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catgoryhomesection = await _context.Catgoryhomesection.SingleOrDefaultAsync(m => m.Id == id);
            if (catgoryhomesection == null)
            {
                return NotFound();
            }

            return View(catgoryhomesection);
        }

        // POST: Catgoryhomesections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catgoryhomesection = await _context.Catgoryhomesection.SingleOrDefaultAsync(m => m.Id == id);
            _context.Catgoryhomesection.Remove(catgoryhomesection);
            await _context.SaveChangesAsync();
            return RedirectToAction("/Index");
        }

        private bool CatgoryhomesectionExists(int id)
        {
            return _context.Catgoryhomesection.Any(e => e.Id == id);
        }
    }
}
