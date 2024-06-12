using SimpleASPNetMVC.DbModels;

namespace SimpleASPNetMVC.Services
{
    public interface IAuthService
    {
        Task<Student?> SignInAsync(HttpContext httpContext,  string email, string password);
        Task SignOutAsync(HttpContext httpContext);
        Task<Student> GetStudent(string email); 
    }
}
