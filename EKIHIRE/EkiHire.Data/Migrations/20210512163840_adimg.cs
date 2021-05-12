using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class adimg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdFeedback_Ad_AdId",
                table: "AdFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK_AdFeedback_AspNetUsers_UserId",
                table: "AdFeedback");

            migrationBuilder.AddColumn<long>(
                name: "AdId",
                table: "AdImage",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "AdFeedback",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "AdId",
                table: "AdFeedback",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_AdImage_AdId",
                table: "AdImage",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdFeedback_Ad_AdId",
                table: "AdFeedback",
                column: "AdId",
                principalTable: "Ad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdFeedback_AspNetUsers_UserId",
                table: "AdFeedback",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdImage_Ad_AdId",
                table: "AdImage",
                column: "AdId",
                principalTable: "Ad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdFeedback_Ad_AdId",
                table: "AdFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK_AdFeedback_AspNetUsers_UserId",
                table: "AdFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK_AdImage_Ad_AdId",
                table: "AdImage");

            migrationBuilder.DropIndex(
                name: "IX_AdImage_AdId",
                table: "AdImage");

            migrationBuilder.DropColumn(
                name: "AdId",
                table: "AdImage");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "AdFeedback",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AdId",
                table: "AdFeedback",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdFeedback_Ad_AdId",
                table: "AdFeedback",
                column: "AdId",
                principalTable: "Ad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdFeedback_AspNetUsers_UserId",
                table: "AdFeedback",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
