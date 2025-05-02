using TWMSServer.Model.Enum;

namespace TWMSServer.Model
{
    public class DepartmentRoleAssignment
    {
        public int DepartmentRoleAssignmentId { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public DepartmentRole Role { get; set; } = DepartmentRole.None;
    }
}
