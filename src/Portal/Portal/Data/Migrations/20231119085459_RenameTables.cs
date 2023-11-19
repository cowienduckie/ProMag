using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Task_ParentId",
                schema: "Portal",
                table: "Attachment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStatus_Project_ProjectId",
                schema: "Portal",
                table: "ProjectStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Project_ProjectsId",
                schema: "Portal",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Task_TasksId",
                schema: "Portal",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_Project_ProjectId",
                schema: "Portal",
                table: "Section");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionTask_Section_SectionsId",
                schema: "Portal",
                table: "SectionTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionTask_Task_TasksId",
                schema: "Portal",
                table: "SectionTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Story_Task_TargetId",
                schema: "Portal",
                table: "Story");

            migrationBuilder.DropForeignKey(
                name: "FK_TagFollower_Tag_TagId",
                schema: "Portal",
                table: "TagFollower");

            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Tag_TagsId",
                schema: "Portal",
                table: "TagTask");

            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Task_TasksId",
                schema: "Portal",
                table: "TagTask");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFollower_Task_TaskId",
                schema: "Portal",
                table: "TaskFollower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskFollower",
                schema: "Portal",
                table: "TaskFollower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                schema: "Portal",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagFollower",
                schema: "Portal",
                table: "TagFollower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                schema: "Portal",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Story",
                schema: "Portal",
                table: "Story");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Section",
                schema: "Portal",
                table: "Section");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStatus",
                schema: "Portal",
                table: "ProjectStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                schema: "Portal",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachment",
                schema: "Portal",
                table: "Attachment");

            migrationBuilder.RenameTable(
                name: "TaskFollower",
                schema: "Portal",
                newName: "TaskFollowers",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Task",
                schema: "Portal",
                newName: "Tasks",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "TagFollower",
                schema: "Portal",
                newName: "TagFollowers",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Tag",
                schema: "Portal",
                newName: "Tags",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Story",
                schema: "Portal",
                newName: "Stories",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Section",
                schema: "Portal",
                newName: "Sections",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "ProjectStatus",
                schema: "Portal",
                newName: "ProjectStatuses",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Project",
                schema: "Portal",
                newName: "Projects",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Attachment",
                schema: "Portal",
                newName: "Attachments",
                newSchema: "Portal");

            migrationBuilder.RenameIndex(
                name: "IX_Story_TargetId",
                schema: "Portal",
                table: "Stories",
                newName: "IX_Stories_TargetId");

            migrationBuilder.RenameIndex(
                name: "IX_Section_ProjectId",
                schema: "Portal",
                table: "Sections",
                newName: "IX_Sections_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectStatus_ProjectId",
                schema: "Portal",
                table: "ProjectStatuses",
                newName: "IX_ProjectStatuses_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Attachment_ParentId",
                schema: "Portal",
                table: "Attachments",
                newName: "IX_Attachments_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskFollowers",
                schema: "Portal",
                table: "TaskFollowers",
                columns: new[] { "TaskId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                schema: "Portal",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagFollowers",
                schema: "Portal",
                table: "TagFollowers",
                columns: new[] { "TagId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                schema: "Portal",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stories",
                schema: "Portal",
                table: "Stories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sections",
                schema: "Portal",
                table: "Sections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStatuses",
                schema: "Portal",
                table: "ProjectStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                schema: "Portal",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachments",
                schema: "Portal",
                table: "Attachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Tasks_ParentId",
                schema: "Portal",
                table: "Attachments",
                column: "ParentId",
                principalSchema: "Portal",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStatuses_Projects_ProjectId",
                schema: "Portal",
                table: "ProjectStatuses",
                column: "ProjectId",
                principalSchema: "Portal",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Projects_ProjectsId",
                schema: "Portal",
                table: "ProjectTask",
                column: "ProjectsId",
                principalSchema: "Portal",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Tasks_TasksId",
                schema: "Portal",
                table: "ProjectTask",
                column: "TasksId",
                principalSchema: "Portal",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Projects_ProjectId",
                schema: "Portal",
                table: "Sections",
                column: "ProjectId",
                principalSchema: "Portal",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionTask_Sections_SectionsId",
                schema: "Portal",
                table: "SectionTask",
                column: "SectionsId",
                principalSchema: "Portal",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionTask_Tasks_TasksId",
                schema: "Portal",
                table: "SectionTask",
                column: "TasksId",
                principalSchema: "Portal",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Tasks_TargetId",
                schema: "Portal",
                table: "Stories",
                column: "TargetId",
                principalSchema: "Portal",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagFollowers_Tags_TagId",
                schema: "Portal",
                table: "TagFollowers",
                column: "TagId",
                principalSchema: "Portal",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Tags_TagsId",
                schema: "Portal",
                table: "TagTask",
                column: "TagsId",
                principalSchema: "Portal",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Tasks_TasksId",
                schema: "Portal",
                table: "TagTask",
                column: "TasksId",
                principalSchema: "Portal",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFollowers_Tasks_TaskId",
                schema: "Portal",
                table: "TaskFollowers",
                column: "TaskId",
                principalSchema: "Portal",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Tasks_ParentId",
                schema: "Portal",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStatuses_Projects_ProjectId",
                schema: "Portal",
                table: "ProjectStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Projects_ProjectsId",
                schema: "Portal",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Tasks_TasksId",
                schema: "Portal",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Projects_ProjectId",
                schema: "Portal",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionTask_Sections_SectionsId",
                schema: "Portal",
                table: "SectionTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionTask_Tasks_TasksId",
                schema: "Portal",
                table: "SectionTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Tasks_TargetId",
                schema: "Portal",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_TagFollowers_Tags_TagId",
                schema: "Portal",
                table: "TagFollowers");

            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Tags_TagsId",
                schema: "Portal",
                table: "TagTask");

            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Tasks_TasksId",
                schema: "Portal",
                table: "TagTask");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFollowers_Tasks_TaskId",
                schema: "Portal",
                table: "TaskFollowers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                schema: "Portal",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskFollowers",
                schema: "Portal",
                table: "TaskFollowers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                schema: "Portal",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagFollowers",
                schema: "Portal",
                table: "TagFollowers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stories",
                schema: "Portal",
                table: "Stories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sections",
                schema: "Portal",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStatuses",
                schema: "Portal",
                table: "ProjectStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                schema: "Portal",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                schema: "Portal",
                table: "Attachments");

            migrationBuilder.RenameTable(
                name: "Tasks",
                schema: "Portal",
                newName: "Task",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "TaskFollowers",
                schema: "Portal",
                newName: "TaskFollower",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Tags",
                schema: "Portal",
                newName: "Tag",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "TagFollowers",
                schema: "Portal",
                newName: "TagFollower",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Stories",
                schema: "Portal",
                newName: "Story",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Sections",
                schema: "Portal",
                newName: "Section",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "ProjectStatuses",
                schema: "Portal",
                newName: "ProjectStatus",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "Portal",
                newName: "Project",
                newSchema: "Portal");

            migrationBuilder.RenameTable(
                name: "Attachments",
                schema: "Portal",
                newName: "Attachment",
                newSchema: "Portal");

            migrationBuilder.RenameIndex(
                name: "IX_Stories_TargetId",
                schema: "Portal",
                table: "Story",
                newName: "IX_Story_TargetId");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_ProjectId",
                schema: "Portal",
                table: "Section",
                newName: "IX_Section_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectStatuses_ProjectId",
                schema: "Portal",
                table: "ProjectStatus",
                newName: "IX_ProjectStatus_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_ParentId",
                schema: "Portal",
                table: "Attachment",
                newName: "IX_Attachment_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                schema: "Portal",
                table: "Task",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskFollower",
                schema: "Portal",
                table: "TaskFollower",
                columns: new[] { "TaskId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                schema: "Portal",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagFollower",
                schema: "Portal",
                table: "TagFollower",
                columns: new[] { "TagId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Story",
                schema: "Portal",
                table: "Story",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Section",
                schema: "Portal",
                table: "Section",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStatus",
                schema: "Portal",
                table: "ProjectStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                schema: "Portal",
                table: "Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachment",
                schema: "Portal",
                table: "Attachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Task_ParentId",
                schema: "Portal",
                table: "Attachment",
                column: "ParentId",
                principalSchema: "Portal",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStatus_Project_ProjectId",
                schema: "Portal",
                table: "ProjectStatus",
                column: "ProjectId",
                principalSchema: "Portal",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Project_ProjectsId",
                schema: "Portal",
                table: "ProjectTask",
                column: "ProjectsId",
                principalSchema: "Portal",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Task_TasksId",
                schema: "Portal",
                table: "ProjectTask",
                column: "TasksId",
                principalSchema: "Portal",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Project_ProjectId",
                schema: "Portal",
                table: "Section",
                column: "ProjectId",
                principalSchema: "Portal",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionTask_Section_SectionsId",
                schema: "Portal",
                table: "SectionTask",
                column: "SectionsId",
                principalSchema: "Portal",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionTask_Task_TasksId",
                schema: "Portal",
                table: "SectionTask",
                column: "TasksId",
                principalSchema: "Portal",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Story_Task_TargetId",
                schema: "Portal",
                table: "Story",
                column: "TargetId",
                principalSchema: "Portal",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagFollower_Tag_TagId",
                schema: "Portal",
                table: "TagFollower",
                column: "TagId",
                principalSchema: "Portal",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Tag_TagsId",
                schema: "Portal",
                table: "TagTask",
                column: "TagsId",
                principalSchema: "Portal",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Task_TasksId",
                schema: "Portal",
                table: "TagTask",
                column: "TasksId",
                principalSchema: "Portal",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFollower_Task_TaskId",
                schema: "Portal",
                table: "TaskFollower",
                column: "TaskId",
                principalSchema: "Portal",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
