using Microsoft.EntityFrameworkCore.Migrations;

namespace EkiHire.Data.Migrations
{
    public partial class usercart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdsStatus",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Bathroom",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Bedroom",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "BrokerFee",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "CarType",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Certification",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "EmploymentStatus",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "ExchangePossible",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "ExpectedSalary",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Furniture",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "HighestLevelOfEducation",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "LandType",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Maker",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Parking",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Quality",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "ResumePath",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Seats",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "ServiceArea",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "ServiceFeature",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "SquareMeters",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Ad");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Ad",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "TypeOfService",
                table: "Ad",
                newName: "AdReference");

            migrationBuilder.RenameColumn(
                name: "SaveData",
                table: "Ad",
                newName: "InUserCart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InUserCart",
                table: "Ad",
                newName: "SaveData");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Ad",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "AdReference",
                table: "Ad",
                newName: "TypeOfService");

            migrationBuilder.AddColumn<int>(
                name: "AdsStatus",
                table: "Ad",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Age",
                table: "Ad",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Bathroom",
                table: "Ad",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Bedroom",
                table: "Ad",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrokerFee",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarType",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certification",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentStatus",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExchangePossible",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedSalary",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Furniture",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighestLevelOfEducation",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandType",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Maker",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mileage",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Parking",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Quality",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Requirements",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumePath",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Room",
                table: "Ad",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Seats",
                table: "Ad",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceArea",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceFeature",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SquareMeters",
                table: "Ad",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Ad",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
