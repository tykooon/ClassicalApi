using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassicalApi.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Composers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    YearOfBirth = table.Column<int>(type: "INTEGER", nullable: true),
                    YearOfDeath = table.Column<int>(type: "INTEGER", nullable: true),
                    CountryOfBirth = table.Column<string>(type: "TEXT", nullable: false),
                    CityOfBirth = table.Column<string>(type: "TEXT", nullable: false),
                    ShortBio = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Composers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Composers");
        }
    }
}
