using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using artfriks.Data;
using Microsoft.AspNetCore.Identity;
using artfriks.Models;

namespace artfriks.Controllers
{
    [Produces("application/json")]
    public class ArtworkController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        // GET: api/User
        public ArtworkController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Route("~/artowrk/getAllPersonal")]
        public IActionResult Get()
        {
            try
            {
                var user = _userManager.GetUserId(User);
                var returnValue = _context.ArtWorks.Where(x => x.UserId == user).ToList().OrderByDescending(v => v.AddedDate);
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/artowrk/getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var returnValue = _context.ArtWorks.ToList().OrderByDescending(v => v.AddedDate);
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/artowrk/getByTag")]
        public IActionResult GetAllDaily(int Id)
        {
            try
            {
                var returnValue = _context.ArtWithTags.Where(x=>x.TagId==Id).Select(c=>new  {
                    art=_context.ArtWorks.Where(art=>art.Id==c.ArtId).OrderByDescending(v=>v.AddedDate)
                }).ToList();
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/artowrk/GetById")]
        public IActionResult GetById(int Id)
        {
            try
            {
                var returnValue = _context.ArtWorks.FirstOrDefault(x => x.Id == Id) ?? new ArtWork();
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/artowrk/MarkFavourite")]
        public IActionResult MarkFavourite(int Id)
        {
            try
            {
                var user = _userManager.GetUserId(User);
                var Fav = new ArtFavourite();
                Fav.UserId = user;
                Fav.ArtId = Id;
                _context.ArtFavourites.Add(Fav);
                _context.SaveChanges();
             // Call Notification here
                return Ok(new { status = 1, message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/artowrk/Details")]
        public IActionResult Details(int Id)
        {
            try
            {
                var art = _context.ArtWorks.FirstOrDefault(x => x.Id == Id);
                if (art == null)
                {
                    return Ok(new { status = 0, message = "Not Found" });
                }
                var userInfo = _context.Users.Where(x => x.Id == art.UserId).Select(n => new
                {
                    userbio = _context.UserModel.FirstOrDefault(user => user.UserId == n.Id),
                    user = n
                }).First();
                var userArts = _context.ArtWorks.Where(x => x.UserId == art.UserId && x.Id != art.Id).ToList() ?? new List<ArtWork>();
                return Ok(new { status = 1, message = "Success", art=art,bio=userInfo, userArts = userArts });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }
    }
}