using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityCoreProject.Data.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WebNotes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebNotes_UserId",
                table: "WebNotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebNotes_AspNetUsers_UserId",
                table: "WebNotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNotes_AspNetUsers_UserId",
                table: "WebNotes");

            migrationBuilder.DropIndex(
                name: "IX_WebNotes_UserId",
                table: "WebNotes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WebNotes");
        }
    }
}
