using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RegisterLogin.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegisterLogin.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : Controller
    {
        private UserManager<Register> _userManager;
        private SignInManager<Register> _signInManager;
        private readonly AppSettings _appSettings;
        
        public ApplicationUserController(UserManager<Register> userManager, SignInManager<Register> signInManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        //Post: /api/Register/Register
        public async Task<Object> PostApplicationUser(UserModel model)
        {
            var reg = new Register()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                Role = model.Role

            };
                try
                {
                    var result = await _userManager.CreateAsync(reg, model.Password);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    throw e;
                }
        }
       
        

        [HttpPost]
        [Route("Login")]
        //POST: api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel login) {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user,login.Password)) {
                var tokenDescript = new SecurityTokenDescriptor{
                    Subject = new ClaimsIdentity(new Claim[]{
                        new Claim("UserID",user.Id.ToString())
                }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_SECRET))
                    ,SecurityAlgorithms.HmacSha256Signature )
                } ;
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescript);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new {token});
            }
            else {
                return BadRequest(new {message = "Username/Password is incorrect. Please try again!"});
            }
        }
        // GET
        //<controller>      
    }
}
