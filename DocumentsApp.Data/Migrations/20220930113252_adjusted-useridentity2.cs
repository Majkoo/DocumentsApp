using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentsApp.Data.Migrations
{
    public partial class adjusteduseridentity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAccessLevels_AspNetUsers_AccountId1",
                table: "DocumentAccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAccessLevels_Documents_DocumentId",
                table: "DocumentAccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_AccountId1",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_AccountId1",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_DocumentAccessLevels_AccountId1",
                table: "DocumentAccessLevels");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "DocumentAccessLevels");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Documents",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Documents",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Documents",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentId",
                table: "DocumentAccessLevels",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "DocumentAccessLevels",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DocumentAccessLevels",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AccountId",
                table: "Documents",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccessLevels_AccountId",
                table: "DocumentAccessLevels",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAccessLevels_AspNetUsers_AccountId",
                table: "DocumentAccessLevels",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAccessLevels_Documents_DocumentId",
                table: "DocumentAccessLevels",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_AccountId",
                table: "Documents",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAccessLevels_AspNetUsers_AccountId",
                table: "DocumentAccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAccessLevels_Documents_DocumentId",
                table: "DocumentAccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_AccountId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_AccountId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_DocumentAccessLevels_AccountId",
                table: "DocumentAccessLevels");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Documents",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Documents",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Documents",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AccountId1",
                table: "Documents",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "DocumentAccessLevels",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "DocumentAccessLevels",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "DocumentAccessLevels",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AccountId1",
                table: "DocumentAccessLevels",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AccountId1",
                table: "Documents",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccessLevels_AccountId1",
                table: "DocumentAccessLevels",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAccessLevels_AspNetUsers_AccountId1",
                table: "DocumentAccessLevels",
                column: "AccountId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAccessLevels_Documents_DocumentId",
                table: "DocumentAccessLevels",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_AccountId1",
                table: "Documents",
                column: "AccountId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
