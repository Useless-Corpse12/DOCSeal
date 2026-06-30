using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOCSeal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrganisationAndAddInviteCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employees",
                table: "Organisations");

            migrationBuilder.AddColumn<List<string>>(
                name: "PossiblePositions",
                table: "Organisations",
                type: "text[]",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "OrganisationInviteCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uuid", nullable: false),
                    InviteCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationInviteCodes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganisationInviteCodes");

            migrationBuilder.DropColumn(
                name: "PossiblePositions",
                table: "Organisations");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "Employees",
                table: "Organisations",
                type: "uuid[]",
                nullable: false);
        }
    }
}
