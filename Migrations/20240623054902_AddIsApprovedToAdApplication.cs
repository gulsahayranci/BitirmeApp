using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitirmeApp3.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedToAdApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdApplications_Advertisements_AdvertisementId",
                table: "AdApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_AdApplications_AspNetUsers_UserId",
                table: "AdApplications");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AdApplications",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdvertisementId",
                table: "AdApplications",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "AdApplications",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AdApplications_Advertisements_AdvertisementId",
                table: "AdApplications",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdApplications_AspNetUsers_UserId",
                table: "AdApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdApplications_Advertisements_AdvertisementId",
                table: "AdApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_AdApplications_AspNetUsers_UserId",
                table: "AdApplications");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "AdApplications");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AdApplications",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AdvertisementId",
                table: "AdApplications",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_AdApplications_Advertisements_AdvertisementId",
                table: "AdApplications",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdApplications_AspNetUsers_UserId",
                table: "AdApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
