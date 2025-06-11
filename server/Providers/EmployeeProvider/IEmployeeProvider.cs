using TWMSServer.Model;

namespace TWMSServer.Providers.EmployeeProvider
{
    public interface IEmployeeProvider
    {
        public Task<Employee?> FindEmployeeByUserName(string username);
        public Task<Employee?> FindEmployeeByEmployeeNumber(string employeeNumber);
        public Task<List<Employee>> AllEmployees();
        public Task<List<Employee>> AllActiveEmployees();
        public Task<List<Department>> GetDepartments();
        public Task<Department?> FindDepartmentByCode(string departmentCode);
    }
}
