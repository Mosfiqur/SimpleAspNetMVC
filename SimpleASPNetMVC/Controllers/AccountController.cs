using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleASPNetMVC.Models;
using SimpleASPNetMVC.Services;
using System.Security.Claims;

namespace SimpleASPNetMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet] 
        public IActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loggedInStudent = await _authService.SignInAsync(HttpContext, model.Email, model.Password); 
            if (loggedInStudent != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, $"{loggedInStudent.FirstName} {loggedInStudent.LastName}"), 
                    new Claim(ClaimTypes.Email, loggedInStudent.Email) 
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);    
                var principal = new ClaimsPrincipal(identity); 
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal); 
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Perform logout actions such as clearing authentication cookies
            // Redirect to login page or desired location
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
