using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using artfriks.Data;
using artfriks.Models;
using Microsoft.AspNetCore.Identity;

namespace artfriks.Controllers
{
    public class ArtWorksController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        // GET: api/User
        public ArtWorksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: ArtWorks
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = _context.ArtWorks.Where(x=>x.Status==0).ToList().Select(x => new ArtWorkView
                {
                    artwork = x ,
                    user = _context.Users.FirstOrDefault(v => v.Id == x.UserId).Email
                });
                foreach (var i in model)
                {
                    var str = i.artwork.Title;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex=ex.Message,exc=ex.InnerException});
            }
         
        }

        // GET: ArtWorks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artWork = await _context.ArtWorks.SingleOrDefaultAsync(m => m.Id == id);
            if (artWork == null)
            {
                return NotFound();
            }

            return View(artWork);
        }

        // GET: ArtWorks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArtWorks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AddedDate,Category,Description,DimensionUnit,Height,MediumString,PictureUrl,Price,Status,TermAccepted,Title,UserId,Width")] ArtWork artWork)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artWork);
                await _context.SaveChangesAsync();
                return RedirectToAction("/Index");
            }
            return View(artWork);
        }

        // GET: ArtWorks/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artWork = _context.ArtWorks.FirstOrDefault(m => m.Id == id);
            ArtWorkEditView artView = new ArtWorkEditView();
            artView.Artwork = artWork;
            artView.User = _context.Users.Where(v => v.Id == artWork.UserId).First().Email;
            artView.Medium = _context.Mediums.ToList();
            artView.Category = _context.Categories.ToList();
            artView.Units = _context.Units.ToList();
            artView.Tags = _context.ArtTags.ToList();
            artView.Tagset = _context.ArtWithTags.Where(n => n.ArtId == artView.Artwork.Id).Select(c => new ArtWithTagsView
            {
                ArtId = c.ArtId,
                TagId = c.TagId,
                Tag = _context.ArtTags.Where(v => v.Id == c.TagId).First().Tag
            });
            ViewBag.Catgories = GetCategory(_context);
            ViewBag.Medium = GetMediums(_context);
            ViewBag.Units = GetUnits(_context);
            ViewBag.Tags = GetTags(_context);

            if (artWork == null)
            {
                return NotFound();
            }
            return View(artView);
        }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetCategory(ApplicationDbContext _context)
        {
            var model = _context.Categories.ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Title });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetUnits(ApplicationDbContext _context)
        {
            var model = _context.Units.ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Units });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetProfession(ApplicationDbContext _context)
        {
            var model = _context.Professions.ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.ProfessionString });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetTypes(ApplicationDbContext _context)
        {
            var model = _context.ArtTypes.ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Type });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetTags(ApplicationDbContext _context)
        {
            var model = _context.ArtTags.ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Tag });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetMediums(ApplicationDbContext _context)
        {
            var model = _context.Mediums.ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Mediums });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
        // POST: ArtWorks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  ArtWorkEditView param)
        {
            if (id != param.Artwork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    param.Artwork.Status = 1;
                    _context.Update(param.Artwork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtWorkExists(param.Artwork.Id))
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
            return View(param);
        }

        // GET: ArtWorks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artWork = await _context.ArtWorks.SingleOrDefaultAsync(m => m.Id == id);
            if (artWork == null)
            {
                return NotFound();
            }

            return View(artWork);
        }

        // POST: ArtWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artWork = await _context.ArtWorks.SingleOrDefaultAsync(m => m.Id == id);
            _context.ArtWorks.Remove(artWork);
            await _context.SaveChangesAsync();
            return RedirectToAction("/Index");
        }

        private bool ArtWorkExists(int id)
        {
            return _context.ArtWorks.Any(e => e.Id == id);
        }
    }
}
