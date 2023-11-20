using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSectionTaskOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SectionTask",
                schema: "Portal");

            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                schema: "Portal",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SectionId",
                schema: "Portal",
                table: "Tasks",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sections_SectionId",
                schema: "Portal",
                table: "Tasks",
                column: "SectionId",
                principalSchema: "Portal",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sections_SectionId",
                schema: "Portal",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SectionId",
                schema: "Portal",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SectionId",
                schema: "Portal",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "SectionTask",
                schema: "Portal",
                columns: table => new
                {
                    SectionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TasksId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionTask", x => new { x.SectionsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_SectionTask_Sections_SectionsId",
                        column: x => x.SectionsId,
                        principalSchema: "Portal",
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionTask_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalSchema: "Portal",
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SectionTask_TasksId",
                schema: "Portal",
                table: "SectionTask",
                column: "TasksId");
        }
    }
}
