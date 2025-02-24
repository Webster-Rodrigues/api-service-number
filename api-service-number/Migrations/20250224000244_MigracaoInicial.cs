using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_service_number.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                    name: "Tickets",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                        TicketNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Priority = table.Column<int>(type: "int", nullable: false),
                        Status = table.Column<int>(type: "int", nullable: false),
                        StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Tickets", x => x.Id);
                    })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}