using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class tbl6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdImage_Ad_AdId",
                table: "AdImage");

            migrationBuilder.DropIndex(
                name: "IX_AdImage_AdId",
                table: "AdImage");

            migrationBuilder.DropColumn(
                name: "ImageString",
                table: "AdImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageString",
                table: "AdImage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdImage_AdId",
                table: "AdImage",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdImage_Ad_AdId",
                table: "AdImage",
                column: "AdId",
                principalTable: "Ad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
