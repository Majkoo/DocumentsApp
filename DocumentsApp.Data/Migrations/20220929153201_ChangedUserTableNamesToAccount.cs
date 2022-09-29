using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentsApp.Data.Migrations
{
    public partial class ChangedUserTableNamesToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAccessLevels_Users_UserId",
                table: "DocumentAccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_CreatorId",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Accounts");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Documents",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_CreatorId",
                table: "Documents",
                newName: "IX_Documents_AccountId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DocumentAccessLevels",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentAccessLevels_UserId",
                table: "DocumentAccessLevels",
                newName: "IX_DocumentAccessLevels_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAccessLevels_Accounts_AccountId",
                table: "DocumentAccessLevels",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Accounts_AccountId",
                table: "Documents",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAccessLevels_Accounts_AccountId",
                table: "DocumentAccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Accounts_AccountId",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Documents",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_AccountId",
                table: "Documents",
                newName: "IX_Documents_CreatorId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "DocumentAccessLevels",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentAccessLevels_AccountId",
                table: "DocumentAccessLevels",
                newName: "IX_DocumentAccessLevels_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAccessLevels_Users_UserId",
                table: "DocumentAccessLevels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_CreatorId",
                table: "Documents",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
