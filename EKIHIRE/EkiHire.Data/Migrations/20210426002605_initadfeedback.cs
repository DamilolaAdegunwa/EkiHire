using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class initadfeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Ad",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ad_UserId",
                table: "Ad",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ad_AspNetUsers_UserId",
                table: "Ad",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ad_AspNetUsers_UserId",
                table: "Ad");

            migrationBuilder.DropIndex(
                name: "IX_Ad_UserId",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ad");
        }
    }
}
