using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_service_number.Migrations
{
    /// <inheritdoc />
    public partial class removeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeoLocationData",
                table: "Tickets");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeoLocationData",
                table: "Tickets");

        }
    }
}
