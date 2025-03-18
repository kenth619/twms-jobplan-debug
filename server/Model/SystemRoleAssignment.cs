using TemplateProject.Model.Enum;

namespace TemplateProject.Model
{
    public class SystemRoleAssignment
    {
        public int SystemRoleAssignmentId { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public SystemRole Role { get; set; } = SystemRole.None;
    }
}
