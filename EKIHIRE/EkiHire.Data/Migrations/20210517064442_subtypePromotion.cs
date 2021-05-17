using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class subtypePromotion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubscriptionPlanType",
                table: "AspNetUsers",
                newName: "SubscriptionPlan");

            migrationBuilder.AddColumn<bool>(
                name: "Promotion",
                table: "Ad",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Promotion",
                table: "Ad");

            migrationBuilder.RenameColumn(
                name: "SubscriptionPlan",
                table: "AspNetUsers",
                newName: "SubscriptionPlanType");
        }
    }
}
