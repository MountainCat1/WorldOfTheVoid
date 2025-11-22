using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldOfTheVoid.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPeriodicWorkerLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeriodicWorkerLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateStarted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Time = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodicWorkerLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeriodicWorkerLogs");
        }
    }
}
