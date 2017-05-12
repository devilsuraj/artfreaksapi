using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace artfriks.Data.Migrations
{
    public partial class changecountrytable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Phonecode",
                table: "Country",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Numcode",
                table: "Country",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phonecode",
                table: "Country",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Numcode",
                table: "Country",
                nullable: true);
        }
    }
}
