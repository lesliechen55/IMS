using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YQTrack.Core.Backend.Admin.Data.Migrations
{
    public partial class AddManagerField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FAvatar",
                table: "TManager",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FLastLoginTime",
                table: "TManager",
                nullable: false,
                defaultValueSql: "getutcdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FAvatar",
                table: "TManager");

            migrationBuilder.DropColumn(
                name: "FLastLoginTime",
                table: "TManager");
        }
    }
}
