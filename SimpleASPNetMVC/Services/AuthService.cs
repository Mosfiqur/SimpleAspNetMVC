
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SimpleASPNetMVC.Data;
using SimpleASPNetMVC.DbModels;

namespace SimpleASPNetMVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger _logger;
        private readonly StudentContext _context;

        public AuthService(StudentContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<Student> GetStudent(string email)
        {
            return _context.Students.FirstAsync(s => s.Email == email); 
        }

        public Task<Student?> SignInAsync(HttpContext httpContext, string email, string password)
        {
            return _context.Students.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
