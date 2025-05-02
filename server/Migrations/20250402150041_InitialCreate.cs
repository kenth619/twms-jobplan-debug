using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWMSServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentRoleAssignments",
                columns: table => new
                {
                    DepartmentRoleAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentRoleAssignments", x => x.DepartmentRoleAssignmentId);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    EmailDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SentSuccessfullyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateDataRaw = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToRecipients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCRecipients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BCCRecipients = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.EmailDataId);
                });

            migrationBuilder.CreateTable(
                name: "JobSchedules",
                columns: table => new
                {
                    JobScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CronExpression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LastRun = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSchedules", x => x.JobScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "SystemRoleAssignments",
                columns: table => new
                {
                    SystemRoleAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoleAssignments", x => x.SystemRoleAssignmentId);
                });

            migrationBuilder.CreateTable(
                name: "EmailAttachment",
                columns: table => new
                {
                    EmailAttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAttachment", x => x.EmailAttachmentId);
                    table.ForeignKey(
                        name: "FK_EmailAttachment_Emails_EmailDataId",
                        column: x => x.EmailDataId,
                        principalTable: "Emails",
                        principalColumn: "EmailDataId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailAttachment_EmailDataId",
                table: "EmailAttachment",
                column: "EmailDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentRoleAssignments");

            migrationBuilder.DropTable(
                name: "EmailAttachment");

            migrationBuilder.DropTable(
                name: "JobSchedules");

            migrationBuilder.DropTable(
                name: "SystemRoleAssignments");

            migrationBuilder.DropTable(
                name: "Emails");
        }
    }
}
