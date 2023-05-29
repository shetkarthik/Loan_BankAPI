﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAuth.Migrations
{
    /// <inheritdoc />
    public partial class v124 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer_accountinfoo",
                columns: table => new
                {
                    CustomerAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Addresss = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkExp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccupationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccupationAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_accountinfoo", x => x.CustomerAccountId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_accountinfoo");
        }
    }
}
