using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityCoreProject.Data.Migrations
{
    public partial class files4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNotes_FileAttachment_FileAttachmentId",
                table: "WebNotes");

            migrationBuilder.DropIndex(
                name: "IX_WebNotes_FileAttachmentId",
                table: "WebNotes");

            migrationBuilder.DropColumn(
                name: "FileAttachmentId",
                table: "WebNotes");

            migrationBuilder.AddColumn<int>(
                name: "WebNoteId",
                table: "FileAttachment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_WebNoteId",
                table: "FileAttachment",
                column: "WebNoteId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachment_WebNotes_WebNoteId",
                table: "FileAttachment",
                column: "WebNoteId",
                principalTable: "WebNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachment_WebNotes_WebNoteId",
                table: "FileAttachment");

            migrationBuilder.DropIndex(
                name: "IX_FileAttachment_WebNoteId",
                table: "FileAttachment");

            migrationBuilder.DropColumn(
                name: "WebNoteId",
                table: "FileAttachment");

            migrationBuilder.AddColumn<int>(
                name: "FileAttachmentId",
                table: "WebNotes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebNotes_FileAttachmentId",
                table: "WebNotes",
                column: "FileAttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebNotes_FileAttachment_FileAttachmentId",
                table: "WebNotes",
                column: "FileAttachmentId",
                principalTable: "FileAttachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
