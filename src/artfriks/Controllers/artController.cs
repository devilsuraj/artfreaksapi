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
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                var returnValue = _context.ArtWorks.Where(x => x.UserId == user && _context.Users.Any(c => c.Id == x.UserId) && x.Status != 55).Select(p => new
                {
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
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user),
                    tags = _context.ArtWithTags.Where(c => c.ArtId == p.Id).Select(v => new
                    {
                        Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                    })
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
                var returnValue = _context.ArtFavourites.Where(x => x.UserId == user && _context.Users.Any(c => c.Id == x.UserId) && _context.ArtWorks.Any(v => v.Id == x.ArtId)).
                    Select(mo => new
                    {

                        artwork = _context.ArtWorks.FirstOrDefault(op => op.Id == mo.ArtId && op.Title != null) ?? new ArtWork(),
                        UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.Where(b => b.Id == mo.ArtId).First().UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(vx => vx.ArtId == mo.Id).Count(),
                        isfav = true,
                        tags = _context.ArtWithTags.Where(xc => xc.ArtId == mo.ArtId).Select(v => new
                        {
                            Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                        })
                    });
                return Ok(new { status = 1, message = returnValue.Distinct() });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/getAll")]
        public IActionResult GetAll(int limit, int offset)
        {
            var user = _userManager.GetUserId(User);
            try
            {
                var returnValue = _context.ArtWorks.Where(x => x.Status == 1 && _context.Users.Any(c => c.Id == x.UserId)).Select(p => new
                {
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
                    views = p.Views,
                    Title = p.Title,
                    Width = p.Width,
                    UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                    favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user),
                    tags = _context.ArtWithTags.Where(c => c.ArtId == p.Id).Select(v => new
                    {
                        Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                    })
                }).OrderByDescending(v => v.AddedDate);
                return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count(), Month = GetMonth() });
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
                var returnValue = _context.ArtWithTags.Where(x => x.TagId == Id && _context.ArtWorks.Any(art => art.Id == x.ArtId && _context.Users.Any(c => c.Id == art.UserId))).Select(c => new
                {
                    art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId),
                    UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.First(art => art.Id == c.ArtId).UserId).First().FullName,

                    favcount = _context.ArtFavourites.Where(x => x.ArtId == c.ArtId).Count(),
                    tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.ArtId).Select(v => new
                    {
                        Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                    }),
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId)
                }).ToList();
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/getdeals")]
        public IActionResult getdeals(int Id)
        {
            if (Id == 301)
            {
                try
                {
                    var returnValue = _context.ArtWithTags.Where(x => x.TagId == 10 && _context.ArtWorks.Any(v => v.Price >= Id && v.Id == x.ArtId &&
                    _context.Users.Any(c => c.Id == v.UserId))).Select(c => new
                    {
                        art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId),
                        UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == c.ArtId).Count(),
                        tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.ArtId).Select(v => new
                        {
                            Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                        }),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId)
                    }).ToList();
                    return Ok(new { status = 1, message = returnValue });
                }
                catch (Exception ex)
                {
                    return Ok(new { status = 0, message = ex.Message });
                }
            }
            try
            {
                var returnValue = _context.ArtWithTags.Where(x => x.TagId == 10 && _context.ArtWorks.Any(v => v.Price <= Id && v.Id == x.ArtId && _context.Users.Any(c => c.Id == v.UserId))).Select(c => new
                {
                    art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId),
                    UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId).First().FullName,
                    favcount = _context.ArtFavourites.Where(x => x.ArtId == c.ArtId).Count(),
                    tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.ArtId).Select(v => new
                    {
                        Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                    }),
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
        public IActionResult getbycategory(int Id, int limit, int offset)
        {
            var user = _userManager.GetUserId(User);
            IEnumerable<Category> result = _context.Categories.FromSql("catsLevel2 @p0", Id).ToList();
            try
            {
                if (result != null)
                {
                    var model = _context.ArtWorks.Where(x => x.Status == 1 && _context.Users.Any(c => c.Id == x.UserId)).Select(p => new
                    {
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
                        views = p.Views,
                        Width = p.Width,
                        tags = _context.ArtWithTags.Where(xc => xc.ArtId == p.Id).Select(v => new
                        {
                            Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                        }),
                        UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user)
                    }).OrderByDescending(v => v.AddedDate);
                    var returnValue = (from a in result join b in model on a.Id.ToString() equals b.Category group b by b.Id into bg select bg.FirstOrDefault());
                    var subcategories = _context.Categories.Where(x => x.ParentId == Id);
                    var categorytitle = _context.Categories.FirstOrDefault(x => x.Id == Id).Title;
                    return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), subcategories = subcategories, categorytitle = categorytitle, pages = returnValue.Count() });
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
                var slider = _context.ArtWithTags.Where(x => x.TagId == 12).Select(c => new
                {
                    art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId)
                }).OrderBy(x => Guid.NewGuid()).Take(9).ToList();

                var featured = _context.Featured.ToList();
                var category = _context.Catgoryhomesection.ToList();
                var styles = _context.Styles.ToList();
                var homesection = _context.homesection.ToList();

                return Ok(new { status = 1, slider = slider, featured = featured, styles = styles, homesection = homesection, category = category });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/GetRandom")]
        public IActionResult GetRandom(int Id, int limit, int offset)
        {
            //10 - deals , 11 - collection , 12 - slider
            try
            {
                var slider = _context.ArtWithTags.Where(x => x.TagId == 12).Select(c => new
                {
                    art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId)
                }).OrderBy(x => Guid.NewGuid()).Take(9).ToList();
                return Ok(new { status = 1, slider = slider.Skip(limit * offset).Take(limit), pages = slider.Count() });
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
            IEnumerable<ArtWork> result = _context.ArtWorks.FromSql("Sp_GetAllbyId @p0", Id).ToList();
            try
            {
                //var returnValue = _context.ArtWorks.Where(x => x.Id == Id && _context.Users.Any(c => c.Id == x.UserId)).Select(p => new ArtWorkView
                //{
                //    artwork = p,
                //    user = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                //    isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user),
                //    favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                //    tags = _context.ArtWithTags.Where(c => c.ArtId == p.Id).Select(v => new ArtTag
                //    {
                //        Id = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId).Id,
                //        Tag = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId).Tag
                //    }),
                //    keywords = _context.ArtKeywords.Where(v => v.ArtId == p.Id)

                //}).First() ?? new ArtWorkView();
                int ParentId = _context.Categories.Where(x => _context.ArtCategories.Any(tz => tz.Category == x.Id && tz.ArtId == result.First().Id)).First().ParentId;

                var returnValue = result.Where(x => x.Id == Id).Select(p => new ArtWorkView
                {
                    artwork = p,
                    user = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user),
                    favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                    tags = _context.ArtWithTags.Where(c => c.ArtId == p.Id).Select(v => new ArtTag
                    {
                        Id = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId).Id,
                        Tag = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId).Tag
                    }),
                    keywords = _context.ArtKeywords.Where(v => v.ArtId == p.Id),
                    CategorysId = _context.ArtCategories.Where(tx => tx.ArtId == p.Id).Select(tx => tx.Category).ToArray(),
                    Items = GetSubCategories(_context, ParentId, _context.ArtCategories.Where(tx => tx.ArtId == p.Id).Select(tx => tx.Category).ToArray())

                }).First() ?? new ArtWorkView();

                if (returnValue.artwork != null)
                {
                    var a = returnValue.artwork;
                    a.Views = a.Views + 1;
                    _context.ArtWorks.Update(a);
                    _context.SaveChanges();
                }

                var Catgories = GetCategory1(_context, 0, ParentId);
                var subCatgories = GetSubCategories(_context, ParentId, _context.ArtCategories.Where(tx => tx.ArtId == returnValue.artwork.Id).Select(tx => tx.Category).ToArray());

                //artView.CategorysId = _context.ArtCategories.Where(tx => tx.ArtId == id).Select(tx => tx.Category).ToArray();
                //artView.Items = GetSubCategories(_context, ParentId);

                var otherarts = _context.ArtWorks.Where(x => x.Category == returnValue.artwork.Category).OrderBy(x => Guid.NewGuid()).Take(5).ToList();
                if (returnValue == null) { return Ok(new { status = 0, message = "Not Found" }); }
                var userInfo = _context.Users.Where(c => c.Id == returnValue.artwork.UserId).First();
                var userProfile = _context.UserModel.Where(x => x.UserId == returnValue.artwork.UserId).First() ?? new UserModel();
                var userArtWorks = _context.ArtWorks.Where(x => x.UserId == returnValue.artwork.UserId).Take(5).OrderByDescending(c => c.AddedDate);
                if (user != null)
                {
                    var MessageReplies = _context.Messages.Where(x => x.ArtId == Id && x.FromUserId == user).Select(o => new
                    {
                        message = _context.Messages.Where(b => b.Id == o.Id),
                        replyies = _context.MessageReplies.Where(v => v.MessageId == o.Id)

                    });
                    return Ok(new
                    {
                        status = 1,
                        message = "success",
                        art = returnValue,
                        user = userInfo,
                        profile = userProfile,
                        artworks = userArtWorks,
                        messages = MessageReplies,
                        otherarts = otherarts,
                        Category = Catgories,
                        SubCategory = subCatgories
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
                foreach (var i in Posttags)
                {
                    var arta = new ArtWithTags();
                    arta.ArtId = Convert.ToInt32(i.artId);
                    arta.TagId = Convert.ToInt32(i.tagId);
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

        [HttpPost]
        [Route("api/artowrk/PostKeywords")]
        public IActionResult PostKeywords([FromBody]IEnumerable<ARtKeywords> Posttags)
        {
            try
            {
                foreach (var i in Posttags)
                {
                    var arta = new ARtKeywords();
                    arta.ArtId = Convert.ToInt32(i.ArtId);
                    arta.Keyword = i.Keyword;
                    _context.ArtKeywords.Add(arta);
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
        [Route("api/artowrk/RemoveKeywords")]
        public IActionResult RemoveKeywords(int Id, string tag)
        {

            try
            {
                var arta = _context.ArtKeywords.Where(x => x.ArtId == Id && x.Keyword == tag);
                if (arta == null)
                {
                    return Ok(new { status = 0, message = "Not Found" });
                }
                foreach (var i in arta)
                {
                    _context.ArtKeywords.Remove(i);
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
                var Fav = _context.ArtFavourites.Where(x => x.UserId == user && x.ArtId == Id);
                foreach (var i in Fav)
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
                var art = _context.ArtWorks.FirstOrDefault(x => x.Id == Id && _context.Users.Any(c => c.Id == x.UserId));
                if (art == null)
                {
                    return Ok(new { status = 0, message = "Not Found" });
                }
                var userInfo = _context.Users.Where(x => x.Id == art.UserId).Select(n => new
                {
                    userbio = _context.UserModel.FirstOrDefault(user => user.UserId == n.Id),
                    user = n
                }).First();
                art.Views = art.Views + 1;
                _context.ArtWorks.Update(art);
                _context.SaveChanges();
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
            return Ok(_context.ArtTags.Where(x => x.Type != "Admin" && x.Tag.ToLower().Contains(name.ToLower())));
        }
        [Route("api/artowrk/allTags")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult allTags(string name)
        {
            return Ok(_context.ArtTags.Where(x => x.Tag.ToLower().Contains(name.ToLower())));
        }

        [Route("api/artowrk/artbyname")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult artbyname(string name, int limit, int offset)
        {
            //var model = from a in _context.ArtWorks join b in _context.Categories on a.Category
            try
            {
                var result = _context.ArtWorks.FromSql("selectquery @p0", name).ToList().Select(c => new
                {
                    art = c,
                    favcount = _context.ArtFavourites.Where(x => x.ArtId == c.Id).Count(),
                    tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.Id).Select(v => new
                    {
                        Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                    }),
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.Id).UserId)
                });
                return Ok(new { result = result.Skip(limit * offset).Take(limit), pages = result.Count() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/artowrk/findyourart")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult findyourart([FromBody] FindYourArt name, int limit, int offset)
        {
            if (name.Prices == 1200)
            {
                var model = _context.ArtWorks.Where(x => name.CategoryId.Contains(x.Category) && _context.Users.Any(c => c.Id == x.UserId) && name.Orientation.Contains(x.Orientation) && x.Price > name.Prices).
                    Select(c => new
                    {
                        art = c,
                        UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.FirstOrDefault(art => art.Id == c.Id).UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == c.Id).Count(),
                        tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.Id).Select(v => new
                        {
                            Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                        }),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.Id).UserId)

                    }
                    );
                return Ok(new { result = model.Skip(limit * offset).Take(limit), pages = model.Count() });
            }
            else if (name.Orientation.Count() == 0)
            {
                var model = _context.ArtWorks.Where(x => name.CategoryId.Contains(x.Category) && _context.Users.Any(c => c.Id == x.UserId) && x.Price > name.Prices).
                    Select(c => new
                    {
                        art = c,
                        UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.FirstOrDefault(art => art.Id == c.Id).UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == c.Id).Count(),
                        tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.Id).Select(v => new
                        {
                            Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                        }),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.Id).UserId)

                    }
                    );
                return Ok(new { result = model.Skip(limit * offset).Take(limit), pages = model.Count() });
            }
            else
            {
                var model = _context.ArtWorks.Where(x => name.CategoryId.Contains(x.Category) && _context.Users.Any(c => c.Id == x.UserId) && _context.Users.Any(c => c.Id == x.UserId) && name.Orientation.Contains(x.Orientation) && x.Price < name.Prices).
                    Select(c => new
                    {
                        art = c,
                        UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.FirstOrDefault(art => art.Id == c.Id).UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == c.Id).Count(),
                        tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.Id).Select(v => new
                        {
                            Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                        }),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.Id).UserId)

                    });
                return Ok(new { result = model.Skip(limit * offset).Take(limit), pages = model.Count() });
            }

        }

        [Route("api/artowrk/getspecialtags")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getspecialtags(string name)
        {
            return Ok(_context.ArtTags.Where(x => x.Type.ToLower().Contains(name.ToLower())));
        }

        [Route("api/artowrk/getARticle")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getArticles(int limit, int offset)
        {
            var returnValue = _context.ArtArticles.ToList();

            return Ok(new { Month = GetMonth(), Articles = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count() });
            // return Ok(_context.ArtArticles.ToList());
        }

        public List<Tuple<string, string>> GetMonth()
        {
            var list = new List<Tuple<string, string>>();

            var Month = Enumerable.Range(0, 3).Select(i => new { M = DateTimeOffset.Now.AddMonths(-i).ToString("MM/yyyy") }).ToArray();

            for (int i = 0; i < Month.Length; i++)
            {
                var t = Month[i].M.Split('/');
                list.Add(Tuple.Create(Month[i].M, (CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(t[0])).Substring(0, 3) + " " + t[1])));
            }
            return list;
        }

        [HttpGet]
        [Route("api/artowrk/GetAllbyDate")]
        public IActionResult GetAllbyDate(string date, int limit, int offset)
        {
            var user = _userManager.GetUserId(User);
            try
            {
                var returnValue = _context.ArtWorks.Where(x => x.Status == 1 && x.AddedDate.ToString("MM/yyyy") == date).Select(p => new
                {
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
                    views = p.Views,
                    Title = p.Title,
                    Width = p.Width,
                    UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                    favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user),
                    tags = _context.ArtWithTags.Where(c => c.ArtId == p.Id).Select(v => new
                    {
                        Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                    })
                }).OrderByDescending(v => v.AddedDate);
                return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count(), Month = GetMonth() });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [Route("api/artowrk/getARticlebyDate")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getArticlesbyDate(string date)
        {
            return Ok(new { Month = GetMonth(), Articles = _context.ArtArticles.Where(x => x.AddedDate.ToString("MM/yyyy") == date).ToList() });
        }

        [Route("api/artowrk/getARticlebyId")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getArticlesbyId(int Id)
        {
            return Ok(_context.ArtArticles.Where(x => x.Id == Id).First());
        }

        [Route("api/artowrk/Categories")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getCategories()
        {
            return Ok(_context.Categories.Where(x => x.ParentId == 0).ToList());
        }

        [Route("api/artowrk/Subcategories")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getSubCategories(int id)
        {
            _context.Categories.ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Title });

            return Ok(_context.Categories.Where(x => x.ParentId == id).ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Title }));
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

        [Route("api/artowrk/Countries")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getCountries()
        {

            return Ok(_context.Country.OrderBy(x => x.CountryName).Distinct().ToList());
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
            string[] arr = Art.Category.Split(',');

            var user = _userManager.GetUserId(User);
            Art.UserId = user;
            Art.Status = 0;
            Art.AddedDate = DateTime.Now;
            Art.Category = arr[0];
            if (Art.Width == Art.Height)
                Art.Orientation = "Square";
            else if (Art.Width > Art.Height)
                Art.Orientation = "Landscape";
            else
                Art.Orientation = "Portrait";
            try
            {
                _context.ArtWorks.Add(Art);
                _context.SaveChanges();

                foreach (string i in arr)
                {
                    var Cat = new ArtCategory();
                    Cat.ArtId = Art.Id;
                    Cat.Category = Convert.ToInt32(i);
                    _context.ArtCategories.Add(Cat);
                }

                _context.SaveChanges();

                return Ok(new { status = 1, message = "Success", id = Art.Id });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/artowrk/tempupdateArt")]
        public IActionResult tempupdateart()
        {
            int i = 0;
            var arts = _context.ArtWorks.ToList();
            foreach (var Art in arts)
            {
                if (Art.Width == Art.Height)
                    Art.Orientation = "Square";
                else if (Art.Width > Art.Height)
                    Art.Orientation = "Landscape";
                else if (Art.Height > Art.Width)
                    Art.Orientation = "Portrait";

                try
                {
                    _context.ArtWorks.Update(Art);
                    i = i + 1;
                }
                catch (Exception ex)
                {
                    return Ok(new { status = 0, message = ex.Message });
                }
                _context.SaveChanges();

            }
            return Ok(new { status = 1, message = "Success", id = i });
            // return Ok(new { status = 0, message = "Final" });
        }

        [HttpPost]
        [Authorize]
        [Route("api/artowrk/updateArt")]
        public IActionResult updateArt([FromBody]ArtUploadModel Art)
        {
            var user = _userManager.GetUserId(User);
            var oldart = _context.ArtWorks.FirstOrDefault(x => x.Id == Art.Artwork.Id);
            if (oldart == null)
            {
                return Ok(new { status = 0, message = "Not Found" });
            }
            if (oldart.UserId != user)
            {
                return Ok(new { status = 0, message = "Access Denied" });
            }
            
            List<Int32> list = (_context.ArtCategories.Where(x => x.ArtId == Art.Artwork.Id).Select(x => x.Id).ToList());


            oldart.ArtCreationDate = Art.Artwork.AddedDate;
            oldart.Category = Art.Artwork.Category;
            oldart.Description = Art.Artwork.Description;
            oldart.DimensionUnit = Art.Artwork.DimensionUnit;
            oldart.Width = Art.Artwork.Width;
            oldart.Height = Art.Artwork.Height;
            oldart.MediumString = Art.Artwork.MediumString;
            oldart.Price = oldart.Price;
            oldart.Status = 0;
            oldart.Title = Art.Artwork.Title;
            oldart.AddedDate = DateTime.Now;
            if (Art.Artwork.Width == Art.Artwork.Height)
                oldart.Orientation = "Square";
            else if (Art.Artwork.Width > Art.Artwork.Height)
                oldart.Orientation = "Landscape";
            else
                oldart.Orientation = "Portrait";
            try
            {
                _context.ArtWorks.Update(oldart);
                var tags = _context.ArtWithTags.Where(x => x.ArtId == Art.Artwork.Id);
                foreach (var i in tags)
                {
                    _context.ArtWithTags.Remove(i);
                }
                _context.SaveChanges();

                for (int i = 0; i < list.Count(); i++)
                {
                    var Cat = new ArtCategory();
                    Cat.Id = list[i];
                    _context.ArtCategories.Remove(Cat);
                }
                _context.SaveChanges();
                foreach (string i in Art.SubCategrory)
                {
                    var Cat = new ArtCategory();
                    Cat.ArtId = Art.Artwork.Id;
                    Cat.Category = Convert.ToInt32(i);
                    _context.ArtCategories.Add(Cat);
                }

                _context.SaveChanges();

                return Ok(new { status = 1, message = "Success", id = Art.Artwork.Id });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/getbyArtdeals")]
        public IActionResult getbyArtdeals(int Id, int limit, int offset)
        {
            var user = _userManager.GetUserId(User);
            if (Id == 301)
            {
                try
                {
                    var returnValue = _context.ArtWithTags.Where(x => x.TagId == 10 && _context.ArtWorks.Any(v => v.Price >= Id && v.Id == x.ArtId &&
                    _context.Users.Any(c => c.Id == v.UserId))).Select(c => new
                    {
                        art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId),
                        UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == c.ArtId).Count(),
                        tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.ArtId).Select(v => new
                        {
                            Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                        }),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId)
                    }).ToList();
                    return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count() });
                }
                catch (Exception ex)
                {
                    return Ok(new { status = 0, message = ex.Message });
                }
            }
            try
            {
                var returnValue = _context.ArtWithTags.Where(x => x.TagId == 10 && _context.ArtWorks.Any(v => v.Price <= Id && v.Id == x.ArtId && _context.Users.Any(c => c.Id == v.UserId))).Select(c => new
                {
                    art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId),
                    UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId).First().FullName,
                    favcount = _context.ArtFavourites.Where(x => x.ArtId == c.ArtId).Count(),
                    tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.ArtId).Select(v => new
                    {
                        Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                    }),
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId)
                }).ToList();
                return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count() });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }

        }

        [HttpPost]
        [Authorize]
        [Route("api/artowrk/UpdateViewOnArt")]
        public IActionResult UpdateViewOnArt(int id)
        {
            try
            {
                var oldart = _context.ArtWorks.FirstOrDefault(x => x.Id == id);
                if (oldart == null)
                {
                    return Ok(new { status = 0, message = "Not Found" });
                }
                oldart.Views = oldart.Views + 1;
                _context.ArtWorks.Update(oldart);

                _context.SaveChanges();
                return Ok(new { status = 1, message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("api/artowrk/deleteArt")]
        public IActionResult deleteArt(int Id)
        {
            var user = _userManager.GetUserId(User);
            var oldart = _context.ArtWorks.FirstOrDefault(x => x.Id == Id);
            if (oldart == null)
            {
                return Ok(new { status = 0, message = "Not Found" });
            }
            if (oldart.UserId != user)
            {
                return Ok(new { status = 0, message = "Access Denied" });
            }
            oldart.Status = 55;
            oldart.AddedDate = DateTime.Now;

            try
            {
                _context.ArtWorks.Update(oldart);
                var tags = _context.ArtWithTags.Where(x => x.ArtId == Id);
                foreach (var i in tags)
                {
                    _context.ArtWithTags.Remove(i);
                }
                _context.SaveChanges();
                return Ok(new { status = 1, message = "Success", id = Id });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [Route("api/artowrk/Country")]
        [HttpGet]
        public IActionResult getCountry()
        {
            var list = _context.Users.Select(x => new
            {
                country = x.Country
            }).ToList().OrderBy(b => b.country).Distinct();
            return Ok(list);
        }

        [HttpGet]
        [Route("api/artowrk/getAllTagsbyPag")]
        public IActionResult getAllTagsbyPag(int Id, int limit, int offset)
        {
            var user = _userManager.GetUserId(User);

            try
            {
                var returnValue = _context.ArtWithTags.Where(x => x.TagId == Id && _context.ArtWorks.Any(art => art.Id == x.ArtId && _context.Users.Any(c => c.Id == art.UserId))).Select(c => new
                {
                    art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId),
                    UserId = _context.Users.Where(n => n.Id == _context.ArtWorks.First(art => art.Id == c.ArtId).UserId).First().FullName,

                    favcount = _context.ArtFavourites.Where(x => x.ArtId == c.ArtId).Count(),
                    tags = _context.ArtWithTags.Where(xc => xc.ArtId == c.ArtId).Select(v => new
                    {
                        Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                    }),
                    isfav = _context.ArtFavourites.Any(x => x.ArtId == c.Id && x.UserId == _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId).UserId)
                }).ToList();

                return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count() });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }

        }


        [HttpGet]
        [Route("api/artowrk/GetRandomArt")]
        public IActionResult GetRandomArt(int Id, int limit, int offset)
        {
            //10 - deals , 11 - collection , 12 - slider , 55 - Find your Art
            try
            {
                var slider = _context.ArtWithTags.Where(x => x.TagId == 55 && _context.ArtWorks.Any(t => t.Id == x.ArtId)).Select(c => new
                {
                    art = _context.ArtWorks.FirstOrDefault(art => art.Id == c.ArtId && _context.Users.Any(cc => cc.Id == art.UserId))
                }).ToList();

                return Ok(new { status = 1, slider = slider.Skip(limit * offset).Take(limit), pages = slider.Count() });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/artowrk/getbycategory1")]
        public IActionResult getbycategory1(int Id, int limit, int offset)
        {
            var user = _userManager.GetUserId(User);
            IEnumerable<Category> result = _context.Categories.FromSql("catsLevel2 @p0", Id).ToList();

            IEnumerable<ArtWork> artwok = _context.ArtWorks.FromSql("Sp_GetArtCategory @p0", Id).ToList();
            try
            {
                if (result != null)
                {
                    var model = artwok.Select(p => new
                    {
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
                        views = p.Views,
                        Width = p.Width,
                        tags = _context.ArtWithTags.Where(xc => xc.ArtId == p.Id).Select(v => new
                        {
                            Tags = _context.ArtTags.FirstOrDefault(b => b.Id == v.TagId)
                        }),
                        UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == user)
                    }).OrderByDescending(v => v.AddedDate);

                    var returnValue = (from a in result join b in model on a.Id.ToString() equals b.Category group b by b.Id into bg select bg.FirstOrDefault());
                    var subcategories = _context.Categories.Where(x => x.ParentId == Id);
                    var categorytitle = _context.Categories.FirstOrDefault(x => x.Id == Id).Title;

                    return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), subcategories = subcategories, categorytitle = categorytitle, pages = returnValue.Count() });
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



        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetCategory1(ApplicationDbContext _context, int id, int val)
        {
            var model = _context.Categories.Where(x => x.ParentId == id).ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Title, Selected = (x.Id == val) });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }

        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetSubCategories(ApplicationDbContext _context, int id, int[] ArtId)
        {
            var model = _context.Categories.Where(x => x.ParentId == id).ToList().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Title, Selected = (ArtId.Contains(x.Id)) });
            List<SelectListItem> Category = new List<SelectListItem>(model);
            return Category;
        }
    }
}