using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class tbl10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NewsletterSubscriber",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsletterSubscriber", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsletterSubscriber");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "AspNetUsers");
        }
    }
}
