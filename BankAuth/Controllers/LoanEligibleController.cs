using BankAuth.Context;
using BankAuth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BankAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanEligibleController : ControllerBase
    {
        private readonly AppDbContext _authContext;

        public class LoanEligible
        {
            public string? LoanType { get; set; }
            public string? LoanAmount { get; set; }
            public int? Tenure { get; set; }
            public string? MonthlyIncome { get; set; }
            public string? AnnualIncome { get; set; }
            public string? OtherEmi { get; set; }

        }
        public class Result
        {
            public string AvailableEMI { get; set; }
            public string LoanEMI { get; set; }
            public string Results { get; set; }
            public string Color { get; set; }
        }
        public LoanEligibleController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;

        }

        [HttpGet("getInterest")]
        public async Task<float?> GetInterestByLoanType(string loanType)
        {
            var type = await _authContext.LoanInterest.FirstOrDefaultAsync(x => x.LoanType == loanType);

            return type.LoanInterest;

        }
        [HttpPost("checkEligible")]

           public async Task<IActionResult> CheckEligible([FromBody] LoanEligible LoanObj)
            {
                var Interest = await GetInterestByLoanType(LoanObj.LoanType);

                float loanAmount = float.Parse(LoanObj.LoanAmount);
                float monthlyIncome = float.Parse(LoanObj.AnnualIncome) /12;
                float annualIncome = float.Parse(LoanObj.AnnualIncome);
                float otheremi = float.Parse(LoanObj.OtherEmi);

              if(loanAmount <  10000 || loanAmount >= 100000000) 
              {
                return BadRequest(new { Message = "Loan Amount must be within 10,000 to 10 crores" });
              }

              if(annualIncome >= 100000000)
              {
                return BadRequest(new { Message = "AnnualIncome maxed out" });
            }


                var tenure = LoanObj.Tenure;

                var monthemi = Math.Round(monthlyIncome / 2);
               var annualemi = Math.Round(annualIncome / 24);

                var avgemi = Math.Round((monthemi + annualemi) / 2);

                var availableamount = avgemi - otheremi;

                var loan_Emi = Math.Round((double)(loanAmount * (Interest / 1200) * Math.Pow((double)(1 + (Interest / 1200)), (double)(tenure))) / (Math.Pow((double)(1 + (Interest / 1200)), (double)(tenure)) - 1));

            if (availableamount < loan_Emi)
            {

                var result = new Result
                {
                    AvailableEMI = $"Your Available EMI is \u20B9{availableamount}",
                    LoanEMI = $"Your Loan EMI for the following input is \u20B9{loan_Emi}",
                    Results = "Sorry your Loan cannot be approved, Please Contact our customer services for more info",
                    Color = "danger"
                };
                return Ok(result);
            }
            else
            {
                var result = new Result
                {
                    AvailableEMI = $"Your Available EMI is \u20B9{availableamount}",
                    LoanEMI = $"Your Loan EMI for the following input is \u20B9{loan_Emi}",
                    Results = "Congratulations!! Your Loan is Eligible and waiting to be approved",
                    Color = "success"
                };
                return Ok(result);
            }

        }
    }
}
