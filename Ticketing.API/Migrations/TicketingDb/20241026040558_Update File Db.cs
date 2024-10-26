using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.API.Migrations.TicketingDb
{
    /// <inheritdoc />
    public partial class UpdateFileDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "File",
                newName: "MimeType");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "File",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MimeType",
                table: "File",
                newName: "Type");

            migrationBuilder.AlterColumn<double>(
                name: "Path",
                table: "File",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
