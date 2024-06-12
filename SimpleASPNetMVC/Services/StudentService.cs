using Microsoft.EntityFrameworkCore;
using SimpleASPNetMVC.Data;
using SimpleASPNetMVC.DbModels;
using SimpleASPNetMVC.DTO;
using SimpleASPNetMVC.Models;

namespace SimpleASPNetMVC.Services
{
    public class StudentService : IStudentService
    {
        private readonly ILogger<StudentService> _logger;
        private readonly StudentContext _context;

        public StudentService(ILogger<StudentService> logger, StudentContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> AddQuotaByUid(int studentId, int quota)
        {
            // Find the student by id
            var student = await _context.Students
                .Include(s => s.Fund)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student != null)
            {
                // Update the student's fund balance 
                var costForNewQuota = quota * Constants.OneAFourToCHF; 
                student.Fund.Balance += costForNewQuota;
                student.Fund.Quota += quota;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Transfer successful
                return true;
            }
            else
            {
                // Student not found
                return false;
            }
        }

        public async Task<bool> AddQuotaByUserName(string email, int quota)
        {
            var student = await _context.Students
                                .Include(s => s.Fund)
                                .FirstOrDefaultAsync(x => x.Email == email);
            if (student != null)
            {
                // Update the student's fund balance 
                var costForNewQuota = quota * Constants.OneAFourToCHF;
                student.Fund.Balance += costForNewQuota;
                student.Fund.Quota += quota;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Transfer successful
                return true;
            }
            else
            {
                // Student not found
                return false;
            }
        }

        public async Task<bool> CreateStudent(StudentDto studentDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(); 
            try
            {
                var student = new Student
                {
                    FirstName = studentDto.FirstName,
                    LastName = studentDto.LastName,
                    Password = studentDto.Password,
                    Email = studentDto.Email
                };
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                var studentFund = new StudentFund
                {
                    StudentId = student.Id,
                    Balance = (decimal)studentDto.FundAmount,
                    Quota = (int)((decimal)studentDto.FundAmount / Constants.OneAFourToCHF)
                }; 
                _context.Funds.Add(studentFund); 
                await _context.SaveChangesAsync();   

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {

                return false; 
            }

        }

        public async Task<bool> DeductBalance(string email, int noOfCopies)
        {
            var student = await _context.Students
                                .Include(s => s.Fund)
                                .FirstOrDefaultAsync(x => x.Email == email);
            if (student == null)
            {
                return false;
            }

            var costOfPapers = noOfCopies * Constants.OneAFourToCHF; 
            student.Fund.Balance -= costOfPapers;
            student.Fund.Quota -= noOfCopies;
            await _context.SaveChangesAsync(); 
            return true; 
        }

        public async Task<StudentViewModel?> GetStudent(string email)
        {
            var student = await _context.Students
                                .Include(s => s.Fund) 
                                .FirstOrDefaultAsync(x => x.Email == email); 
            if (student == null)
            {
                return null;
            }

            return new StudentViewModel
            {
                Id = student.Id, 
                FirstName = student.FirstName, 
                LastName = student.LastName, 
                Email = student.Email, 
                Balance = student.Fund.Balance, 
                Quota = student.Fund.Quota 
            };
        }

        public async Task<Student?> GetStudentById(int studentId)
        {
            return _context.Students.FirstOrDefault(s => s.Id == studentId);
        }

        public async Task<string> GetUserNameByUid(int studentId) // Return User Email as UserName
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId); 
            if(student == null)
            {
                return string.Empty; 
            }
            return student.Email;            
        }

        public async Task<bool> HaveEnoughQuota(string email, int noOfCopies)
        {
            var student = await _context.Students
                                .Include(s => s.Fund)
                                .FirstOrDefaultAsync(x => x.Email == email);
            if (student == null)
            {
                return false;
            }

            if (noOfCopies > student.Fund.Quota) 
            {
                return false;
            }

            return true; 
        }

        public async Task<bool> TransferFundAsync(string email, double fundToTransfer)
        {
            // Find the student by email
            var student = await _context.Students
                .Include(s => s.Fund)
                .FirstOrDefaultAsync(s => s.Email == email);

            if (student != null)
            {
                // Update the student's fund balance
                student.Fund.Balance += (decimal)fundToTransfer;
                var newQuota = (int)(student.Fund.Balance / Constants.OneAFourToCHF); 
                student.Fund.Quota = newQuota; 

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Transfer successful
                return true;
            }
            else
            {
                // Student not found
                return false;
            }
        }

        public async Task<bool> TransferFundByIdAsync(int studentId, double fundToTransfer)
        {
            // Find the student by id
            var student = await _context.Students
                .Include(s => s.Fund)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student != null)
            {
                // Update the student's fund balance
                student.Fund.Balance += (decimal)fundToTransfer;
                var newQuota = (int)(student.Fund.Balance / Constants.OneAFourToCHF);
                student.Fund.Quota = newQuota;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Transfer successful
                return true;
            }
            else
            {
                // Student not found
                return false;
            }
        }
    }
}
