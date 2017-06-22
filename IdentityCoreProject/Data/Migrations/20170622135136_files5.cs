using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityCoreProject.Data.Migrations
{
    public partial class files5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachment_AspNetUsers_UserId",
                table: "FileAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachment_WebNotes_WebNoteId",
                table: "FileAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileAttachment",
                table: "FileAttachment");

            migrationBuilder.RenameTable(
                name: "FileAttachment",
                newName: "FileAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachment_WebNoteId",
                table: "FileAttachments",
                newName: "IX_FileAttachments_WebNoteId");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachment_UserId",
                table: "FileAttachments",
                newName: "IX_FileAttachments_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileAttachments",
                table: "FileAttachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_AspNetUsers_UserId",
                table: "FileAttachments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_WebNotes_WebNoteId",
                table: "FileAttachments",
                column: "WebNoteId",
                principalTable: "WebNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_AspNetUsers_UserId",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_WebNotes_WebNoteId",
                table: "FileAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileAttachments",
                table: "FileAttachments");

            migrationBuilder.RenameTable(
                name: "FileAttachments",
                newName: "FileAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_WebNoteId",
                table: "FileAttachment",
                newName: "IX_FileAttachment_WebNoteId");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_UserId",
                table: "FileAttachment",
                newName: "IX_FileAttachment_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileAttachment",
                table: "FileAttachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachment_AspNetUsers_UserId",
                table: "FileAttachment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachment_WebNotes_WebNoteId",
                table: "FileAttachment",
                column: "WebNoteId",
                principalTable: "WebNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
