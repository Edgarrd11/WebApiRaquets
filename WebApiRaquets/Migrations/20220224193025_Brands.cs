using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiRaquets.Migrations
{
    public partial class Brands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Player = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    RaquetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brands_Raquets_RaquetId",
                        column: x => x.RaquetId,
                        principalTable: "Raquets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brands_RaquetId",
                table: "Brands",
                column: "RaquetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
