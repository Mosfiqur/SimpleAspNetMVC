using System.ComponentModel.DataAnnotations;

namespace SimpleASPNetMVC.DTO
{
    public class StudentDto
    {
        [Required(ErrorMessage = "First Name is required")] 
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Fund amount is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Fund amount must be greater than zero")] 
        public double FundAmount { get; set; }
        [Required(ErrorMessage = "Password is required")] 
        public string Password { get; set; }
    }
}
