using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class fixItem3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdProperty_Subcategory_SubcategoryId",
                table: "AdProperty");

            migrationBuilder.AlterColumn<long>(
                name: "SubcategoryId",
                table: "AdProperty",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdProperty_Subcategory_SubcategoryId",
                table: "AdProperty",
                column: "SubcategoryId",
                principalTable: "Subcategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdProperty_Subcategory_SubcategoryId",
                table: "AdProperty");

            migrationBuilder.AlterColumn<long>(
                name: "SubcategoryId",
                table: "AdProperty",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_AdProperty_Subcategory_SubcategoryId",
                table: "AdProperty",
                column: "SubcategoryId",
                principalTable: "Subcategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
