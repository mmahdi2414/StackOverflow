using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MTProject.Models;
using MTProject.Models.Response.Users;
using MTProject.Models.Request.Users;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTProject.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly JWT jwt;
        private readonly UserManager<User> userManager;
        public UsersController(UserManager<User> _userManager,IConfiguration configuration)
        {
            userManager = _userManager;
            jwt = new JWT(configuration);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("SignUp")]
        public async Task<UserResponse> PostUser([FromBody]PostUserRequest request)
        {
            var appUser = userManager.FindByNameAsync(request.Email);

            if (appUser.Result != null)
                return new UserResponse { Data = null, Message = "ایمیل وارد شده توسط فرد دیگری استفاده می شود" };

            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = request.Password,
                UserName = request.Email
            };
            var result = await userManager.CreateAsync(user);

            if (!result.Succeeded)
                return new UserResponse { Data = null, Message = "اطلاعات با عدم موفقیت پایان یافت" };

            string[] roles = { "User" };
            string jwtString = jwt.GenerateJwtToken(user, roles);
            await userManager.SetAuthenticationTokenAsync(user, "", "jwt", jwtString);
            await userManager.AddToRoleAsync(user, "User");
            return new UserResponse { Message = "اطلاعات با موفقیت ثبت شد", Data = jwtString };
        }
        [Route("SignIn")]
        public UserResponse SignIn([FromQuery] string Email,[FromQuery] string password)
        {
            var user = userManager.Users.SingleOrDefault(s => s.Email == Email && s.PasswordHash == password);
            if(user == null)
            {
                return new UserResponse { Data = null, Message = "اطلاعات کاربری یافت نشد" };
            }
            string jwtToken = jwt.GenerateJwtToken(user, new string[] { "User" });
            return new UserResponse { Data = jwtToken, Message = "ورود با موفقیت انجام شد" };
        }
        
        [Route("LoginPage")]
        public IActionResult LoginPage()
        {
            return View("login");
        }
        [Route("Home")]
        public IActionResult HomePage()
        {
            return View("index");
        }

    }
}
