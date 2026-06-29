using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOCSeal.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokensAndUserPositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPosition_Users_UserId",
                table: "UserPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPosition",
                table: "UserPosition");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiresAt",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "UserPosition",
                newName: "UserPositions");

            migrationBuilder.RenameIndex(
                name: "IX_UserPosition_UserId",
                table: "UserPositions",
                newName: "IX_UserPositions_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserPositions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    RefreshTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BrowserFingerPrint = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions");

            migrationBuilder.RenameTable(
                name: "UserPositions",
                newName: "UserPosition");

            migrationBuilder.RenameIndex(
                name: "IX_UserPositions_UserId",
                table: "UserPosition",
                newName: "IX_UserPosition_UserId");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiresAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserPosition",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPosition",
                table: "UserPosition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPosition_Users_UserId",
                table: "UserPosition",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
