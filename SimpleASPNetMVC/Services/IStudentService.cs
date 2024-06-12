using SimpleASPNetMVC.DbModels;
using SimpleASPNetMVC.DTO;
using SimpleASPNetMVC.Models;

namespace SimpleASPNetMVC.Services
{
    public interface IStudentService
    {
        Task<StudentViewModel?> GetStudent(string email);
        Task<bool> HaveEnoughQuota(string email, int noOfCopies);
        Task<bool> DeductBalance(string email, int noOfCopies);
        Task<bool> TransferFundAsync(string email, double fundToTransfer);
        Task<bool> TransferFundByIdAsync(int studentId, double fundToTransfer);
        Task<bool> AddQuotaByUid(int studentId, int quota);
        Task<bool> AddQuotaByUserName(string email, int quota);
        Task<string> GetUserNameByUid(int studentId);   
        Task<Student?> GetStudentById(int studentId);
        Task<bool> CreateStudent(StudentDto studentDto);
    }
}
