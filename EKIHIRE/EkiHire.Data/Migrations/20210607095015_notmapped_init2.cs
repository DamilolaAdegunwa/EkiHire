using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class notmapped_init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ad_AspNetUsers_UserId",
                table: "Ad");

            migrationBuilder.DropForeignKey(
                name: "FK_Ad_Subcategory_SubcategoryId",
                table: "Ad");

            migrationBuilder.DropIndex(
                name: "IX_Ad_SubcategoryId",
                table: "Ad");

            migrationBuilder.DropIndex(
                name: "IX_Ad_UserId",
                table: "Ad");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ad_SubcategoryId",
                table: "Ad",
                column: "SubcategoryId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Ad_Subcategory_SubcategoryId",
                table: "Ad",
                column: "SubcategoryId",
                principalTable: "Subcategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
