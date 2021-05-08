using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class hmmAd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdImage_Ad_AdId",
                table: "AdImage");

            migrationBuilder.DropForeignKey(
                name: "FK_AdProperty_Subcategory_SubcategoryId",
                table: "AdProperty");

            migrationBuilder.DropTable(
                name: "WorkExperience");

            migrationBuilder.DropIndex(
                name: "IX_AdImage_AdId",
                table: "AdImage");

            migrationBuilder.DropColumn(
                name: "AdId",
                table: "AdImage");

            migrationBuilder.AlterColumn<long>(
                name: "SubcategoryId",
                table: "AdProperty",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_AdFeedback_AdId",
                table: "AdFeedback",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_AdFeedback_UserId",
                table: "AdFeedback",
                column: "UserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AdProperty_Subcategory_SubcategoryId",
                table: "AdProperty",
                column: "SubcategoryId",
                principalTable: "Subcategory",
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
                name: "FK_AdProperty_Subcategory_SubcategoryId",
                table: "AdProperty");

            migrationBuilder.DropIndex(
                name: "IX_AdFeedback_AdId",
                table: "AdFeedback");

            migrationBuilder.DropIndex(
                name: "IX_AdFeedback_UserId",
                table: "AdFeedback");

            migrationBuilder.AlterColumn<long>(
                name: "SubcategoryId",
                table: "AdProperty",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AdId",
                table: "AdImage",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkExperience",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TillNow = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperience_Ad_AdId",
                        column: x => x.AdId,
                        principalTable: "Ad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdImage_AdId",
                table: "AdImage",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_AdId",
                table: "WorkExperience",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdImage_Ad_AdId",
                table: "AdImage",
                column: "AdId",
                principalTable: "Ad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdProperty_Subcategory_SubcategoryId",
                table: "AdProperty",
                column: "SubcategoryId",
                principalTable: "Subcategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
