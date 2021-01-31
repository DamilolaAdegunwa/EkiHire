using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class initSub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ErrorCodes_wrot",
                table: "ErrorCodes_wrot");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "NextOfKin",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "NextOfKinPhone",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "ErrorCodes_wrot",
                newName: "ErrorCode");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlanType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ErrorCode",
                table: "ErrorCode",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ErrorCode",
                table: "ErrorCode");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanType",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ErrorCode",
                newName: "ErrorCodes_wrot");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextOfKin",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextOfKinPhone",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "Account",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ErrorCodes_wrot",
                table: "ErrorCodes_wrot",
                column: "Id");
        }
    }
}
