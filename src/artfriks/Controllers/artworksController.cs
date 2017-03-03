using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using artfriks.Data;
using Microsoft.EntityFrameworkCore;

namespace artfriks.Controllers
{
    public class artworksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public artworksController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: artworks
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArtWorks.ToListAsync());
        }

        // GET: artworks/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        // GET: artworks/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: artworks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: artworks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: artworks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: artworks/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        // POST: artworks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}