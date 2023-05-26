
using BankAuth.Context;
using BankAuth.Helpers;
using BankAuth.Models;
using BankAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace BankAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IEmailService _emailService;
        public string generated_tokened;
        

        public UserController(AppDbContext appDbContext,  IEmailService emailService
            )
        {
            _authContext = appDbContext;
            _emailService = emailService;
           

        }

        [HttpPost("authenticate")]

        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Customerid == userObj.Customerid);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });

            }

            string generated_token = GeneratedTokenForTwoF();


            
            user.TwoVal = PasswordHasher.HashPassword(generated_token);
            await _authContext.SaveChangesAsync();


            var text = new Message(
                new string[] { "shetkarthik89@gmail.com" },
                "OTP for Authentication",
              $"Hello {user.UserName}, {Environment.NewLine}  Your OTP for verification is  {generated_token.ToString()}");




            _emailService.SendEmail(text);


            return Ok(new
            {
                //Token = user.Token,
                Message = "Check your registered Email for the OTP"
            });



        }


        [HttpPost("authenticate/2F")]
        public async Task<IActionResult> ProAuthenticate([FromBody] User userObj,string tokenentry)
        {
            

            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Customerid == userObj.Customerid);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }


            if(PasswordHasher.VerifyPassword(tokenentry,user.TwoVal))

            //if (tokenentry == user.TwoVal)
            {
                user.Token = CreateJwtToken(user);

                return Ok(new
                {
                    Token = user.Token,
                    Message = $"Login Sucess"
                });
            }

            return BadRequest(new
            {
                
                Message = $"Please Check the OTP and try again"
            });


        }

        [HttpPost("getByCustomerId")]

        public async Task<IActionResult> getUserByCustomerId(string customerid)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Customerid == customerid);

            if (user == null)
            {
                return BadRequest(new
                {

                    Message = "User doesnt exist"
                });
            }
            return Ok(user);
        }



        [HttpPost("register")]


        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }

            if (await CheckPhoneExistAsync(userObj.ContactNum))
                return BadRequest(new { Message = "ContactNum already exists" });

            if (await CheckAccountNumberExistAsync(userObj.AccountNum))
                return BadRequest(new { Message = "AccountNum already exists" });

            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email already exists" });

            if (await CheckCustomerIdExistAsync(userObj.Customerid))
                return BadRequest(new { Message = "CustomerId already exists" });

            var pass = CheckPasswordStrength(userObj.Password);

            if (!string.IsNullOrEmpty(pass))
            {
                return BadRequest(new { Message = pass.ToString() });
            }
            var customerId = CheckCustomerIdStrength(userObj.Customerid);

            if (!string.IsNullOrEmpty(customerId))
            {
                return BadRequest(new { Message = customerId.ToString() });
            }
            var accountnumber = CheckAccountNumberStrength(userObj.AccountNum);
            if (!string.IsNullOrEmpty(accountnumber))
            {
                return BadRequest(new { Message = accountnumber.ToString() });
            }
            var contactnumber = CheckContactStrength(userObj.ContactNum);
            if (!string.IsNullOrEmpty(contactnumber))
            {
                return BadRequest(new { Message = contactnumber.ToString() });
            }


            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "user";
            userObj.Token = "";

            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Register Success" });
        }

        private async Task<bool> CheckPhoneExistAsync(string phone) => await _authContext.Users.AnyAsync(x => x.ContactNum == phone);


        private async Task<bool> CheckCustomerIdExistAsync(string customer_id)

         => await _authContext.Users.AnyAsync(x => x.Customerid == customer_id);


        private async Task<bool> CheckEmailExistAsync(string email)

         => await _authContext.Users.AnyAsync(x => x.Email == email);

        private async Task<bool> CheckAccountNumberExistAsync(string accountnum)

          => await _authContext.Users.AnyAsync(x => x.AccountNum == accountnum);

        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();

            if (password.Length < 8)
            {
                sb.Append("Password should have minimum of 8 characters" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                sb.Append("Password should be Alphanumeric" + Environment.NewLine);
            }
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,-,=]"))
            {
                sb.Append("Password should contain special characters" + Environment.NewLine);
            }

            return sb.ToString();

        }

        private string CheckAccountNumberStrength(string accountnum)
        {
            StringBuilder sb = new StringBuilder();

            if (accountnum.Length < 12 || accountnum.Length > 12)
            {
                sb.Append("AccountNumber must have 12 digits" + Environment.NewLine);
            }

            return sb.ToString();

        }

        private string CheckContactStrength(string contactNum)
        {
            StringBuilder sb = new StringBuilder();

            if (contactNum.Length < 10 || contactNum.Length > 10)
            {
                sb.Append("Phonenumber must have 10 digits" + Environment.NewLine);
            }

            return sb.ToString();

        }

        private string CheckCustomerIdStrength(string customerid)
        {
            StringBuilder sb = new StringBuilder();

            if (customerid.Length < 8)
            {
                sb.Append("CustomerId should have minimum of 8 characters" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(customerid, "[a-z]") && Regex.IsMatch(customerid, "[A-Z]") && Regex.IsMatch(customerid, "[0-9]")))
            {
                sb.Append("CustomerId should be Alphanumeric" + Environment.NewLine);
            }
            if (!Regex.IsMatch(customerid, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,-,=]"))
            {
                sb.Append("CustomerId should contain special characters" + Environment.NewLine);
            }

            return sb.ToString();

        }

        private string  GeneratedTokenForTwoF()
        {
            Random rnd = new Random();

            var token_generated = rnd.Next(100000, 999999);

          
            return token_generated.ToString();
        }

        private string CreateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryveryverysecret...");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Customerid)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);

        }
        [Authorize]
        [HttpGet]

        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _authContext.Users.ToListAsync());
        }

        [HttpGet("/testemail")]

        public async Task<IActionResult> TestEmail()
        {
            var text = new Message(new string[] { "shetkarthik89@gmail.com" }, "Test", "<h1>Done</h1>");
            _emailService.SendEmail(text);
            return Ok(new { Message = "Email produced" });
        }
    }
}
