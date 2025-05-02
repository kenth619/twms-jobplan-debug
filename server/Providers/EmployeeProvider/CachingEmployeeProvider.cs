﻿using Microsoft.Extensions.Caching.Memory;
using TWMSServer.Model;
using TWMSServer.Providers.Secrets;

namespace TWMSServer.Providers.EmployeeProvider
{
    public class CachingEmployeeProvider : IEmployeeProvider
    {
        private readonly MemoryCacheEntryOptions CACHE_OPTIONS;

        private readonly IMemoryCache _cache;

        private readonly IEmployeeProvider _baseProvider;

        public CachingEmployeeProvider(ISecretsProvider secretsProvider, IMemoryCache cache)
        {
            _cache = cache;
            _baseProvider = new EmployeeProvider(secretsProvider);

            int minsToKeepInCache = 120;
            CACHE_OPTIONS = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minsToKeepInCache));
        }

        public async Task<Employee?> FindEmployeeByEmployeeNumber(string employeeNumber)
        {
            if (!_cache.TryGetValue(employeeNumber, out Employee? cachedEmployee))
            {
                //cache miss
                cachedEmployee = await _baseProvider.FindEmployeeByEmployeeNumber(employeeNumber);
                if (cachedEmployee != null)
                {
                    _cache.Set(employeeNumber, cachedEmployee, CACHE_OPTIONS);
                }
            }

            return cachedEmployee;
        }

        public async Task<Employee?> FindEmployeeByUserName(string username)
        {
            Employee? employee = await _baseProvider.FindEmployeeByUserName(username);
            if (employee != null)
            {
                _cache.Set(employee.EmployeeNumber, employee, CACHE_OPTIONS);
            }
            return employee;
        }

        public async Task<List<Employee>> AllEmployees()
        {
            return await _baseProvider.AllEmployees();
        }

        public async Task<List<Employee>> AllActiveEmployees()
        {
            return await _baseProvider.AllActiveEmployees();
        }

        public async Task<List<Department>> GetDepartments()
        {
            return await _baseProvider.GetDepartments();
        }

        public async Task<Department?> FindDepartmentByCode(string departmentCode)
        {
            return await _baseProvider.FindDepartmentByCode(departmentCode);
        }
    }
}
