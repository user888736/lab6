using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class AccountController : Controller
    {

        private readonly List<UserModel> users;
        public AccountController()
        {
            //Sample Users Data, it can be fetched with the use of any ORM
            users = new List<UserModel>
            {
                new UserModel() { UserId = 1, Username = "adminek", Password = "123", Role = "Admin", Email = "admin@domain.com" },
                new UserModel() { UserId = 2, Username = "userek", Password = "123", Role = "User", Email = "user1@domain.com" }
            };
        }

        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginModel objLoginModel = new()
            {
                ReturnUrl = ReturnUrl
            };
            return View(objLoginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel objLoginModel)
        {
            if (ModelState.IsValid)
            {
                //try match Login model to User model
                var user = users.Where(x => x.Username == objLoginModel.UserName && x.Password == objLoginModel.Password).FirstOrDefault();
                if (user == null)
                {
                    ViewBag.Message = "Provided credential is not valid.";
                    return View(objLoginModel);
                }
                else
                {
                    //A claim is a statement about a subject by an issuer and
                    //represent attributes of the subject that are useful in the context of authentication and authorization operations.
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email )
                    };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
                    var principal = new ClaimsPrincipal(identity);
                    //Signin user and create cookie
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties() { IsPersistent = objLoginModel.RememberLogin }
                        );

                    return LocalRedirect(objLoginModel.ReturnUrl);
                }
            }
            return View(objLoginModel);
        }

        public async Task<IActionResult> LogOut()
        {
            //logout and remove cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }
    }
}

