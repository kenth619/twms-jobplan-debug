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
                name: "tblAssets_Classes",
                columns: table => new
                {
                    AssetClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetClassShortDesc = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    AssetClassLongDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", maxLength: 50, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAssets_Classes", x => x.AssetClassId);
                });

            migrationBuilder.CreateTable(
                name: "tblJob_Plans_Headers",
                columns: table => new
                {
                    JobPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPlanStatus = table.Column<bool>(type: "bit", maxLength: 25, nullable: false),
                    JobPlanShortDesc = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    JobPlanLongDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetClassId = table.Column<int>(type: "int", maxLength: 25, nullable: false),
                    AssetClassShortDesc = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblJob_Plans_Headers", x => x.JobPlanId);
                });

            migrationBuilder.CreateTable(
                name: "tblWorkOrderHeader",
                columns: table => new
                {
                    WorkOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssociatedWONumber = table.Column<int>(type: "int", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTypeSubCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    SourceDocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceDocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostCentre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblWorkOrderHeader", x => x.WorkOrderId);
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

            migrationBuilder.CreateTable(
                name: "tblJob_Plans_Lines",
                columns: table => new
                {
                    Job_Plan_Line_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Job_Plan_ID = table.Column<int>(type: "int", nullable: false),
                    Job_Plan_Line_No = table.Column<long>(type: "bigint", nullable: false),
                    Job_Plan_Line_Desc = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblJob_Plans_Lines", x => x.Job_Plan_Line_ID);
                    table.ForeignKey(
                        name: "FK_tblJob_Plans_Lines_tblJob_Plans_Headers_Job_Plan_ID",
                        column: x => x.Job_Plan_ID,
                        principalTable: "tblJob_Plans_Headers",
                        principalColumn: "JobPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailAttachment_EmailDataId",
                table: "EmailAttachment",
                column: "EmailDataId");

            migrationBuilder.CreateIndex(
                name: "IX_tblJob_Plans_Lines_Job_Plan_ID",
                table: "tblJob_Plans_Lines",
                column: "Job_Plan_ID");
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
                name: "tblJob_Plans_Lines");

            migrationBuilder.DropTable(
                name: "tblAssets_Classes");

            migrationBuilder.DropTable(
                name: "tblWorkOrderHeader");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "tblJob_Plans_Headers");
        }
    }
}
