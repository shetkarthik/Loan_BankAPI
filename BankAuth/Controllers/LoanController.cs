using BankAuth.Context;
using BankAuth.Models;
using BankAuth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace BankAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IEmailService _emailService;

        public class Payload
        {
            public int? Id { get; set; }
            public string? status { get; set; }
            public string? comment { get; set; }
        }


        public LoanController(AppDbContext appDbContext, IEmailService emailService)
        {
            _authContext = appDbContext;
            _emailService = emailService;

        }

        [HttpGet("getAllLoans")]

        public async Task<IEnumerable<LoanDetails>> GetLoanDetails()
        {
            return await _authContext.LoanDetails
        .Where(ld => ld.LoanStatus == null)
        .ToListAsync();
        }


        [HttpGet("getInterest")]
        public async Task<float?> GetInterestByLoanType(string loanType)
        {
            var type = await _authContext.LoanInterest.FirstOrDefaultAsync(x => x.LoanType == loanType);

            return type.LoanInterest;

        }
        [HttpPost("applyLoan")]

        public async Task<IActionResult> ApplyLoan([FromBody] LoanDetails LoanObj)
        {
            LoanObj.Interest = await GetInterestByLoanType(LoanObj.LoanType);

            float loanAmount = float.Parse(LoanObj.LoanAmount);
            var tenure = LoanObj.Tenure /12;


            DateTime currentDate = DateTime.Now;
            DateTime futureDate = DateTime.Now;
            string loanStartDate = currentDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-GB"));
            futureDate = futureDate.AddYears((int)tenure);
            string loanEndDate = futureDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-GB"));



            var total_loan_Amount = (Math.Round((double)(loanAmount + (loanAmount * (LoanObj.Interest / 100) * tenure)))).ToString();
            var loan_Emi = (Math.Round((double)(loanAmount * (LoanObj.Interest / 1200) * Math.Pow((double)(1 + (LoanObj.Interest / 1200)), (double)(tenure * 12))) / (Math.Pow((double)(1 + (LoanObj.Interest / 1200)), (double)(tenure * 12)) - 1))).ToString();

            

            var updatedLoanObj = new LoanDetails
            {
                LoanId = LoanObj.LoanId,
                AccountNum = LoanObj.AccountNum,
                LoanType = LoanObj.LoanType,
                LoanAmount = LoanObj.LoanAmount,
                Interest = LoanObj.Interest,
                Tenure = LoanObj.Tenure,
                LoanEmi = loan_Emi,
                LoanTotalAmount = total_loan_Amount,
                MonthlyIncome = LoanObj.MonthlyIncome,
                AnnualIncome = LoanObj.AnnualIncome,
                OtherEmi = LoanObj.OtherEmi,
                LoanStartDate = loanStartDate,
                LoanEndDate = loanEndDate,
                LoanPurpose = LoanObj.LoanPurpose,
                PropertyArea = LoanObj.PropertyArea,
                PropertyLoc = LoanObj.PropertyLoc,
                PropertyValue = LoanObj.PropertyValue,
                OngoingLoan = LoanObj.OngoingLoan,
                VehiclePrice = LoanObj.VehiclePrice,
                VehicleRCNumber = LoanObj.VehicleRCNumber,
                VehicleType = LoanObj.VehicleType,
                VendorAddress = LoanObj.VendorAddress,
                VendorName = LoanObj.VendorName,
                CourseDuration = LoanObj.CourseDuration,
                CourseName = LoanObj.CourseName,
                TotalFee = LoanObj.TotalFee,
                EducationType = LoanObj.EducationType,
                InstituteName = LoanObj.InstituteName,
            };

            _authContext.LoanDetails.Add(updatedLoanObj);

            await _authContext.SaveChangesAsync();


           // return Ok(new { Message = "LoanApplied Successfully" });

            return Ok(new {updatedLoanObj.LoanId});
            

        }
        [HttpGet("getLoanByAccountNum")]
        public async Task<IActionResult> getLoanByAccountNumber(string accountnum)
        {
            var loanDetails = await _authContext.LoanDetails.Where(x => x.AccountNum == accountnum).OrderByDescending(x => x.LoanId).FirstOrDefaultAsync();


            if (loanDetails == null)
            {
                return BadRequest(new
                {

                    Message = "No Loans taken by the user"
                });
            }
            return Ok(loanDetails);
        }
        
        
        [HttpGet("getLoanStatusByAccountNum")]
        public async Task<IActionResult> getLoanStatusByAccountNumber(string accountnum)
        {
            var loanDetails = await _authContext.LoanDetails.Where(x => x.AccountNum == accountnum).ToListAsync();


            if (loanDetails == null)
            {
                return BadRequest(new
                {

                    Message = "No Loans taken by the user"
                });
            }
            return Ok(loanDetails);
        }
        [HttpPut("status")]
        public async Task<IActionResult> UpdateLoanStatus([FromBody] Payload payload)
        {
            var loan = await _authContext.LoanDetails.FindAsync(payload.Id);
            if (loan == null)
            {
                return NotFound();
            }

            loan.LoanStatus = payload.status;
            loan.Comment = payload.comment;
          

            _authContext.Entry(loan).State = EntityState.Modified;

            try
            {
                var text = new Message(
       new string[] { "shetkarthik89@gmail.com" }, "Updated Loan Application Status", $"Hello Sir/Madam, {Environment.NewLine} Your Loan has been reviewed and verified by our managers and results are as follows.{Environment.NewLine}" + $"Loan Id : {payload.Id} {Environment.NewLine}" + $"Loan Status : {payload.status} {Environment.NewLine}" + $"Reviewed Comments: {payload.comment} {Environment.NewLine}" + $"Please visit our branch for further details and queries");
                await _authContext.SaveChangesAsync();
                _emailService.SendEmail(text);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

    }






}
