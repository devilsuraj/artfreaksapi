using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace artfriks.Data.Migrations
{
    public partial class subcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "ArtCategories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ArtCategories");
        }
    }
}
