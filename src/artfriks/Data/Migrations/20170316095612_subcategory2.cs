using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace artfriks.Data.Migrations
{
    public partial class subcategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ArtCategories");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Categories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "ArtCategories",
                nullable: false,
                defaultValue: 0);
        }
    }
}
