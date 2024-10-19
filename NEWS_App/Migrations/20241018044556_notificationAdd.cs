using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEWS_App.Migrations
{
    /// <inheritdoc />
    public partial class notificationAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notifications",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notifications",
                table: "Users");
        }
    }
}
