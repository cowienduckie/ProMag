using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalData.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePersonalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Personal",
                table: "Workspaces",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "Personal",
                table: "Workspaces",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Personal",
                table: "Teams",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Personal",
                table: "People",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Personal",
                table: "Organizations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Personal",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "Personal",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Personal",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Personal",
                table: "People");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Personal",
                table: "Organizations");
        }
    }
}
