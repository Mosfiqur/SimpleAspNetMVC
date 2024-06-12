using System.ComponentModel.DataAnnotations;

namespace SimpleASPNetMVC.Models
{
    public class PrintViewModel
    {
        [Required(ErrorMessage = "Please select a pdf file to print.")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "pdf", ErrorMessage = "Only PDF files are allowed.")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Please specify the number of copies.")]
        [Range(1, int.MaxValue, ErrorMessage = "The number of copies must be a positive integer.")]
        public int NumberOfCopies { get; set; }
    }
}
