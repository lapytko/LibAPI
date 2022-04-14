using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibAPI.Migrations
{
    public partial class AddLendingItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookLending");

            migrationBuilder.DropColumn(
                name: "DayCount",
                table: "Lendings");

            migrationBuilder.RenameColumn(
                name: "Numper",
                table: "Lendings",
                newName: "Number");

            migrationBuilder.CreateTable(
                name: "LendingItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayCount = table.Column<byte>(type: "tinyint", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LendingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LendingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LendingItems_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LendingItems_Lendings_LendingId",
                        column: x => x.LendingId,
                        principalTable: "Lendings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LendingItems_BookId",
                table: "LendingItems",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_LendingItems_LendingId",
                table: "LendingItems",
                column: "LendingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LendingItems");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Lendings",
                newName: "Numper");

            migrationBuilder.AddColumn<byte>(
                name: "DayCount",
                table: "Lendings",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "BookLending",
                columns: table => new
                {
                    BooksId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LendingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLending", x => new { x.BooksId, x.LendingId });
                    table.ForeignKey(
                        name: "FK_BookLending_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLending_Lendings_LendingId",
                        column: x => x.LendingId,
                        principalTable: "Lendings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookLending_LendingId",
                table: "BookLending",
                column: "LendingId");
        }
    }
}
