using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassicalApi.Core.Migrations
{
    /// <inheritdoc />
    public partial class RefactorPortrait : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portrait_Composers_ComposerId",
                table: "Portraits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portrait",
                table: "Portraits");

            migrationBuilder.DropIndex(
                name: "IX_Portrait_ComposerId",
                table: "Portraits");

            migrationBuilder.DropColumn(
                name: "ComposerId",
                table: "Portraits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portraits",
                table: "Portraits",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Composers_PortraitId",
                table: "Composers",
                column: "PortraitId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Composers_Portraits_PortraitId",
                table: "Composers",
                column: "PortraitId",
                principalTable: "Portraits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Composers_Portraits_PortraitId",
                table: "Composers");

            migrationBuilder.DropIndex(
                name: "IX_Composers_PortraitId",
                table: "Composers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portraits",
                table: "Portraits");

            migrationBuilder.RenameTable(
                name: "Portraits",
                newName: "Portrait");

            migrationBuilder.AddColumn<int>(
                name: "ComposerId",
                table: "Portrait",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portrait",
                table: "Portrait",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Portrait_ComposerId",
                table: "Portrait",
                column: "ComposerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Portrait_Composers_ComposerId",
                table: "Portrait",
                column: "ComposerId",
                principalTable: "Composers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
