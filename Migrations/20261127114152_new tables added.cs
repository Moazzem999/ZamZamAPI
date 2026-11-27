using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZamZamAPI.Migrations
{
    /// <inheritdoc />
    public partial class newtablesadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecurityManus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentManuId = table.Column<long>(type: "bigint", nullable: true),
                    ManuName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ControllerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityManus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityManus_SecurityManus_ParentManuId",
                        column: x => x.ParentManuId,
                        principalTable: "SecurityManus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SecurityRolePermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    SecurityManuId = table.Column<long>(type: "bigint", nullable: false),
                    IsView = table.Column<bool>(type: "bit", nullable: false),
                    IsInsert = table.Column<bool>(type: "bit", nullable: false),
                    IsModify = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsReport = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityRolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityRolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityRolePermissions_SecurityManus_SecurityManuId",
                        column: x => x.SecurityManuId,
                        principalTable: "SecurityManus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SecurityManus_ParentManuId",
                table: "SecurityManus",
                column: "ParentManuId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRolePermissions_RoleId",
                table: "SecurityRolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRolePermissions_SecurityManuId",
                table: "SecurityRolePermissions",
                column: "SecurityManuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecurityRolePermissions");

            migrationBuilder.DropTable(
                name: "SecurityManus");
        }
    }
}
