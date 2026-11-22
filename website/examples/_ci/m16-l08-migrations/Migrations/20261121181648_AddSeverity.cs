using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace m16l08migrations.Migrations;

/// <inheritdoc />
public partial class _20261121181648_AddSeverity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "Severity",
            table: "Incidents",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Severity",
            table: "Incidents");
    }
}
