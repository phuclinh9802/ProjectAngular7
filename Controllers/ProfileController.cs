using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterLogin.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegisterLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : Controller
    {
        private UserManager<Register> _userManager;
        private readonly RegisterContext _context;
        
        public ProfileController(UserManager<Register> userManager, RegisterContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        //[Authorize(Roles = Roles.Admin)]
        //[HttpGet]
        //public async Task<Object> GetUsersAsync()
        //{
        //    using (var context = new RegisterContext())
        //    {
        //        return await context.Register.ToListAsync();
        //    }
        //}
        [HttpGet]
       [Authorize]
       //GET: api/Profile
       public async Task<Object> GetProfile()
       {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user.Role == Roles.Admin)
            {
                var x = await _context.Register.ToListAsync();
                return x;
            }
            else if (user.Role == Roles.User)
            {
                var x = await _context.Register.Where(y => y.UserName == user.UserName).ToListAsync();
                //return new
                //{
                //    user.FullName,
                //    user.Email,
                //    user.UserName,
                //    user.Role
                //};
                return x;

            }
            else
            {
                return BadRequest(new { message = "Registration Error!" });
            }
            //}
            //return Ok();
        }
        
    }
}
