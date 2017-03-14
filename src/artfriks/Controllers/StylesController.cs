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
    public class StylesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StylesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Styles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Styles.ToListAsync());
        }

        // GET: Styles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var styles = await _context.Styles.SingleOrDefaultAsync(m => m.Id == id);
            if (styles == null)
            {
                return NotFound();
            }

            return View(styles);
        }

        // GET: Styles/Create
        public IActionResult Create()
        {
            ViewBag.cats = GetCategory(_context);
            ViewBag.Tags = GetTags(_context);
            return View();
        }

        // POST: Styles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,TagId,Text")] Styles styles)
        {
            ViewBag.Catgories = GetCategory(_context);
            ViewBag.Tags = GetTags(_context);
            if (ModelState.IsValid)
            {
                _context.Add(styles);
                await _context.SaveChangesAsync();
                return RedirectToAction("/Index");
            }
            return View(styles);
        }

        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetCategory(ApplicationDbContext _context)
        {
            var model = _context.Categories.ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Title });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetTags(ApplicationDbContext _context)
        {
            var model = _context.ArtTags.Where(x=>x.Type=="Style").ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Tag  });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }

        // GET: Styles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Catgories = GetCategory(_context);
            ViewBag.Tags = GetTags(_context);
            if (id == null)
            {
                return NotFound();
            }

            var styles = await _context.Styles.SingleOrDefaultAsync(m => m.Id == id);
            if (styles == null)
            {
                return NotFound();
            }
            return View(styles);
        }

        // POST: Styles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,TagId,Text")] Styles styles)
        {
            ViewBag.Catgories = GetCategory(_context);
            ViewBag.Tags = GetTags(_context);
            if (id != styles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(styles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StylesExists(styles.Id))
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
            return View(styles);
        }

        // GET: Styles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var styles = await _context.Styles.SingleOrDefaultAsync(m => m.Id == id);
            if (styles == null)
            {
                return NotFound();
            }

            return View(styles);
        }

        // POST: Styles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var styles = await _context.Styles.SingleOrDefaultAsync(m => m.Id == id);
            _context.Styles.Remove(styles);
            await _context.SaveChangesAsync();
            return RedirectToAction("/Index");
        }

        private bool StylesExists(int id)
        {
            return _context.Styles.Any(e => e.Id == id);
        }
    }
}
