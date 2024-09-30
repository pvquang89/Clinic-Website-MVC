using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPhongKham.Migrations
{
    /// <inheritdoc />
    public partial class initdatabase03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhachHangs_DiaChis_DiaChiId",
                table: "KhachHangs");

            migrationBuilder.DropIndex(
                name: "IX_KhachHangs_DiaChiId",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "DiaChiId",
                table: "KhachHangs");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHangs_idDiaChi",
                table: "KhachHangs",
                column: "idDiaChi");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHangs_DiaChis_idDiaChi",
                table: "KhachHangs",
                column: "idDiaChi",
                principalTable: "DiaChis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhachHangs_DiaChis_idDiaChi",
                table: "KhachHangs");

            migrationBuilder.DropIndex(
                name: "IX_KhachHangs_idDiaChi",
                table: "KhachHangs");

            migrationBuilder.AddColumn<int>(
                name: "DiaChiId",
                table: "KhachHangs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KhachHangs_DiaChiId",
                table: "KhachHangs",
                column: "DiaChiId");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHangs_DiaChis_DiaChiId",
                table: "KhachHangs",
                column: "DiaChiId",
                principalTable: "DiaChis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
