using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class tbl9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreviousWorkExperiences_JobApplications_JobApplicationId",
                table: "PreviousWorkExperiences");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestQuotes_AspNetUsers_RequesterId",
                table: "RequestQuotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Search_Ad_AdId",
                table: "Search");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Search",
                table: "Search");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestQuotes",
                table: "RequestQuotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreviousWorkExperiences",
                table: "PreviousWorkExperiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications");

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "Skill");

            migrationBuilder.RenameTable(
                name: "Search",
                newName: "AdLookupLog");

            migrationBuilder.RenameTable(
                name: "RequestQuotes",
                newName: "RequestQuote");

            migrationBuilder.RenameTable(
                name: "PreviousWorkExperiences",
                newName: "PreviousWorkExperience");

            migrationBuilder.RenameTable(
                name: "JobApplications",
                newName: "JobApplication");

            migrationBuilder.RenameIndex(
                name: "IX_Search_AdId",
                table: "AdLookupLog",
                newName: "IX_AdLookupLog_AdId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestQuotes_RequesterId",
                table: "RequestQuote",
                newName: "IX_RequestQuote_RequesterId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousWorkExperiences_JobApplicationId",
                table: "PreviousWorkExperience",
                newName: "IX_PreviousWorkExperience_JobApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdLookupLog",
                table: "AdLookupLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestQuote",
                table: "RequestQuote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreviousWorkExperience",
                table: "PreviousWorkExperience",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplication",
                table: "JobApplication",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SubscriptionPackage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_SubscriptionPackage", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AdLookupLog_Ad_AdId",
                table: "AdLookupLog",
                column: "AdId",
                principalTable: "Ad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousWorkExperience_JobApplication_JobApplicationId",
                table: "PreviousWorkExperience",
                column: "JobApplicationId",
                principalTable: "JobApplication",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestQuote_AspNetUsers_RequesterId",
                table: "RequestQuote",
                column: "RequesterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdLookupLog_Ad_AdId",
                table: "AdLookupLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousWorkExperience_JobApplication_JobApplicationId",
                table: "PreviousWorkExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestQuote_AspNetUsers_RequesterId",
                table: "RequestQuote");

            migrationBuilder.DropTable(
                name: "SubscriptionPackage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestQuote",
                table: "RequestQuote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreviousWorkExperience",
                table: "PreviousWorkExperience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplication",
                table: "JobApplication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdLookupLog",
                table: "AdLookupLog");

            migrationBuilder.RenameTable(
                name: "Skill",
                newName: "Skills");

            migrationBuilder.RenameTable(
                name: "RequestQuote",
                newName: "RequestQuotes");

            migrationBuilder.RenameTable(
                name: "PreviousWorkExperience",
                newName: "PreviousWorkExperiences");

            migrationBuilder.RenameTable(
                name: "JobApplication",
                newName: "JobApplications");

            migrationBuilder.RenameTable(
                name: "AdLookupLog",
                newName: "Search");

            migrationBuilder.RenameIndex(
                name: "IX_RequestQuote_RequesterId",
                table: "RequestQuotes",
                newName: "IX_RequestQuotes_RequesterId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousWorkExperience_JobApplicationId",
                table: "PreviousWorkExperiences",
                newName: "IX_PreviousWorkExperiences_JobApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_AdLookupLog_AdId",
                table: "Search",
                newName: "IX_Search_AdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestQuotes",
                table: "RequestQuotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreviousWorkExperiences",
                table: "PreviousWorkExperiences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Search",
                table: "Search",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousWorkExperiences_JobApplications_JobApplicationId",
                table: "PreviousWorkExperiences",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestQuotes_AspNetUsers_RequesterId",
                table: "RequestQuotes",
                column: "RequesterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Search_Ad_AdId",
                table: "Search",
                column: "AdId",
                principalTable: "Ad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
