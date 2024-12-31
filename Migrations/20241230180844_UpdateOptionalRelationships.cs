using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogsitesi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOptionalRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yorums_Makales_MakaleId",
                table: "Yorums");

            migrationBuilder.AlterColumn<int>(
                name: "MakaleId",
                table: "Yorums",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Yorums_Makales_MakaleId",
                table: "Yorums",
                column: "MakaleId",
                principalTable: "Makales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yorums_Makales_MakaleId",
                table: "Yorums");

            migrationBuilder.AlterColumn<int>(
                name: "MakaleId",
                table: "Yorums",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Yorums_Makales_MakaleId",
                table: "Yorums",
                column: "MakaleId",
                principalTable: "Makales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
