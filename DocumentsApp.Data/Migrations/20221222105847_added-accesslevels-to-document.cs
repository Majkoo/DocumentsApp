using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentsApp.Data.Migrations
{
    public partial class addedaccesslevelstodocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessLevel",
                table: "DocumentAccessLevels",
                newName: "AccessLevelEnum");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessLevelEnum",
                table: "DocumentAccessLevels",
                newName: "AccessLevel");
        }
    }
}
