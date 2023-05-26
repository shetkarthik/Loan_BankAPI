using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAuth.Models
{
    public class User
    {
        public User()
        {
            AadharNum = "23452345234590";
            PanNum = "KWBPS2301H";
            DOB = "1/07/1994";
            Gender = "Male";
            Addresss = "1676 Simons Hollow Road,Hazleton,Pennsylvania,PIN:18201";
        }
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; } 
        
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? ContactNum { get; set; }

        public string? AccountNum { get; set; }

        public string? Token { get; set; }

        public string? Role { get; set; }

        public string? TwoVal { get; set; }

        public string? Customerid { get; set; }

        [DefaultValue("23452345234590")]
        public string ?AadharNum { get; set; }

        [DefaultValue("KWBPS2301H")]
        public string ?PanNum { get; set; }

        [DefaultValue("1/07/1994")]
        public string ?DOB { get; set; }

        [DefaultValue("Male")]
        public string ?Gender { get; set; }
        
        
        [DefaultValue("1676 Simons Hollow Road,Hazleton,Pennsylvania,PIN:18201")]
        public string ?Addresss { get; set; }

    }
}
