using BankAuth.Context;
using BankAuth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanAccDetailsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;


        public LoanAccDetailsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET api/loan/{accountNumber}
        [HttpGet("{accountNumber}")]
        public async Task<IActionResult>  GetLoanDetails(string accountNumber)
        {
            // Retrieve loan details, customer account info, and documents based on the account number
            var loanDetails = _dbContext.LoanDetails.FirstOrDefault(ld => ld.AccountNum == accountNumber);
            var accountInfo = _dbContext.AccInfo.FirstOrDefault(ai => ai.AccountNum == accountNumber);
            var documents = await _dbContext.Documents.Where(d => d.AccountNum == accountNumber).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
           
            // If any of the data is not found, return a 404 Not Found response
            if (loanDetails == null || accountInfo == null || documents == null)
            {
                return NotFound();
            }

            // Merge the data into a single object
            var loanInfo = new LoanInfo
            {
                AccountNumber = accountNumber,
                LoanDetails = loanDetails,
                CustomerAccountInfo = accountInfo,
                Document = documents
            };

            return Ok(loanInfo);
        }
    }

    // Define the LoanInfo class to hold the merged data
    public class LoanInfo
    {
        public string AccountNumber { get; set; }
        public LoanDetails LoanDetails { get; set; }
        public CustomerAccountInfo CustomerAccountInfo { get; set; }
        public Document Document { get; set; }
    }
}

