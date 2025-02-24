using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_service_number.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoAjuste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                    name: "Status",
                    table: "Tickets",
                    type: "varchar(20)",
                    maxLength: 20,
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                    name: "Priority",
                    table: "Tickets",
                    type: "varchar(20)",
                    maxLength: 20,
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                    name: "Status",
                    table: "Tickets",
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(20)",
                    oldMaxLength: 20)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                    name: "Priority",
                    table: "Tickets",
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(20)",
                    oldMaxLength: 20)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}