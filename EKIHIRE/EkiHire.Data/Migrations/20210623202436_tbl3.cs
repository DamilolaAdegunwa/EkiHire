using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class tbl3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "JobApplications",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmail",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhoneNumber",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyEmail",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "ContactPhoneNumber",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "JobApplications",
                newName: "PhoneNumber");
        }
    }
}
