using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePortal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Portal",
                table: "Tasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Portal",
                table: "Tags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Portal",
                table: "Stories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Portal",
                table: "Sections",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Portal",
                table: "ProjectStatuses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Portal",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Portal",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Portal",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Portal",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Portal",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Portal",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Portal",
                table: "ProjectStatuses");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Portal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Portal",
                table: "Attachments");
        }
    }
}
