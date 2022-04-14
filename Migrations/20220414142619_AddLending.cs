using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibAPI.Migrations
{
    public partial class AddLending : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passports_Clients_Id",
                table: "Passports");

            migrationBuilder.AddColumn<string>(
                name: "AddressId",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    House = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Building = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lendings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Numper = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayCount = table.Column<byte>(type: "tinyint", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lendings_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lendings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Clients_AddressId",
                table: "Clients",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookLending_LendingId",
                table: "BookLending",
                column: "LendingId");

            migrationBuilder.CreateIndex(
                name: "IX_Lendings_ClientId",
                table: "Lendings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Lendings_CreatorId",
                table: "Lendings",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Addresses_AddressId",
                table: "Clients",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Passports_Clients_Id",
                table: "Passports",
                column: "Id",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Addresses_AddressId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Passports_Clients_Id",
                table: "Passports");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "BookLending");

            migrationBuilder.DropTable(
                name: "Lendings");

            migrationBuilder.DropIndex(
                name: "IX_Clients_AddressId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Clients");

            migrationBuilder.AddForeignKey(
                name: "FK_Passports_Clients_Id",
                table: "Passports",
                column: "Id",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
