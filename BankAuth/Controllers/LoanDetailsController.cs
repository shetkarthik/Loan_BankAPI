using BankAuth.Context;
using BankAuth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            // Fetch the Document based on the associated AccountNum
            var document = await _context.Documents.FirstOrDefaultAsync(doc => doc.LoanId == id);
            if (document == null)
            {
                return NotFound();
            }

            // Create a ViewModel to hold the merged details
            var loanDetailsObj = new LoanDetailsObj
            {
                LoanDetails = loanDetails,
                CustomerAccountInfo = customerAccountInfo,
                Document = document
            };

            return Ok(loanDetailsObj);
        }

       
    }

}
