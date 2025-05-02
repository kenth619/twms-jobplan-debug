using TWMSServer.Model.Enum;

namespace TWMSServer.Model
{
    public class EmployeeWithRoles(Employee employee, List<SystemRole> systemRoles, List<DepartmentRoleMappingEntry> departmentRoleMapping)
    {
        public string Username { get; set; } = employee.Username;
        public string EmployeeNumber { get; set; } = employee.EmployeeNumber;
        public string FirstName { get; set; } = employee.FirstName;
        public string LastName { get; set; } = employee.LastName;
        public string Department { get; set; } = employee.Department;
        public string Division { get; set; } = employee.Division;
        public string EmployeeStatus { get; set; } = employee.EmployeeStatus;
        public string? Email { get; set; } = employee.Email;
        public string JobName { get; set; } = employee.JobName;
        public string Grade { get; set; } = employee.Grade;

        public string FullName => $"{FirstName} {LastName}";
        public string Label => $"{FirstName} {LastName} ({EmployeeNumber}) - Dept: {Department}";

        public List<SystemRole> SystemRoles { get; set; } = employee.EmployeeStatus == "A" ? systemRoles : [];
        public List<DepartmentRoleMappingEntry> DepartmentRoleMapping { get; set; } = employee.EmployeeStatus == "A" ? departmentRoleMapping : [];
    }
}
