using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using artfriks.Data;
using Microsoft.AspNetCore.Identity;
using artfriks.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace artfriks.Controllers
{
    [Produces("application/json")]
    public class artController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        // GET: api/User
        public artController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Route("api/artowrk/getAllPersonal")]
        public IActionResult Get()
        {
            try
            {
                var user = _userManager.GetUserId(User);
                var returnValue = _context.ArtWorks.Where(x => x.UserId == user).Select(p => new {
                    Id = p.Id,
                    AddedDate = p.AddedDate,
                    TermAccepted = p.TermAccepted,
                    Category = p.Category,
                    Description = p.Description,
                    DimensionUnit = p.DimensionUnit,
                    Height = p.Height,
                    MediumString = p.MediumString,
                    PictureUrl = p.PictureUrl,
                    Price = p.Price,
                    views = p.Views,
                    Status = p.Status,
                    Title = p.Title,
                    Width = p.Width,
                    UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                    favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user)
                }).OrderByDescending(v => v.AddedDate); 
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/getAllFavourites")]
        public IActionResult Getfav()
        {
            try
            {
                var user = _userManager.GetUserId(User);
                var returnValue = _context.ArtFavourites.Where(x=>x.UserId==user).
                    Select(mo=> new {

                       artwork = _context.ArtWorks.FirstOrDefault(op=> op.Id ==mo.ArtId && op.Title !=null) ?? new ArtWork(),
                        UserId = _context.Users.Where(n => n.Id == mo.UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == mo.Id).Count(),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == mo.Id && x.UserId == user)
                 });
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/getAll")]
        public IActionResult GetAll()
        {
            var user = _userManager.GetUserId(User);
            try
            {
                var returnValue = _context.ArtWorks.Where(x=>x.Status==1).Select(p=>new  {
                    Id=p.Id,
                    AddedDate=p.AddedDate,
                    TermAccepted=p.TermAccepted,
                    Category=p.Category,
                    Description=p.Description,
                    DimensionUnit=p.DimensionUnit,
                    Height=p.Height,
                    MediumString=p.MediumString,
                    PictureUrl=p.PictureUrl,
                    Price=p.Price,
                    Status=p.Status,
                    views = p.Views,
                    Title =p.Title,
                    Width=p.Width,
                    UserId=_context.Users.Where(n=>n.Id==p.UserId).First().FullName,
                    favcount=_context.ArtFavourites.Where(x=>x.ArtId==p.Id).Count(),
                    isfav=_context.ArtFavourites.Any(x=>x.ArtId==p.Id && x.UserId==user)
                }).OrderByDescending(v => v.AddedDate);
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/getByTag")]
        public IActionResult GetAllDaily(int Id)
        {
            try
            {
                var returnValue = _context.ArtWithTags.Where(x => x.TagId == Id).Select(c => new {
                    art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId),
                    UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId).First().FullName,
                    favcount = _context.ArtFavourites.Where(x => x.ArtId == c.Id).Count(),
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId)
                }).ToList();
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        public IEnumerable<CategoryChildFromStore> GetCatLevel(int id)
        {
                datalayer _dataLayer = new datalayer();
            using (var reader = _dataLayer.GetDataReaderByProc("catsLevel", "@CategoryId", id.ToString()))
            {
                while (reader.Read())
                {
                    yield return Create(reader);
                }
            }
        }
        public static CategoryChildFromStore Create(IDataRecord record)
        {
            return new CategoryChildFromStore
            {
                id = Convert.ToInt32(record["id"]),
                parentId = Convert.ToInt32(record["parentId"]),
                category = record["category"].ToString(),
                Level = Convert.ToInt32(record["Level"])
            };
        }

        [HttpGet]
        [Route("api/artowrk/getbycategory")]
        public IActionResult getbycategory(int Id)
        {
            var user = _userManager.GetUserId(User);
            var result = GetCatLevel(Id);
            try
            {
                if (result != null)
                {
                    var model = _context.ArtWorks.Where(x => x.Status == 1).Select(p => new {
                        Id = p.Id,
                        AddedDate = p.AddedDate,
                        TermAccepted = p.TermAccepted,
                        Category = p.Category,
                        Description = p.Description,
                        DimensionUnit = p.DimensionUnit,
                        Height = p.Height,
                        MediumString = p.MediumString,
                        PictureUrl = p.PictureUrl,
                        Price = p.Price,
                        Status = p.Status,
                        Title = p.Title,
                        views=p.Views,
                        Width = p.Width,
                        UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user)
                    }).OrderByDescending(v => v.AddedDate);

                    var returnValue = (from a in result join b in model on a.id.ToString() equals b.Category group b by b.Id into bg select bg.FirstOrDefault());
                    var subcategories = _context.Categories.Where(x => x.ParentId == Id);
                    var categorytitle = _context.Categories.FirstOrDefault(x => x.Id == Id).Title;
                    return Ok(new { status = 1, message = returnValue, subcategories= subcategories, categorytitle= categorytitle });
                }
                else
                {
                    return Ok(new { status = 0, message = "No Records" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/GetHomeData")]
        public IActionResult GetHomeData(int Id)
        {
            //10 - deals , 11 - collection , 12 - slider
            try
            {
                var slider = _context.ArtWithTags.Where(x => x.TagId == 12).Select(c => new {
                    art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId)
                  }).OrderBy(x => Guid.NewGuid()).Take(7).ToList();

                var featured = _context.Featured.ToList();
                var category = _context.Catgoryhomesection.ToList();
                var styles = _context.Styles.ToList();
                var homesection = _context.homesection.ToList();
              
                return Ok(new { status = 1, slider=slider,featured=featured,styles=styles,homesection=homesection , category=category });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/GetById")]
        public IActionResult GetById(int Id)
        {
            var user = _userManager.GetUserId(User);
            try
            {
                var returnValue = _context.ArtWorks.Where(x => x.Id == Id).Select(p => new ArtWorkView
                {
                    artwork=p,
                    user= _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user),
                    favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count()
                   
                }).First() ?? new ArtWorkView();
                if (returnValue == null) { return Ok(new { status = 0, message = "Not Found" }); }
                var userInfo = _context.Users.Where(c => c.Id == returnValue.artwork.UserId).First() ;
                var userProfile = _context.UserModel.Where(x => x.UserId == returnValue.artwork.UserId).First() ?? new UserModel();
                var userArtWorks = _context.ArtWorks.Where(x => x.UserId == returnValue.artwork.UserId).Take(5).OrderByDescending(c=>c.AddedDate);
                if (user != null) {
                    var MessageReplies = _context.Messages.Where(x => x.ArtId == Id && x.FromUserId == user).Select(o => new {
                        message = _context.Messages.Where(b => b.Id == o.Id),
                        replyies = _context.MessageReplies.Where(v=>v.MessageId==o.Id)
                     
                });
                    return Ok(new
                    {
                        status = 1,
                        message = "success",
                        art = returnValue,
                        user = userInfo,
                        profile = userProfile,
                        artworks = userArtWorks,
                        messages = MessageReplies
                    });
                }
                return Ok(new
                {
                    status = 1,
                    message = "success",
                    art = returnValue,
                    user = userInfo,
                    profile = userProfile,
                    artworks = userArtWorks
                });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }


        [HttpPost]
        [Route("api/artowrk/PostTags")]
        public IActionResult PostTags([FromBody]IEnumerable<PostTags> Posttags)
        {
            try
            {
                foreach(var i in Posttags)
                {
                    var arta = new ArtWithTags();
                    arta.ArtId = Convert.ToInt32( i.artId);
                    arta.TagId = Convert.ToInt32( i.tagId);
                    _context.ArtWithTags.Add(arta);
                }
             
                _context.SaveChanges();
                return Ok(new { status = 1, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/RemoveTags")]
        public IActionResult RemoveTags(string Id, string tag)
        {
            var artId = Convert.ToInt32(Id);
            var tagId = Convert.ToInt32(tag);
            try
            {
                var arta = _context.ArtWithTags.Where(x => x.ArtId == artId && x.TagId == tagId);
                if (arta == null)
                {
                    return Ok(new { status = 0, message = "Not Found" });
                }
                foreach (var i in arta)
                {
                    _context.ArtWithTags.Remove(i);
                }
                _context.SaveChanges();
                return Ok(new { status = 1, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/MarkFavourite")]
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
        [Route("api/artowrk/removefav")]
        public IActionResult removefav(int Id)
        {
            try
            {
                var user = _userManager.GetUserId(User);
                var Fav = _context.ArtFavourites.Where(x=>x.UserId==user && x.ArtId==Id);
                 foreach(var i in Fav)
                {
                    _context.ArtFavourites.Remove(i);
                }
              
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
        [Route("api/artowrk/Details")]
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
                return Ok(new { status = 1, message = "Success", art = art, bio = userInfo, userArts = userArts });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [Route("api/artowrk/Tags")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getUsers(string name)
        {
            return Ok(_context.ArtTags.Where(x => x.Tag.ToLower().Contains(name.ToLower())));
        }

        [Route("api/artowrk/getARticle")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getArticles()
        {
            return Ok(_context.ArtArticles.ToList());
        }
        [Route("api/artowrk/getARticlebyId")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getArticlesbyId(int Id)
        {
            return Ok(_context.ArtArticles.Where(x=>x.Id==Id).First());
        }

        [Route("api/artowrk/Categories")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getCategories()
        {
            return Ok(_context.Categories.Where(x=>x.ParentId==0).ToList());
        }

        [Route("api/artowrk/Subcategories")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getSubCategories(int id)
        {
            return Ok(_context.Categories.Where(x => x.ParentId == id).ToList());
        }

        [Route("api/artowrk/Types")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getTypes()
        {
            return Ok(_context.ArtTypes.ToList());
        }
        [Route("api/artowrk/Profession")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getProfession()
        {
            return Ok(_context.Professions.ToList());
        }

        [Route("api/artowrk/units")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getUnits()
        {
            return Ok(_context.Units.ToList());
        }

        [Route("api/artowrk/mediums")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getmediums()
        {
            return Ok(_context.Mediums.ToList());
        }

        [HttpPost]
        [Authorize]
        [Route("api/artowrk/postArt")]
        public IActionResult postArt([FromBody]ArtWork Art)
        {
            var user = _userManager.GetUserId(User);
            Art.UserId = user;
            Art.Status = 0;
            Art.AddedDate = DateTime.Now;
            try
            {
                _context.ArtWorks.Add(Art);
                _context.SaveChanges();
                return Ok(new { status = 1, message = "Success", id = Art.Id });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }
    }
}