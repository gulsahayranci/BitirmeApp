using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitirmeApp3.Migrations
{
    /// <inheritdoc />
    public partial class RenameSurNameToLastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "AdApplications",
                newName: "LastName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AdApplications",
                newName: "Surname");
        }
    }
}
