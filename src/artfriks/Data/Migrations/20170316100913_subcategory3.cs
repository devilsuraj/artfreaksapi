using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace artfriks.Data.Migrations
{
    public partial class subcategory3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Orientation",
                table: "ArtWorks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Orientation",
                table: "ArtWorks");
        }
    }
}
