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
                    userbio = _context.UserModel.Where(x => x.UserId == o.Id)
                }).ToList();
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
        public void Post([FromBody]UserModel value)
        {

        }
        
        // PUT: api/User/5
        [HttpPost("{id}")]
        [Route("~/user/updateuserinfo")]
        public IActionResult Put(int id, [FromBody]UserModel value)
        {
            try { 
            var userId = _userManager.GetUserId(User);
            var UserBio = _context.UserModel.FirstOrDefault(x => x.UserId == userId);
                if (UserBio == null)
                {
                    value.UserId = userId;
                    _context.UserModel.Add(UserBio);
                    _context.SaveChanges();
                    return Ok(new { status = 2, message = "Added Successfully" });
                }
            UserBio = value;
            _context.UserModel.Update(UserBio);
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
