using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityCoreProject.Migrations
{
    public partial class files2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebNotes_FileId",
                table: "WebNotes");

            migrationBuilder.AddColumn<int>(
                name: "WebNoteId",
                table: "FileAttachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WebNotes_FileId",
                table: "WebNotes",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_WebNoteId",
                table: "FileAttachments",
                column: "WebNoteId");


            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_WebNotes_WebNoteId",
                table: "FileAttachments",
                column: "WebNoteId",
                principalTable: "WebNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_WebNotes_WebNoteId",
                table: "FileAttachments");

            migrationBuilder.DropIndex(
                name: "IX_WebNotes_FileId",
                table: "WebNotes");

            migrationBuilder.DropIndex(
                name: "IX_FileAttachments_WebNoteId",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "WebNoteId",
                table: "FileAttachments");

            migrationBuilder.CreateIndex(
                name: "IX_WebNotes_FileId",
                table: "WebNotes",
                column: "FileId",
                unique: true);
        }
    }
}
