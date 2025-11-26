using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerCategory_CustomerCategoryId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_CustomerCategoryId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "FaxNumber",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Customer",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "WhatsApp",
                table: "Customer",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Customer",
                newName: "Floor");

            migrationBuilder.RenameColumn(
                name: "TwitterX",
                table: "Customer",
                newName: "FlatNumber");

            migrationBuilder.RenameColumn(
                name: "TikTok",
                table: "Customer",
                newName: "BuildingNumber");

            migrationBuilder.RenameColumn(
                name: "CustomerCategoryId",
                table: "Customer",
                newName: "TRN");

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "Customer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Customer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GovernorateId",
                table: "Customer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "TRN",
                table: "Customer",
                newName: "CustomerCategoryId");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Customer",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Customer",
                newName: "WhatsApp");

            migrationBuilder.RenameColumn(
                name: "Floor",
                table: "Customer",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "FlatNumber",
                table: "Customer",
                newName: "TwitterX");

            migrationBuilder.RenameColumn(
                name: "BuildingNumber",
                table: "Customer",
                newName: "TikTok");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Customer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Customer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Customer",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Customer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Customer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaxNumber",
                table: "Customer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "Customer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "Customer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Customer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Customer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerCategoryId",
                table: "Customer",
                column: "CustomerCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerCategory_CustomerCategoryId",
                table: "Customer",
                column: "CustomerCategoryId",
                principalTable: "CustomerCategory",
                principalColumn: "Id");
        }
    }
}
