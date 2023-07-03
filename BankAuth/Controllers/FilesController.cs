using BankAuth.Context;
using BankAuth.Models;
using BankAuth.Services;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net.Mail;
using System.Reflection.Metadata;
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

                var loanId = Convert.ToInt32(Request.Form["loanId"]);

                var documentexists = await _authContext.Documents.FirstOrDefaultAsync(ld => ld.LoanId == loanId);

                var loandetailexists = await _authContext.LoanDetails.FirstOrDefaultAsync(ld => ld.LoanId == loanId);

                
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
                            //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName)
                            var fileName = file.FileName;
                            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                            fileUrls.Add(fileUrl);
                            fileNames.Add(fileName);
                        }
                    }
                    total_file_string = string.Join(",", fileUrls);
                    total_file_names = string.Join(",", fileNames);
                if (documentexists == null)
                {
                    var newDocument = new Models.Document
                    {
                        AccountNum = accountNum,
                        FileName = total_file_names,
                        FilePath = total_file_string,
                        LoanType = loanType,
                        LoanId = Convert.ToInt32(loanId),
                    };

                    _authContext.Documents.Add(newDocument);
                    await _authContext.SaveChangesAsync();


                    return Ok(newDocument);
                }
                else
                {
                    var appendedFileString = string.Join(",", documentexists.FilePath, total_file_string);
                    var appendedFileNames = string.Join(",", documentexists.FileName, total_file_names);

                    documentexists.FilePath = appendedFileString;
                    documentexists.FileName = appendedFileNames;

                    loandetailexists.LoanStatus = "Processing";
                    loandetailexists.Comment = "";

                    _authContext.Entry(documentexists).State = EntityState.Modified;
                    _authContext.Entry(loandetailexists).State = EntityState.Modified;
                    await _authContext.SaveChangesAsync();

                    return Ok(new { Message = "Files appended successfully" });

                } //return Ok(new { Message = "Files Uploaded Successfully" });
                }
            
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading files: {ex.Message}");
            }
        }

      

        [HttpPut("deleteFile")]

        public async Task<IActionResult> DeleteFile(string fileName,int loanId)
        {
            var document = await _authContext.Documents.FirstOrDefaultAsync(ld=> ld.LoanId == loanId);
            var loandetailexists = await _authContext.LoanDetails.FirstOrDefaultAsync(ld => ld.LoanId == loanId);

            //var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads", fileName);

            var filePath = $"https://localhost:7080/uploads/{fileName}";

        

            var fileUrls = document.FilePath;
            var fileNames = document.FileName;

            string[] values = fileNames.Split(',');
            var filteredValues = values.Where(val => val.Trim() != fileName);
            string updatedString = string.Join(',', filteredValues);


            string[] valued = fileUrls.Split(',');
            var filteredValued = valued.Where(val => val.Trim() != filePath);
            string updatedStringed = string.Join(',', filteredValued);



            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }


            document.FileName = updatedString;
            document.FilePath = updatedStringed;

            loandetailexists.LoanStatus = "Processing";
            loandetailexists.Comment = "";

            _authContext.Entry(document).State = EntityState.Modified;
            _authContext.Entry(loandetailexists).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "File Updated successfully" });

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
              new string[] { "shetkarthik89@gmail.com" },
              "Loan Application",
              "Hello, please find your the loan application here"
          );

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







 
