using BankAuth.Context;
using BankAuth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BankAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoanDetailsController(AppDbContext context)
        {
            _context = context;
        }
        public class LoanDetailsObj
        {
            public LoanDetails LoanDetails { get; set; }
            public CustomerAccountInfo CustomerAccountInfo { get; set; }
            public Document Document { get; set; }
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDetailsObj>> GetLoanDetails(int id)
        {
            // Fetch the LoanDetails based on the provided ID
            var loanDetails = await _context.LoanDetails.FirstOrDefaultAsync(ld => ld.LoanId == id);
            if (loanDetails == null)
            {
                return NotFound();
            }
            // var loanDate = loanDetails.Created_At.Truncate(TimeSpan.FromSeconds(1));

            // Fetch the CustomerAccountInfo based on the associated AccountNum
            var customerAccountInfo = await _context.AccInfo.FirstOrDefaultAsync(acc => acc.AccountNum == loanDetails.AccountNum);
            if (customerAccountInfo == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FirstOrDefaultAsync(doc => doc.LoanId == id);
            if (document == null)
            {
                return NotFound();
            }

            var loanDetailsObj = new LoanDetailsObj
            {
                LoanDetails = loanDetails,
                CustomerAccountInfo = customerAccountInfo,
                Document = document
            };

            return Ok(loanDetailsObj);
        }

        [HttpGet("search")]
        public async Task<ActionResult<LoanDetails>> Search(int? loanId, string? AccountNum, string? loanType, string? loanStatus)
        {
            var query =  _context.LoanDetails.AsQueryable();

            if (loanId != null)
            {
                query = query.Where(l => l.LoanId == loanId);
            }

            if (!string.IsNullOrEmpty(AccountNum))
            {
                query = query.Where(l => l.AccountNum == AccountNum);
            }

            if (!string.IsNullOrEmpty(loanType))
            {
                query = query.Where(l => l.LoanType == loanType);
            }
            if (!string.IsNullOrEmpty(loanStatus))
            {
                query = query.Where(l => l.LoanStatus == loanStatus);
            }

            

            var loans = query.ToList();

            if (loans.IsNullOrEmpty())
            {
                return BadRequest(new { Message = "No such Loans exist" });

            }

            return Ok(loans);
        }


    }

}