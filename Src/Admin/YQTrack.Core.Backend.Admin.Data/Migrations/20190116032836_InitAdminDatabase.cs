using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YQTrack.Core.Backend.Admin.Data.Migrations
{
    public partial class InitAdminDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TManager",
                columns: table => new
                {
                    FId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FCreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    FCreatedBy = table.Column<int>(nullable: false),
                    FIsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    FUpdatedTime = table.Column<DateTime>(nullable: true),
                    FUpdatedBy = table.Column<int>(nullable: true),
                    FNickName = table.Column<string>(maxLength: 16, nullable: false),
                    FAccount = table.Column<string>(maxLength: 16, nullable: false),
                    FPassword = table.Column<string>(maxLength: 32, nullable: false),
                    FIsLock = table.Column<bool>(nullable: false, defaultValue: false),
                    FRemark = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TManager", x => x.FId);
                });

            migrationBuilder.CreateTable(
                name: "TManagerRole",
                columns: table => new
                {
                    FManagerId = table.Column<int>(nullable: false),
                    FRoleId = table.Column<int>(nullable: false),
                    FCreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    FCreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TManagerRole", x => new { x.FManagerId, x.FRoleId });
                });

            migrationBuilder.CreateTable(
                name: "TPermission",
                columns: table => new
                {
                    FId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FCreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    FCreatedBy = table.Column<int>(nullable: false),
                    FIsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    FUpdatedTime = table.Column<DateTime>(nullable: true),
                    FUpdatedBy = table.Column<int>(nullable: true),
                    FAreaName = table.Column<string>(maxLength: 16, nullable: true),
                    FControllerName = table.Column<string>(maxLength: 32, nullable: true),
                    FActionName = table.Column<string>(maxLength: 32, nullable: true),
                    FFullName = table.Column<string>(maxLength: 64, nullable: false),
                    FUrl = table.Column<string>(maxLength: 128, nullable: false),
                    FParentId = table.Column<int>(nullable: true),
                    FSort = table.Column<int>(nullable: false),
                    FRemark = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPermission", x => x.FId);
                });

            migrationBuilder.CreateTable(
                name: "TRole",
                columns: table => new
                {
                    FId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FCreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    FCreatedBy = table.Column<int>(nullable: false),
                    FIsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    FUpdatedTime = table.Column<DateTime>(nullable: true),
                    FUpdatedBy = table.Column<int>(nullable: true),
                    FName = table.Column<string>(maxLength: 16, nullable: false),
                    FIsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    FRemark = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRole", x => x.FId);
                });

            migrationBuilder.CreateTable(
                name: "TRolePermission",
                columns: table => new
                {
                    FRoleId = table.Column<int>(nullable: false),
                    FPermissionId = table.Column<int>(nullable: false),
                    FCreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    FCreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRolePermission", x => new { x.FRoleId, x.FPermissionId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TManager");

            migrationBuilder.DropTable(
                name: "TManagerRole");

            migrationBuilder.DropTable(
                name: "TPermission");

            migrationBuilder.DropTable(
                name: "TRole");

            migrationBuilder.DropTable(
                name: "TRolePermission");
        }
    }
}
