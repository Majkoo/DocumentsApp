using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentsApp.Data.Migrations
{
    public partial class AddedAccessLevelDateGranted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AccessLevelDateGranted",
                table: "DocumentAccessLevels",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessLevelDateGranted",
                table: "DocumentAccessLevels");
        }
    }
}
