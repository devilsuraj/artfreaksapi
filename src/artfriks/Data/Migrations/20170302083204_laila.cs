using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace artfriks.Data.Migrations
{
    public partial class laila : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Width",
                table: "ArtWorks",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "MediumString",
                table: "ArtWorks",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "ArtWorks",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "DimensionUnit",
                table: "ArtWorks",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "ArtWorks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Width",
                table: "ArtWorks",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MediumString",
                table: "ArtWorks",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Height",
                table: "ArtWorks",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DimensionUnit",
                table: "ArtWorks",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "ArtWorks",
                nullable: false);
        }
    }
}
