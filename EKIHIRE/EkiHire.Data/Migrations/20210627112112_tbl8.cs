using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class tbl8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resume",
                table: "JobApplications");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "Transaction",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TransactionStatus",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Transaction",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumePath",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionStatus",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ResumePath",
                table: "JobApplications");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Resume",
                table: "JobApplications",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
