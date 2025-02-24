using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_service_number.Migrations
{
    /// <inheritdoc />
    public partial class PopulaTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Tickets",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
            
            mb.Sql("Insert INTO tickets(TicketNumber, Priority, Status, StartDate, EndDate) VALUES ('PRE001', 'PregnantWoman', 'Active', now(), null)");
            mb.Sql("Insert INTO tickets(TicketNumber, Priority, Status, StartDate, EndDate) VALUES ('ELD001', 'Elderly', 'Active', now(), null)");
            mb.Sql("Insert INTO tickets(TicketNumber, Priority, Status, StartDate, EndDate) VALUES ('COM001', 'Common', 'Active', now(), null)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Tickets",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);
        }
    }
}