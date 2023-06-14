using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAuth.Migrations
{
    /// <inheritdoc />
    public partial class v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseDuration",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationType",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstituteName",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoanComment",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalFee",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehiclePrice",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleRCNumber",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorAddress",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorName",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseDuration",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "EducationType",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "InstituteName",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "LoanComment",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "TotalFee",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "VehiclePrice",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "VehicleRCNumber",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "VendorAddress",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "VendorName",
                table: "loan_details");
        }
    }
}
