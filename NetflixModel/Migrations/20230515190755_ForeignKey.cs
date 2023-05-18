using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetflixModel.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Genre",
                newName: "Genre1");

            migrationBuilder.AlterColumn<int>(
                name: "GenreID",
                table: "Shows",
                type: "int",
                fixedLength: true,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10);

            migrationBuilder.CreateIndex(
                name: "IX_Shows_GenreID",
                table: "Shows",
                column: "GenreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Genre_GenreID",
                table: "Shows",
                column: "GenreID",
                principalTable: "Genre",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Genre_GenreID",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_GenreID",
                table: "Shows");

            migrationBuilder.RenameColumn(
                name: "Genre1",
                table: "Genre",
                newName: "Genre");

            migrationBuilder.AlterColumn<string>(
                name: "GenreID",
                table: "Shows",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldFixedLength: true);
        }
    }
}
