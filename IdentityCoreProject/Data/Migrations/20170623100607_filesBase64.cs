using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityCoreProject.Data.Migrations
{
    public partial class filesBase64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileData",
                table: "FileAttachments",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "FileAttachments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "FileAttachments");

            migrationBuilder.AlterColumn<byte[]>(
                name: "FileData",
                table: "FileAttachments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
