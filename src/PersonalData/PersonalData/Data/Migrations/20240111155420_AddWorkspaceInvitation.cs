using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalData.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkspaceInvitation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "Personal",
                table: "Workspaces");

            migrationBuilder.CreateTable(
                name: "WorkspaceInvitations",
                schema: "Personal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitedPersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Accepted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceInvitations_People_InvitedPersonId",
                        column: x => x.InvitedPersonId,
                        principalSchema: "Personal",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceInvitations_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "Personal",
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceInvitations_InvitedPersonId",
                schema: "Personal",
                table: "WorkspaceInvitations",
                column: "InvitedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceInvitations_WorkspaceId",
                schema: "Personal",
                table: "WorkspaceInvitations",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkspaceInvitations",
                schema: "Personal");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "Personal",
                table: "Workspaces",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
