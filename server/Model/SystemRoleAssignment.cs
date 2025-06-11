using TWMSServer.Model.Enum;

namespace TWMSServer.Model
{
    public class SystemRoleAssignment
    {
        public int SystemRoleAssignmentId { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public SystemRole Role { get; set; } = SystemRole.None;
    }
}
