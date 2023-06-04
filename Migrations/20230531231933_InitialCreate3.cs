using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alea.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "likovi",
                table: "Knjige",
                newName: "gLikovi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "gLikovi",
                table: "Knjige",
                newName: "likovi");
        }
    }
}
