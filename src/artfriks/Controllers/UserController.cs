using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using artfriks.Data;
using Microsoft.AspNetCore.Identity;
using artfriks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace artfriks.Controllers
{
    [Produces("application/json")]
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        // GET: api/User
        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        [Authorize]
        [HttpGet]
        [Route("~/user/userinfo")]
        public IActionResult Get()
        {
            try
            {
                var user = _userManager.GetUserId(User);
                var returnValue = _context.Users.Where(x => x.Id == user).Select(o => new
                {
                    user = o,
                    userbio = _context.UserModel.Where(x => x.UserId == o.Id).First() ?? new UserModel()
                }).First();
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/user/userinfoById")]
        public IActionResult userinfoById(string Id)
        {
            try
            {
                var returnValue = _context.Users.Where(x => x.Id == Id && _context.ArtWorks.Any(b => b.UserId == x.Id)).Select(o => new
                {
                    user = o,
                    userbio = _context.UserModel.Where(x => x.UserId == o.Id).First() ?? new UserModel(),
                    arts = _context.ArtWorks.Where(x => x.UserId == Id).Select(p => new
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
                        Width = p.Width,
                        UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == Id)
                    }).OrderByDescending(v => v.AddedDate)
                }).First();
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/user/userinfoByName")]
        public IActionResult userinfoByName(string alpha)
        {
            try
            {
                var returnValue = _context.Users.Where(x => x.FullName.ToLower().StartsWith(alpha.ToLower()) && _context.ArtWorks.Any(b => b.UserId == x.Id)).Select(o => new
                {
                    user = o,
                    userbio = _context.UserModel.Where(x => x.UserId == o.Id).First() ?? new UserModel(),
                    arts = _context.ArtWorks.Where(x => x.UserId == o.Id).Select(p => new
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
                        Width = p.Width,
                        UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == o.Id)
                    }).OrderByDescending(v => v.AddedDate).Take(3),
                    maxprice = _context.ArtWorks.Where(v => v.UserId == o.Id).Max(c => c.Price),
                    minprice = _context.ArtWorks.Where(v => v.UserId == o.Id).Min(c => c.Price)
                });
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/user/userinfoByLocation")]
        public IActionResult userinfoByLocation(string alpha)
        {
            try
            {
                var returnValue = _context.Users.Where(x => x.Country.ToLower().StartsWith(alpha.ToLower()) && _context.ArtWorks.Any(b => b.UserId == x.Id)).Select(o => new
                {
                    user = o,
                    userbio = _context.UserModel.Where(x => x.UserId == o.Id).First() ?? new UserModel(),
                    arts = _context.ArtWorks.Where(x => x.UserId == o.Id).Select(p => new
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
                        Width = p.Width,
                        UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                        favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                        isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == o.Id)
                    }).OrderByDescending(v => v.AddedDate).Take(3),
                    maxprice = _context.ArtWorks.Where(v => v.UserId == o.Id).Max(c => c.Price),
                    minprice = _context.ArtWorks.Where(v => v.UserId == o.Id).Min(c => c.Price)
                });
                return Ok(new { status = 1, message = returnValue });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        [HttpPost]
        [Route("~/user/sendMessage")]
        public IActionResult Post([FromBody]Messages value)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                value.FromUserId = userId;
                value.AddedDate = DateTime.Now;
                value.ToUserId = _context.ArtWorks.FirstOrDefault(x => x.Id == value.ArtId).UserId ?? "";
                _context.Messages.Add(value);
                _context.SaveChanges();
                return Ok(new { status = 1, message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        // Get: api/User
        [HttpGet]
        [Route("~/user/getMessage")]
        public IActionResult getMessage()
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                var message = _context.Messages.Where(x => x.ToUserId == userId || x.FromUserId == userId).Select(m => new
                {
                    message = m,
                    from = _context.Users.FirstOrDefault(x => x.Id == m.FromUserId).UserName,
                    to = _context.Users.FirstOrDefault(x => x.Id == m.ToUserId).UserName,
                    replies = _context.MessageReplies.Where(x => x.MessageId == m.Id).Select(mo => new
                    {
                        message = mo,
                        from = _context.Users.FirstOrDefault(x => x.Id == mo.UserId).UserName
                    })
                });

                return Ok(new { status = 1, message = message });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        // Get: api/User
        [HttpGet]
        [Route("~/user/getreplyMessage")]
        public IActionResult getreplyMessage(int id)
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                var message = _context.MessageReplies.Where(x => x.MessageId == id).Select(m => new
                {
                    message = m,
                    from = _context.Users.FirstOrDefault(x => x.Id == m.UserId).UserName
                });

                return Ok(new { status = 1, message = message });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        // POST: api/User
        [HttpPost]
        [Route("~/user/ReplyMessage")]
        public IActionResult PostReply([FromBody]MessageReplies value)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                value.UserId = userId;
                value.AddedDate = DateTime.Now;
                _context.MessageReplies.Add(value);
                _context.SaveChanges();
                return Ok(new { status = 1, message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("~/user/deleteReplyMessage")]
        public IActionResult deleteReplyMessage(int Id)
        {
            try
            {
                var Message = _context.MessageReplies.FirstOrDefault(x => x.Id == Id);
                if (Message == null)
                {
                    return Ok(new { status = 0, message = "Not Found" });
                }
                _context.MessageReplies.Remove(Message);
                _context.SaveChanges();
                return Ok(new { status = 1, message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("~/user/deleteMessage")]
        public IActionResult deleteMessage(int Id)
        {
            try
            {
                var Message = _context.Messages.FirstOrDefault(x => x.Id == Id);
                if (Message == null)
                {
                    return Ok(new { status = 0, message = "Not Found" });
                }
                _context.Messages.Remove(Message);
                _context.SaveChanges();
                return Ok(new { status = 1, message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }
        // PUT: api/User/5
        [HttpPost("{id}")]
        [Route("~/user/updateuserinfo")]
        public IActionResult Put(int id, [FromBody]UserModel value)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var UserBio = _context.UserModel.Any(x => x.UserId == userId);
                if (UserBio == false)
                {
                    value.UserId = userId;
                    _context.UserModel.Add(value);
                    _context.SaveChanges();
                    return Ok(new { status = 2, message = "Added Successfully" });
                }

                _context.Entry(value).State = EntityState.Modified;
                /* var UserBio2 = _context.UserModel.Where(x => x.UserId == userId).First();

                 UserBio2 = value;
             _context.UserModel.Update(UserBio2);*/
                _context.SaveChanges();
                return Ok(new { status = 1, message = "Updated Successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpPost("{id}")]
        [Route("~/user/updateuserprofile")]
        public IActionResult PutApplicationUser(int id, [FromBody]ApplicationUser value)
        {
            try
            {
                var userId = _userManager.GetUserName(User);
                var UserBio = _context.Users.FirstOrDefault(x => x.UserName == userId);
                if (UserBio == null)
                {
                    _context.Users.Add(UserBio);
                    _context.SaveChanges();
                    return Ok(new { status = 2, message = "Added Successfully" });
                }
                UserBio = value;
                _context.Users.Update(UserBio);
                _context.SaveChanges();
                return Ok(new { status = 1, message = "Updated Successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("~/user/AlluserinfoByName")]
        public IActionResult AlluserinfoByName(int limit, int offset, string alpha)
        {

            try
            {
                if (alpha == "" || alpha == null || alpha == String.Empty)
                {
                    var returnValue = _context.Users.Where(x => _context.ArtWorks.Any(b => b.UserId == x.Id)).Select(o => new
                    {
                        user = o,
                        userbio = _context.UserModel.Where(x => x.UserId == o.Id).First() ?? new UserModel(),
                        arts = _context.ArtWorks.Where(x => x.UserId == o.Id && _context.Users.Any(v => v.Id == x.UserId)).Select(p => new
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
                            Width = p.Width,
                            UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                            favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                            isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == o.Id)
                        }).OrderByDescending(v => v.AddedDate).Take(3),
                        maxprice = _context.ArtWorks.Where(v => v.UserId == o.Id && _context.Users.Any(n => n.Id == v.UserId)).Max(c => c.Price),
                        minprice = _context.ArtWorks.Where(v => v.UserId == o.Id && _context.Users.Any(n => n.Id == v.UserId)).Min(c => c.Price)
                    });
                    return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count() });
                }
                else
                {
                    var returnValue = _context.Users.Where(x => x.FullName.ToLower().StartsWith(alpha.ToLower()) && _context.ArtWorks.Any(b => b.UserId == x.Id)).Select(o => new
                    {
                        user = o,
                        userbio = _context.UserModel.Where(x => x.UserId == o.Id).First() ?? new UserModel(),
                        arts = _context.ArtWorks.Where(x => x.UserId == o.Id).Select(p => new
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
                            Width = p.Width,
                            UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                            favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                            isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == o.Id)
                        }).OrderByDescending(v => v.AddedDate).Take(3),
                        maxprice = _context.ArtWorks.Where(v => v.UserId == o.Id).Max(c => c.Price),
                        minprice = _context.ArtWorks.Where(v => v.UserId == o.Id).Min(c => c.Price)
                    });
                    return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count() });
                }


            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/user/AlluserinfoByLocation")]
        public IActionResult AlluserinfoByLocation(int limit, int offset, string alpha)
        {
            try
            {
                if (alpha == "" || alpha == null || alpha == String.Empty)
                {
                    var returnValue = _context.Users.Where(x => _context.ArtWorks.Any(b => b.UserId == x.Id)).Select(o => new
                    {
                        user = o,
                        userbio = _context.UserModel.Where(x => x.UserId == o.Id).First() ?? new UserModel(),
                        arts = _context.ArtWorks.Where(x => x.UserId == o.Id && _context.Users.Any(n => n.Id == x.UserId)).Select(p => new
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
                            Width = p.Width,
                            UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                            favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                            isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == o.Id)
                        }).OrderByDescending(v => v.AddedDate).Take(3),
                        maxprice = _context.ArtWorks.Where(v => v.UserId == o.Id && _context.Users.Any(n => n.Id == v.UserId)).Max(c => c.Price),
                        minprice = _context.ArtWorks.Where(v => v.UserId == o.Id && _context.Users.Any(n => n.Id == v.UserId)).Min(c => c.Price)
                    });
                    return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count() });
                }
                else
                {
                    var returnValue = _context.Users.Where(x => x.Country.ToLower().StartsWith(alpha.ToLower()) && _context.ArtWorks.Any(b => b.UserId == x.Id)).Select(o => new
                    {
                        user = o,
                        userbio = _context.UserModel.Where(x => x.UserId == o.Id).First() ?? new UserModel(),
                        arts = _context.ArtWorks.Where(x => x.UserId == o.Id).Select(p => new
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
                            Width = p.Width,
                            UserId = _context.Users.Where(n => n.Id == p.UserId).First().FullName,
                            favcount = _context.ArtFavourites.Where(x => x.ArtId == p.Id).Count(),
                            isfav = _context.ArtFavourites.Any(x => x.ArtId == p.Id && x.UserId == o.Id)
                        }).OrderByDescending(v => v.AddedDate).Take(3),
                        maxprice = _context.ArtWorks.Where(v => v.UserId == o.Id).Max(c => c.Price),
                        minprice = _context.ArtWorks.Where(v => v.UserId == o.Id).Min(c => c.Price)
                    });
                    return Ok(new { status = 1, message = returnValue.Skip(limit * offset).Take(limit), pages = returnValue.Count() });
                }



            }
            catch (Exception ex)
            {
                return Ok(new { status = 0, message = ex.Message });
            }
        }
    }



}
