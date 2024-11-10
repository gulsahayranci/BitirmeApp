using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitirmeApp3.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToAdApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AdApplications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "AdApplications");
        }
    }
}
