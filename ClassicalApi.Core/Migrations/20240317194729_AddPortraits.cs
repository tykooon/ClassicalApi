using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassicalApi.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddPortraits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortraitId",
                table: "Composers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Portrait",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ComposerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portrait", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portrait_Composers_ComposerId",
                        column: x => x.ComposerId,
                        principalTable: "Composers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Portrait_ComposerId",
                table: "Portrait",
                column: "ComposerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Portrait");

            migrationBuilder.DropColumn(
                name: "PortraitId",
                table: "Composers");
        }
    }
}
