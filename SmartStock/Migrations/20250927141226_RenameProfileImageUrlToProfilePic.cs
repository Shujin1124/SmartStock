using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.Migrations
{
    /// <inheritdoc />
    public partial class RenameProfileImageUrlToProfilePic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "profiles");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePic",
                table: "profiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "profiles");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "profiles",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
