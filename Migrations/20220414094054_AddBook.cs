using Microsoft.EntityFrameworkCore.Migrations;

namespace LibAPI.Migrations
{
    public partial class AddBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishYear = table.Column<short>(type: "smallint", nullable: true),
                    PublisherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CoverType = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PayType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookScores",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalCount = table.Column<int>(type: "int", nullable: false),
                    GivenCount = table.Column<int>(type: "int", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookScores_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Style_BookId",
                table: "Style",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_BookId",
                table: "Authors",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_BookScores_BookId",
                table: "BookScores",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Books_BookId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Style_Books_BookId",
                table: "Style");

            migrationBuilder.DropTable(
                name: "BookScores");

            migrationBuilder.DropTable(
                name: "Books");

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
        }
    }
}
