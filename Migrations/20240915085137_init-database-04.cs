using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPhongKham.Migrations
{
    /// <inheritdoc />
    public partial class initdatabase04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiaChis_LoaiTinhs_idLoaiTinh",
                table: "DiaChis");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHangs_DiaChis_idDiaChi",
                table: "KhachHangs");

            migrationBuilder.AlterColumn<int>(
                name: "idDiaChi",
                table: "KhachHangs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "SoTien",
                table: "KhachHangs",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "KhachHangs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "KhachHangs",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "idLoaiTinh",
                table: "DiaChis",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DiaChis_LoaiTinhs_idLoaiTinh",
                table: "DiaChis",
                column: "idLoaiTinh",
                principalTable: "LoaiTinhs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHangs_DiaChis_idDiaChi",
                table: "KhachHangs",
                column: "idDiaChi",
                principalTable: "DiaChis",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiaChis_LoaiTinhs_idLoaiTinh",
                table: "DiaChis");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHangs_DiaChis_idDiaChi",
                table: "KhachHangs");

            migrationBuilder.AlterColumn<int>(
                name: "idDiaChi",
                table: "KhachHangs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "SoTien",
                table: "KhachHangs",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "KhachHangs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "KhachHangs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "idLoaiTinh",
                table: "DiaChis",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DiaChis_LoaiTinhs_idLoaiTinh",
                table: "DiaChis",
                column: "idLoaiTinh",
                principalTable: "LoaiTinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHangs_DiaChis_idDiaChi",
                table: "KhachHangs",
                column: "idDiaChi",
                principalTable: "DiaChis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
