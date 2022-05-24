using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Added_UID_and_column_constraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortenedUrl",
                table: "Urls");

            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentifier",
                table: "Urls",
                type: "TEXT",
                maxLength: 6,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueIdentifier",
                table: "Urls");

            migrationBuilder.AddColumn<string>(
                name: "ShortenedUrl",
                table: "Urls",
                type: "TEXT",
                maxLength: 30,
                nullable: true);
        }
    }
}
