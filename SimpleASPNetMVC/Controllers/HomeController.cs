using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleASPNetMVC.DbModels;
using SimpleASPNetMVC.Models;
using SimpleASPNetMVC.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace SimpleASPNetMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthService _authService;
        private readonly IStudentService _studentService;

        public HomeController(ILogger<HomeController> logger, IAuthService authService, IStudentService studentService)
        {
            _logger = logger;
            _authService = authService;
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(Policy = Constants.AuthPolicyName)]
        public async Task<IActionResult> Index()
        {
            var email = Util.GetLoggedInUserEmail(User);

            StudentViewModel viewModel = new StudentViewModel(); 
            if(!string.IsNullOrEmpty(email))
            {
                viewModel = await _studentService.GetStudent(email);
            }
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
