using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityCoreProject.Data.Migrations
{
    public partial class files2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "FileAttachment");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "FileAttachment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "FileAttachment");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "FileAttachment",
                nullable: true);
        }
    }
}
