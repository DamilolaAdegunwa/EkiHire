using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class notmapped_init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AdPropertyValue_AdId",
                table: "AdPropertyValue",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdPropertyValue_Ad_AdId",
                table: "AdPropertyValue",
                column: "AdId",
                principalTable: "Ad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdPropertyValue_Ad_AdId",
                table: "AdPropertyValue");

            migrationBuilder.DropIndex(
                name: "IX_AdPropertyValue_AdId",
                table: "AdPropertyValue");
        }
    }
}
