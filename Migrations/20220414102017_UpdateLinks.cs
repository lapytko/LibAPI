using Microsoft.EntityFrameworkCore.Migrations;

namespace LibAPI.Migrations
{
    public partial class UpdateLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Books_BookId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Style_Books_BookId",
                table: "Style");

            migrationBuilder.DropIndex(
                name: "IX_Style_BookId",
                table: "Style");

            migrationBuilder.DropIndex(
                name: "IX_Authors_BookId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Style");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Authors");

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BooksId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorId, x.BooksId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookStyle",
                columns: table => new
                {
                    BooksId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StyleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookStyle", x => new { x.BooksId, x.StyleId });
                    table.ForeignKey(
                        name: "FK_BookStyle_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookStyle_Style_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Style",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksId",
                table: "AuthorBook",
                column: "BooksId");

            migrationBuilder.CreateIndex(
                name: "IX_BookStyle_StyleId",
                table: "BookStyle",
                column: "StyleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropTable(
                name: "BookStyle");

            migrationBuilder.AddColumn<string>(
                name: "BookId",
                table: "Style",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookId",
                table: "Authors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Style_BookId",
                table: "Style",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_BookId",
                table: "Authors",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Books_BookId",
                table: "Authors",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Style_Books_BookId",
                table: "Style",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
