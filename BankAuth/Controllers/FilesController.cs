using BankAuth.Context;
using BankAuth.Models;
using BankAuth.Services;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net.Mail;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace BankAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AppDbContext _authContext;
        private readonly IEmailService _emailService;

        public FilesController(IWebHostEnvironment hostingEnvironment, AppDbContext authcontext, IEmailService emailService)
        {
            _hostingEnvironment = hostingEnvironment;
            _authContext = authcontext;
            _emailService = emailService;
        }



        [HttpPost]
        [DisableRequestSizeLimit]

        public async Task<IActionResult> UploadFiles()
        {
            try
            {
                var files = Request.Form.Files;

                var accountNum = Request.Form["accountNumber"];

                var loanType = Request.Form["loanType"];

                if (files == null || files.Count == 0)
                    return BadRequest(new { Message = "No files uploaded." });

                var fileUrls = new List<string>();
                var fileNames = new List<string>();
                var total_file_string = new string("");
                var total_file_names = new string("");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                        fileUrls.Add(fileUrl);
                        fileNames.Add(fileName);
                        total_file_string = string.Join(",", fileUrls);
                        total_file_names += string.Join(",", fileNames);

                    }
                }
              
                var newDocument = new Document
                {
                    AccountNum = accountNum,
                    FileName = total_file_names,
                    FilePath = total_file_string,
                    LoanType = loanType
                };

                _authContext.Documents.Add(newDocument);
                await _authContext.SaveChangesAsync();


                return Ok(new { Message = "Files Uploaded Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading files: {ex.Message}");
            }
        }


        [HttpPost("sendFile")]
        [DisableRequestSizeLimit]

        public async Task<IActionResult> SendFiles()
        {
            try
            {
                var files = Request.Form.Files;


                if (files == null || files.Count == 0)
                    return BadRequest(new { Message = "No files uploaded." });

                var fileUrls = new List<string>();
                var fileNames = new List<string>();
                var total_file_string = new string("");
                var total_file_names = new string("");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                        fileUrls.Add(fileUrl);
                        fileNames.Add(fileName);
                        total_file_string = string.Join(",", fileUrls);
                        total_file_names += string.Join(",", fileNames);

                        var text = new Message(
              new string[] { "abhishekbhatyelluru@gmail.com" },
              "Loan Application",
              $"Hello, please find your the loan application here.");

                        foreach (var email_fileName in fileNames)
                        {
                            var email_filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads", email_fileName);
                            text.Attachments.Add(new Attachment(email_filePath, email_fileName));
                        }

                        _emailService.SendEmail(text);

                    }
                    
                }

               

                return Ok(new { Message = "Files sent Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading files: {ex.Message}");
            }
        }
    }
}







 
