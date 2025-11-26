using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyVendorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_VendorCategory_VendorCategoryId",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_Vendor_VendorCategoryId",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "FaxNumber",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Vendor");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Vendor",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "WhatsApp",
                table: "Vendor",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Vendor",
                newName: "Floor");

            migrationBuilder.RenameColumn(
                name: "VendorCategoryId",
                table: "Vendor",
                newName: "TRN");

            migrationBuilder.RenameColumn(
                name: "TwitterX",
                table: "Vendor",
                newName: "FlatNumber");

            migrationBuilder.RenameColumn(
                name: "TikTok",
                table: "Vendor",
                newName: "BuildingNumber");

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "Vendor",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Vendor",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GovernorateId",
                table: "Vendor",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Vendor");

            migrationBuilder.RenameColumn(
                name: "TRN",
                table: "Vendor",
                newName: "VendorCategoryId");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Vendor",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Vendor",
                newName: "WhatsApp");

            migrationBuilder.RenameColumn(
                name: "Floor",
                table: "Vendor",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "FlatNumber",
                table: "Vendor",
                newName: "TwitterX");

            migrationBuilder.RenameColumn(
                name: "BuildingNumber",
                table: "Vendor",
                newName: "TikTok");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Vendor",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaxNumber",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_VendorCategoryId",
                table: "Vendor",
                column: "VendorCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_VendorCategory_VendorCategoryId",
                table: "Vendor",
                column: "VendorCategoryId",
                principalTable: "VendorCategory",
                principalColumn: "Id");
        }
    }
}
