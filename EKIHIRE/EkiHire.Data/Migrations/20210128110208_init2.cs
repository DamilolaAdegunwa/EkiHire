using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AspNetUsers_UserId1",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_UserId1",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "DateOfEmployment",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "EmployeePhoto",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Account");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Account",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "Account",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Account",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "ErrorCodes_wrot",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorCodes_wrot", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AspNetUsers_UserId",
                table: "Account",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AspNetUsers_UserId",
                table: "Account");

            migrationBuilder.DropTable(
                name: "ErrorCodes_wrot");

            migrationBuilder.DropIndex(
                name: "IX_Account_UserId",
                table: "Account");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Account",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Account",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Account",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfEmployment",
                table: "Account",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeePhoto",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "Account",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId1",
                table: "Account",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AspNetUsers_UserId1",
                table: "Account",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
