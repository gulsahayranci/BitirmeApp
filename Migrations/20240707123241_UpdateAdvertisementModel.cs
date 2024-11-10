using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitirmeApp3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdvertisementModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alan",
                table: "Advertisements",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Advertisements",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "Advertisements",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Advertisements",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alan",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Advertisements");
        }
    }
}
