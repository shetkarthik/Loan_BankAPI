using BankAuth.Context;
using BankAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AppDbContext _authContext;

        public FilesController(IWebHostEnvironment hostingEnvironment, AppDbContext authcontext)
        {
            _hostingEnvironment = hostingEnvironment;
            _authContext = authcontext;
        }



        [HttpPost]
        [DisableRequestSizeLimit]

        public async Task<IActionResult> UploadFiles()
        {
            try
            {
                var files = Request.Form.Files;

                var accountNum = Request.Form["accountNumber"];

                if (files == null || files.Count == 0)
                    return BadRequest("No files uploaded.");

                var fileUrls = new List<string>();

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

                        var newDocument = new Document
                        {
                            AccountNum = accountNum,
                            FileName = fileName,
                            FilePath = fileUrl,
                        };

                        fileUrls.Add(fileUrl);
                        _authContext.Documents.Add(newDocument);
                        await _authContext.SaveChangesAsync();



                    }
                }


                return Ok(fileUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading files: {ex.Message}");
            }
        }
    }
}







 
