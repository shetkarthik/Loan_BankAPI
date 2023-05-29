using BankAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankAuth.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UserReg> UserRegs { get; set; }
        public DbSet<CustomerAccountInfo> AccInfo { get; set; }
        

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<UserReg>()

                .ToTable("user_cred");

            modelBuilder.Entity<CustomerAccountInfo>()

                .ToTable("customer_accountinfo");
            



        }


    }
}






