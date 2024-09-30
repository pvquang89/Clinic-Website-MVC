using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPhongKham.Migrations
{
    /// <inheritdoc />
    public partial class initdatabase02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiaChis_LoaiTinhs_LoaiTinhId",
                table: "DiaChis");

            migrationBuilder.DropIndex(
                name: "IX_DiaChis_LoaiTinhId",
                table: "DiaChis");

            migrationBuilder.DropColumn(
                name: "LoaiTinhId",
                table: "DiaChis");

            migrationBuilder.AlterColumn<int>(
                name: "idLoaiTinh",
                table: "DiaChis",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChis_idLoaiTinh",
                table: "DiaChis",
                column: "idLoaiTinh");

            migrationBuilder.AddForeignKey(
                name: "FK_DiaChis_LoaiTinhs_idLoaiTinh",
                table: "DiaChis",
                column: "idLoaiTinh",
                principalTable: "LoaiTinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiaChis_LoaiTinhs_idLoaiTinh",
                table: "DiaChis");

            migrationBuilder.DropIndex(
                name: "IX_DiaChis_idLoaiTinh",
                table: "DiaChis");

            migrationBuilder.AlterColumn<string>(
                name: "idLoaiTinh",
                table: "DiaChis",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LoaiTinhId",
                table: "DiaChis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DiaChis_LoaiTinhId",
                table: "DiaChis",
                column: "LoaiTinhId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiaChis_LoaiTinhs_LoaiTinhId",
                table: "DiaChis",
                column: "LoaiTinhId",
                principalTable: "LoaiTinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
