using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityCoreProject.Data.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebnoteSortingOption",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebnoteSortingOption",
                table: "AspNetUsers");
        }
    }
}
