using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class addednegContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ContactForPrice",
                table: "Ad",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Negotiable",
                table: "Ad",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactForPrice",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Negotiable",
                table: "Ad");
        }
    }
}
