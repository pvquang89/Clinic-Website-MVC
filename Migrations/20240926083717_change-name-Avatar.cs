using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPhongKham.Migrations
{
    /// <inheritdoc />
    public partial class changenameAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "DonHangs",
                newName: "HinhAnh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HinhAnh",
                table: "DonHangs",
                newName: "Avatar");
        }
    }
}
