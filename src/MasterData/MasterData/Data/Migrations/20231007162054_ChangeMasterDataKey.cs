using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MasterData.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMasterDataKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                schema: "Master",
                table: "Languages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                schema: "Master",
                table: "Currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                schema: "Master",
                table: "Countries");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Master",
                table: "Timezones",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Master",
                table: "Languages",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Master",
                table: "Currencies",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Master",
                table: "Countries",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timezones",
                schema: "Master",
                table: "Timezones",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                schema: "Master",
                table: "Languages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                schema: "Master",
                table: "Currencies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                schema: "Master",
                table: "Countries",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Timezones",
                schema: "Master",
                table: "Timezones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                schema: "Master",
                table: "Languages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                schema: "Master",
                table: "Currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                schema: "Master",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Master",
                table: "Timezones");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Master",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Master",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Master",
                table: "Countries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                schema: "Master",
                table: "Languages",
                column: "Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                schema: "Master",
                table: "Currencies",
                column: "Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                schema: "Master",
                table: "Countries",
                column: "Code");
        }
    }
}
