using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZamZamAPI.Migrations
{
    /// <inheritdoc />
    public partial class newtableAboutUscreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionLabel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MainHeading = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExperienceDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProductionDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CapabilityTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CapabilityDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUs");
        }
    }
}
