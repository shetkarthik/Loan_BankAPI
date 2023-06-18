﻿// <auto-generated />
using System;
using BankAuth.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankAuth.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230616091121_v45456")]
    partial class v45456
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BankAuth.Models.CustomerAccountInfo", b =>
                {
                    b.Property<int>("CustomerAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerAccountId"));

                    b.Property<string>("AadharNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Addresss")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DOB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OccupationAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OccupationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PanNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkExp")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerAccountId");

                    b.ToTable("customer_accountinfo", (string)null);
                });

            modelBuilder.Entity("BankAuth.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoanType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("loan_documents", (string)null);
                });

            modelBuilder.Entity("BankAuth.Models.Interest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float?>("LoanInterest")
                        .HasColumnType("real");

                    b.Property<string>("LoanType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("loan_interest", (string)null);
                });

            modelBuilder.Entity("BankAuth.Models.LoanDetails", b =>
                {
                    b.Property<int>("LoanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoanId"));

                    b.Property<string>("AccountNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AnnualIncome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseDuration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<string>("EducationType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstituteName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Interest")
                        .HasColumnType("real");

                    b.Property<string>("LoanAmount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoanComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoanEmi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoanEndDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoanPurpose")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoanStartDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoanStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoanTotalAmount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoanType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Modified_At")
                        .HasColumnType("datetime2");

                    b.Property<string>("MonthlyIncome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OngoingLoan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OtherEmi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyArea")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyLoc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Tenure")
                        .HasColumnType("int");

                    b.Property<string>("TotalFee")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehiclePrice")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleRCNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LoanId");

                    b.ToTable("loan_details", (string)null);
                });

            modelBuilder.Entity("BankAuth.Models.UserReg", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("AccountNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("user_cred", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
