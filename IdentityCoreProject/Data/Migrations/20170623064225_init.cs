using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IdentityCoreProject.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "WebNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WebNotes_FileId",
                table: "WebNotes",
                column: "FileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNotes_FileAttachments_FileId",
                table: "WebNotes");

            migrationBuilder.DropTable(
                name: "FileAttachments");

            migrationBuilder.DropIndex(
                name: "IX_WebNotes_FileId",
                table: "WebNotes");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "WebNotes");
        }
    }
}
