using Microsoft.EntityFrameworkCore;
using TWMSServer.Model;
using TWMSServer.Model.Enum;
using TWMSServer.Providers.EmployeeProvider;

namespace TWMSServer.Providers
{
        public class EmployeeRolesProvider(ILogger<EmployeeRolesProvider> logger, IDbContextFactory<TWMSServerContext> contextFactory, IEmployeeProvider employeeProvider)
    {
        private readonly ILogger<EmployeeRolesProvider> _logger = logger;
        private readonly TWMSServerContext _context = contextFactory.CreateDbContext();
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;

        public async Task<List<EmployeeWithRoles>> GetAllEmployeesWithRoles()
        {
            var employees = await _employeeProvider.AllEmployees();
            var result = new List<EmployeeWithRoles>();

            foreach (var employee in employees)
            {
                var systemRoles = await GetSystemRoles(employee.EmployeeNumber);
                var departmentRoles = await GetDepartmentRoleMappings(employee.EmployeeNumber);

                result.Add(new EmployeeWithRoles(employee, systemRoles, departmentRoles));
            }

            return result;
        }

        public async Task<EmployeeWithRoles?> GetEmployeeWithRoles(string employeeNumber)
        {
            var employee = await _employeeProvider.FindEmployeeByEmployeeNumber(employeeNumber);

            if (employee == null)
            {
                return null;
            }

            var systemRoles = await GetSystemRoles(employeeNumber);
            var departmentRoles = await GetDepartmentRoleMappings(employeeNumber);

            return new EmployeeWithRoles(employee, systemRoles, departmentRoles);
        }

        public async Task<bool> AssignSystemRole(string employeeNumber, SystemRole role)
        {
            try
            {
                // Check if the employee exists
                var employee = await _employeeProvider.FindEmployeeByEmployeeNumber(employeeNumber);
                if (employee == null)
                {
                    _logger.LogWarning("Cannot assign system role to non-existent employee: {EmployeeNumber}", employeeNumber);
                    return false;
                }

                // Check if the role assignment already exists
                var existingAssignment = await _context.SystemRoleAssignments
                    .FirstOrDefaultAsync(r => r.EmployeeNumber == employeeNumber && r.Role == role);

                if (existingAssignment != null)
                {
                    // Role already assigned
                    return true;
                }

                // Create new role assignment
                var newAssignment = new SystemRoleAssignment
                {
                    EmployeeNumber = employeeNumber,
                    Role = role
                };

                await _context.SystemRoleAssignments.AddAsync(newAssignment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning system role {RoleKey} to employee {EmployeeNumber}",
                    role.Key, employeeNumber);
                return false;
            }
        }

        public async Task<bool> RemoveSystemRole(string employeeNumber, SystemRole role)
        {
            try
            {
                var assignment = await _context.SystemRoleAssignments
                    .FirstOrDefaultAsync(r => r.EmployeeNumber == employeeNumber && r.Role == role);

                if (assignment == null)
                {
                    // Role not assigned, nothing to remove
                    return true;
                }

                _context.SystemRoleAssignments.Remove(assignment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing system role {RoleKey} from employee {EmployeeNumber}",
                    role.Key, employeeNumber);
                return false;
            }
        }

        public async Task<bool> AssignDepartmentRole(string employeeNumber, string departmentCode, DepartmentRole role)
        {
            try
            {
                // Check if the employee exists
                var employee = await _employeeProvider.FindEmployeeByEmployeeNumber(employeeNumber);
                if (employee == null)
                {
                    _logger.LogWarning("Cannot assign department role to non-existent employee: {EmployeeNumber}", employeeNumber);
                    return false;
                }

                // Check if the department exists
                var department = await _employeeProvider.FindDepartmentByCode(departmentCode);
                if (department == null)
                {
                    _logger.LogWarning("Cannot assign role for non-existent department: {DepartmentCode}", departmentCode);
                    return false;
                }

                // Check if the role assignment already exists
                var existingAssignment = await _context.DepartmentRoleAssignments
                    .FirstOrDefaultAsync(r => r.EmployeeNumber == employeeNumber &&
                                             r.DepartmentCode == departmentCode &&
                                             r.Role == role);

                if (existingAssignment != null)
                {
                    // Role already assigned
                    return true;
                }

                // Create new role assignment
                var newAssignment = new DepartmentRoleAssignment
                {
                    EmployeeNumber = employeeNumber,
                    DepartmentCode = departmentCode,
                    Role = role
                };

                await _context.DepartmentRoleAssignments.AddAsync(newAssignment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning department role {RoleKey} for department {DepartmentCode} to employee {EmployeeNumber}",
                    role.Key, departmentCode, employeeNumber);
                return false;
            }
        }

        public async Task<bool> RemoveDepartmentRole(string employeeNumber, string departmentCode, DepartmentRole role)
        {
            try
            {
                var assignment = await _context.DepartmentRoleAssignments
                    .FirstOrDefaultAsync(r => r.EmployeeNumber == employeeNumber &&
                                             r.DepartmentCode == departmentCode &&
                                             r.Role == role);

                if (assignment == null)
                {
                    // Role not assigned, nothing to remove
                    return true;
                }

                _context.DepartmentRoleAssignments.Remove(assignment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing department role {RoleKey} for department {DepartmentCode} from employee {EmployeeNumber}",
                    role.Key, departmentCode, employeeNumber);
                return false;
            }
        }

        public async Task<bool> HasSystemRole(string employeeNumber, SystemRole role)
        {
            return await _context.SystemRoleAssignments.AnyAsync(r => r.EmployeeNumber == employeeNumber && r.Role == role);
        }

        public async Task<bool> HasDepartmentRole(string employeeNumber, string departmentCode, DepartmentRole role)
        {
            return await _context.DepartmentRoleAssignments
                .AnyAsync(r => r.EmployeeNumber == employeeNumber &&
                              r.DepartmentCode == departmentCode &&
                              r.Role == role);
        }

        public async Task<List<SystemRole>> GetSystemRoles(string employeeNumber)
        {
            var roleAssignments = await _context.SystemRoleAssignments.Where(r => r.EmployeeNumber == employeeNumber).ToListAsync();
            return [.. roleAssignments.Select(r => r.Role)];
        }

        public async Task<List<DepartmentRoleMappingEntry>> GetDepartmentRoleMappings(string employeeNumber)
        {
            var roleAssignments = await _context.DepartmentRoleAssignments.Where(r => r.EmployeeNumber == employeeNumber).ToListAsync();
            return [.. roleAssignments.Select(r => new DepartmentRoleMappingEntry(r.DepartmentCode, r.Role))];
        }

        public async Task<List<Employee>> GetEmployeesWithSystemRole(SystemRole role)
        {
            try
            {
                // Get all employee numbers with the specified role
                var employeeNumbers = await _context.SystemRoleAssignments
                    .Where(r => r.Role == role)
                    .Select(r => r.EmployeeNumber)
                    .ToListAsync();

                // Get the employee details for each employee number
                var employees = new List<Employee>();
                foreach (var employeeNumber in employeeNumbers)
                {
                    var employee = await _employeeProvider.FindEmployeeByEmployeeNumber(employeeNumber);
                    if (employee != null)
                    {
                        employees.Add(employee);
                    }
                }

                return employees;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employees with system role {RoleKey}", role.Key);
                return [];
            }
        }
    }
}