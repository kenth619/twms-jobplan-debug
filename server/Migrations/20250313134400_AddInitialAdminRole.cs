using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add superuser role to employee 10057
            migrationBuilder.InsertData(
                table: "SystemRoleAssignments",
                columns: ["EmployeeNumber", "Role"],
                values: ["10057", "superuser"]
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the administrator role from the default user
            migrationBuilder.DeleteData(
                table: "SystemRoleAssignments",
                keyColumn: "EmployeeNumber",
                keyValue: "10057"
            );
        }
    }
}