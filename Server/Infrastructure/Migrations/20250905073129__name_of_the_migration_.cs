using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UseCaseService.Migrations
{
    /// <inheritdoc />
    public partial class _name_of_the_migration_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "processed_on",
                table: "outbox_event");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "processed_on",
                table: "outbox_event",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
