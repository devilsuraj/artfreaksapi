using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace artfriks.Data.Migrations
{
    public partial class changecountrytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Iso",
                table: "Country",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Iso3",
                table: "Country",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NiceName",
                table: "Country",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Numcode",
                table: "Country",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Phonecode",
                table: "Country",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iso",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Iso3",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "NiceName",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Numcode",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Phonecode",
                table: "Country");
        }
    }
}
