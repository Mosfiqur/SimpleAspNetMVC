using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleASPNetMVC.Models;
using SimpleASPNetMVC.Services;

namespace SimpleASPNetMVC.Controllers
{
    public class PrintController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<PrintController> _logger;

        public PrintController(IStudentService studentService, ILogger<PrintController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = Constants.AuthPolicyName)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Constants.AuthPolicyName)]
        public async Task<IActionResult> SendToPrint(PrintViewModel model)
        {
            var filePath = string.Empty;
            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    ModelState.AddModelError("File", "Please select a file to print.");
                }
                else if (Path.GetExtension(model.File.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("File", "Only PDF files are allowed.");
                }

                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }

                // Process the file and number of copies    

                filePath = Path.Combine(Path.GetTempPath(), "FileToPrint.pdf"); 

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                //Perform Quota Validation 
                var email = Util.GetLoggedInUserEmail(User);
                if (await _studentService.HaveEnoughQuota(email, model.NumberOfCopies) == false)
                {
                    ModelState.AddModelError(string.Empty, "You don't have enough quota.");
                    return View("Index", model);
                }

                bool isDeducted = await _studentService.DeductBalance(email, model.NumberOfCopies);

                if (isDeducted)
                {
                    // Return the PDF file to the client browser
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    var fileName = Path.GetFileName(filePath);
                    return File(fileBytes, "application/pdf", fileName);
                }
                else 
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while deducting the student's balance");
                    return View("Index", model);
                }
            }
            finally
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath); 
                }
            }

            //ViewBag.Message = "File sent to print successfully!";
            //return View("Index");
        }
    }
}

