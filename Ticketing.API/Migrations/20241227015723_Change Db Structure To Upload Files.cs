using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDbStructureToUploadFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolutionGuideFile_Files_SolutionGuideId",
                table: "SolutionGuideFile");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketFile_Files_FileId",
                table: "TicketFile");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropIndex(
                name: "IX_TicketFile_FileId",
                table: "TicketFile");

            migrationBuilder.DropIndex(
                name: "IX_SolutionGuideFile_SolutionGuideId",
                table: "SolutionGuideFile");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "TicketFile");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "SolutionGuideFile");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TicketFile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "TicketFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TicketFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalName",
                table: "TicketFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "TicketFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "TicketFile",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TicketFile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SolutionGuideFile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "SolutionGuideFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SolutionGuideFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalName",
                table: "SolutionGuideFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "SolutionGuideFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "SolutionGuideFile",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SolutionGuideFile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SolutionGuideFile_SolutionGuideId",
                table: "SolutionGuideFile",
                column: "SolutionGuideId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SolutionGuideFile_SolutionGuideId",
                table: "SolutionGuideFile");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TicketFile");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "TicketFile");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TicketFile");

            migrationBuilder.DropColumn(
                name: "OriginalName",
                table: "TicketFile");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "TicketFile");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "TicketFile");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TicketFile");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SolutionGuideFile");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "SolutionGuideFile");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SolutionGuideFile");

            migrationBuilder.DropColumn(
                name: "OriginalName",
                table: "SolutionGuideFile");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "SolutionGuideFile");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "SolutionGuideFile");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SolutionGuideFile");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "TicketFile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "SolutionGuideFile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<double>(type: "float", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketFile_FileId",
                table: "TicketFile",
                column: "FileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolutionGuideFile_SolutionGuideId",
                table: "SolutionGuideFile",
                column: "SolutionGuideId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SolutionGuideFile_Files_SolutionGuideId",
                table: "SolutionGuideFile",
                column: "SolutionGuideId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketFile_Files_FileId",
                table: "TicketFile",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
