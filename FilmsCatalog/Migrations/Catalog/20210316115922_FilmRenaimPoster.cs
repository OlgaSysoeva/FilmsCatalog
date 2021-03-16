using Microsoft.EntityFrameworkCore.Migrations;

namespace FilmsCatalog.Migrations.Catalog
{
    public partial class FilmRenaimPoster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Poster",
                table: "Films",
                newName: "PosterPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PosterPath",
                table: "Films",
                newName: "Poster");
        }
    }
}
