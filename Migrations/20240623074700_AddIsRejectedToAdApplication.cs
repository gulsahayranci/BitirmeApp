using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitirmeApp3.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRejectedToAdApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "AdApplications",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "AdApplications");
        }
    }
}
