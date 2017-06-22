using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityCoreProject.Data.Migrations
{
    public partial class files3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachment_AspNetUsers_ApplicationUserId",
                table: "FileAttachment");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "FileAttachment",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachment_ApplicationUserId",
                table: "FileAttachment",
                newName: "IX_FileAttachment_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachment_AspNetUsers_UserId",
                table: "FileAttachment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachment_AspNetUsers_UserId",
                table: "FileAttachment");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FileAttachment",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachment_UserId",
                table: "FileAttachment",
                newName: "IX_FileAttachment_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachment_AspNetUsers_ApplicationUserId",
                table: "FileAttachment",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
