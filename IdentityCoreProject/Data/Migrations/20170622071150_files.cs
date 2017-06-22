using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IdentityCoreProject.Data.Migrations
{
    public partial class files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileAttachmentId",
                table: "WebNotes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileAttachment_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebNotes_FileAttachmentId",
                table: "WebNotes",
                column: "FileAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_ApplicationUserId",
                table: "FileAttachment",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebNotes_FileAttachment_FileAttachmentId",
                table: "WebNotes",
                column: "FileAttachmentId",
                principalTable: "FileAttachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNotes_FileAttachment_FileAttachmentId",
                table: "WebNotes");

            migrationBuilder.DropTable(
                name: "FileAttachment");

            migrationBuilder.DropIndex(
                name: "IX_WebNotes_FileAttachmentId",
                table: "WebNotes");

            migrationBuilder.DropColumn(
                name: "FileAttachmentId",
                table: "WebNotes");
        }
    }
}
