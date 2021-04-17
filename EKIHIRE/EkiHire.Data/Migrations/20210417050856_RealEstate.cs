using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class RealEstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "RealEstate",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RealEstate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RealEstate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "RealEstate",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "RealEstate",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubcategoryId",
                table: "RealEstate",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RealEstateStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_RealEstateStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    StateId1 = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_State_StateId1",
                        column: x => x.StateId1,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealEstate_CityId",
                table: "RealEstate",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstate_StatusId",
                table: "RealEstate",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstate_SubcategoryId",
                table: "RealEstate",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_City_StateId1",
                table: "City",
                column: "StateId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstate_City_CityId",
                table: "RealEstate",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstate_RealEstateStatus_StatusId",
                table: "RealEstate",
                column: "StatusId",
                principalTable: "RealEstateStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstate_Subcategory_SubcategoryId",
                table: "RealEstate",
                column: "SubcategoryId",
                principalTable: "Subcategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstate_City_CityId",
                table: "RealEstate");

            migrationBuilder.DropForeignKey(
                name: "FK_RealEstate_RealEstateStatus_StatusId",
                table: "RealEstate");

            migrationBuilder.DropForeignKey(
                name: "FK_RealEstate_Subcategory_SubcategoryId",
                table: "RealEstate");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "RealEstateStatus");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropIndex(
                name: "IX_RealEstate_CityId",
                table: "RealEstate");

            migrationBuilder.DropIndex(
                name: "IX_RealEstate_StatusId",
                table: "RealEstate");

            migrationBuilder.DropIndex(
                name: "IX_RealEstate_SubcategoryId",
                table: "RealEstate");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "RealEstate");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RealEstate");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RealEstate");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "RealEstate");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "RealEstate");

            migrationBuilder.DropColumn(
                name: "SubcategoryId",
                table: "RealEstate");
        }
    }
}
