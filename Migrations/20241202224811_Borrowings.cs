using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grosu_Olesea_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class Borrowings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Verifică existența coloanei 'ID' înainte de a o șterge
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'BookCategory' AND COLUMN_NAME = 'ID'
                )
                BEGIN
                    ALTER TABLE BookCategory DROP COLUMN ID;
                END
            ");

            // Modifică tipul de date pentru coloana 'Price' din tabela 'Book'
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Book",
                type: "decimal(6,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            // Creează tabela 'Member'
            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.ID);
                });

            // Creează tabela 'Borrowing'
            migrationBuilder.CreateTable(
                name: "Borrowing",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: true),
                    BookID = table.Column<int>(type: "int", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowing", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Borrowing_Book_BookID",
                        column: x => x.BookID,
                        principalTable: "Book",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Borrowing_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "ID");
                });

            // Creează indexuri pentru tabela 'Borrowing'
            migrationBuilder.CreateIndex(
                name: "IX_Borrowing_BookID",
                table: "Borrowing",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowing_MemberID",
                table: "Borrowing",
                column: "MemberID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Șterge tabela 'Borrowing'
            migrationBuilder.DropTable(
                name: "Borrowing");

            // Șterge tabela 'Member'
            migrationBuilder.DropTable(
                name: "Member");

            // Adaugă înapoi coloana 'ID' în tabela 'BookCategory'
            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "BookCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Reface tipul de date al coloanei 'Price' în tabela 'Book'
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Book",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)");
        }
    }
}
