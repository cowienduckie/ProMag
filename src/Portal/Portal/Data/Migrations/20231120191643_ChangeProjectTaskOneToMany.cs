using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProjectTaskOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTask",
                schema: "Portal");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                schema: "Portal",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                schema: "Portal",
                table: "Tasks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                schema: "Portal",
                table: "Tasks",
                column: "ProjectId",
                principalSchema: "Portal",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                schema: "Portal",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ProjectId",
                schema: "Portal",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                schema: "Portal",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "ProjectTask",
                schema: "Portal",
                columns: table => new
                {
                    ProjectsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TasksId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTask", x => new { x.ProjectsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_ProjectTask_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalSchema: "Portal",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTask_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalSchema: "Portal",
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_TasksId",
                schema: "Portal",
                table: "ProjectTask",
                column: "TasksId");
        }
    }
}
