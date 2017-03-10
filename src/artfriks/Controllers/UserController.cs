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
        [HttpGet]
        [Route("~/user/userinfo")]
        public IActionResult Get()
        {try
            {
                var user = _userManager.GetUserId(User);
                var returnValue = _context.Users.Where(x => x.Id == user).Select(o => new
                {
                    user = o,
                    userbio = _context.UserModel.Where(x => x.UserId == o.Id).First()
                }).First();
                return Ok( new { status =1, message=returnValue});
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
            try { 
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
                var Message = _context.MessageReplies.FirstOrDefault(x=>x.Id==Id);
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
            try { 
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
    }
}
