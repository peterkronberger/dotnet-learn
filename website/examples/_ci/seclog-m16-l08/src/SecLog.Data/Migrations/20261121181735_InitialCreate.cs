using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecLog.Data.Migrations;

/// <inheritdoc />
public partial class _20261121181735_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Incidents",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                User = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                Source = table.Column<string>(type: "TEXT", nullable: false),
                Status = table.Column<string>(type: "TEXT", nullable: false),
                FailureCount = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Incidents", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Events",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                EventType = table.Column<string>(type: "TEXT", nullable: false),
                User = table.Column<string>(type: "TEXT", nullable: true),
                StoredIncidentId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Events", x => x.Id);
                table.ForeignKey(
                    name: "FK_Events_Incidents_StoredIncidentId",
                    column: x => x.StoredIncidentId,
                    principalTable: "Incidents",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Events_StoredIncidentId",
            table: "Events",
            column: "StoredIncidentId");

        migrationBuilder.CreateIndex(
            name: "IX_Incidents_User",
            table: "Incidents",
            column: "User");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Events");

        migrationBuilder.DropTable(
            name: "Incidents");
    }
}
