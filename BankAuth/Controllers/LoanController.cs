using BankAuth.Context;
using BankAuth.Models;
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


        public LoanController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;
            

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
            var selected_interest = await GetInterestByLoanType(LoanObj.LoanType);

            float loanAmount = float.Parse(LoanObj.LoanAmount);
            var tenure = LoanObj.Tenure;


            DateTime currentDate = DateTime.Now;
            DateTime futureDate = DateTime.Now;
            string loanStartDate = currentDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-GB"));
            futureDate = futureDate.AddYears((int)tenure);
            string loanEndDate = futureDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-GB"));



            var total_loan_Amount = (Math.Round((double)(loanAmount + (loanAmount * (selected_interest / 100) * tenure)))).ToString();
            var loan_Emi = (Math.Round((double)(loanAmount * (selected_interest / 1200) * Math.Pow((double)(1 + (selected_interest / 1200)), (double)(tenure * 12))) / (Math.Pow((double)(1 + (selected_interest / 1200)), (double)(tenure * 12)) - 1))).ToString();


            var updatedLoanObj = new LoanDetails
            {
                AccountNum = "123412341234",
                LoanType = LoanObj.LoanType,
                LoanAmount = LoanObj.LoanAmount,
                Interest = selected_interest,
                Tenure = LoanObj.Tenure,
                LoanEmi = loan_Emi,
                LoanTotalAmount = total_loan_Amount,
                MonthlyIncome = LoanObj.MonthlyIncome,
                AnnualIncome = LoanObj.AnnualIncome,
                OtherEmi = LoanObj.OtherEmi,
                LoanStartDate = loanStartDate,
                LoanEndDate = loanEndDate
            };

             _authContext.LoanDetails.Add(updatedLoanObj);
             await _authContext.SaveChangesAsync();

            return Ok(new { Message = "LoanApplied Successfully" });
        }
    }

    


}
