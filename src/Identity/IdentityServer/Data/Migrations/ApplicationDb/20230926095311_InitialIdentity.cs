#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer.Data.Migrations.ApplicationDb;

/// <inheritdoc />
public partial class InitialIdentity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            "Identity");

        migrationBuilder.RenameTable(
            "UserTokens",
            newName: "UserTokens",
            newSchema: "Identity");

        migrationBuilder.RenameTable(
            "Users",
            newName: "Users",
            newSchema: "Identity");

        migrationBuilder.RenameTable(
            "UserRoles",
            newName: "UserRoles",
            newSchema: "Identity");

        migrationBuilder.RenameTable(
            "UserLogins",
            newName: "UserLogins",
            newSchema: "Identity");

        migrationBuilder.RenameTable(
            "UserClaims",
            newName: "UserClaims",
            newSchema: "Identity");

        migrationBuilder.RenameTable(
            "Roles",
            newName: "Roles",
            newSchema: "Identity");

        migrationBuilder.RenameTable(
            "RoleClaims",
            newName: "RoleClaims",
            newSchema: "Identity");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            "UserTokens",
            "Identity",
            "UserTokens");

        migrationBuilder.RenameTable(
            "Users",
            "Identity",
            "Users");

        migrationBuilder.RenameTable(
            "UserRoles",
            "Identity",
            "UserRoles");

        migrationBuilder.RenameTable(
            "UserLogins",
            "Identity",
            "UserLogins");

        migrationBuilder.RenameTable(
            "UserClaims",
            "Identity",
            "UserClaims");

        migrationBuilder.RenameTable(
            "Roles",
            "Identity",
            "Roles");

        migrationBuilder.RenameTable(
            "RoleClaims",
            "Identity",
            "RoleClaims");
    }
}