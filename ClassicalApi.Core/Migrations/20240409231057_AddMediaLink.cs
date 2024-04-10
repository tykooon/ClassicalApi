using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassicalApi.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComposerMediaLink",
                columns: table => new
                {
                    ComposersId = table.Column<int>(type: "INTEGER", nullable: false),
                    MediaLinksId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposerMediaLink", x => new { x.ComposersId, x.MediaLinksId });
                    table.ForeignKey(
                        name: "FK_ComposerMediaLink_Composers_ComposersId",
                        column: x => x.ComposersId,
                        principalTable: "Composers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComposerMediaLink_MediaLinks_MediaLinksId",
                        column: x => x.MediaLinksId,
                        principalTable: "MediaLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComposerMediaLink_MediaLinksId",
                table: "ComposerMediaLink",
                column: "MediaLinksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComposerMediaLink");

            migrationBuilder.DropTable(
                name: "MediaLinks");
        }
    }
}
