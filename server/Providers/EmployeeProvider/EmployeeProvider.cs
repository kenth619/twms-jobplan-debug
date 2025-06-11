﻿using TWMSServer.Model;
using System.Data.SqlClient;
using TWMSServer.Providers.Secrets;

namespace TWMSServer.Providers.EmployeeProvider
{
    public class EmployeeProvider(ISecretsProvider secretsProvider) : IEmployeeProvider
    {
        private readonly ISecretsProvider _secretsProvider = secretsProvider;

        private static readonly string QUERY_COMMON = @"
                            SELECT vwEmployeeInformationExtra.[EmployeeNo],
                                            IIF([Active]=1, [UserName], '') AS UserName,
                                            [FirstName],
                                            [LastName],
                                            [DeptDesc],
                                            [DivisionDesc],
                                            [Email],
                                            [EmpStatus],
                                            COALESCE(vwEmployeeInformationExtra.Rank, 1) AS [CompositeRank],
                                            [Extension],
                                            [SignatureImage],
                                            [SignatureType],
                                            tblJobTypes.[JobName],
                                            tblJobTypes.[Grade]
                            FROM vwEmployeeInformationExtra
                            JOIN tblJobTypes on vwEmployeeInformationExtra.JobCode = tblJobTypes.JobCode
                            LEFT JOIN tblGradeRanks on vwEmployeeInformationExtra.Grade = tblGradeRanks.Grade ";


        private static readonly string QUERY_ACTIVE = QUERY_COMMON + "WHERE (vwEmployeeInformationExtra.EmpStatus = 'A') ";

        private static Employee? GetEmployeeFromReader(SqlDataReader reader)
        {
            byte[] sig;

            if (DBNull.Value != reader["SignatureImage"])
            {
                sig = (byte[])reader["SignatureImage"];
            }
            else
            {
                sig = [];
            }

            bool result = int.TryParse(reader["Extension"]?.ToString()?.Split("/")?.First(), out int extension);

            var employeeno = reader["EmployeeNo"].ToString();
            var username = reader["UserName"].ToString();
            var firstname = reader["FirstName"].ToString();
            var lastname = reader["LastName"].ToString();
            var deptdesc = reader["DeptDesc"].ToString();
            var divisiondesc = reader["DivisionDesc"].ToString();
            var email = reader["email"]?.ToString();
            var compositerank = reader["CompositeRank"].ToString();
            var signaturetype = reader["SignatureType"].ToString();
            var empStatus = reader["EmpStatus"].ToString();
            var jobName = reader["JobName"]?.ToString() ?? "";
            var grade = reader["Grade"]?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(employeeno) || string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
            {
                return null;
            }

            return new Employee
            {
                EmployeeNumber = employeeno,
                Username = username ?? "NO USERNAME",
                FirstName = firstname,
                LastName = lastname,
                Department = deptdesc ?? "",
                Division = divisiondesc ?? "",
                EmployeeStatus = empStatus ?? "I",
                Email = email,
                Rank = int.Parse(compositerank ?? "0"),
                Extension = result ? extension : null,
                Signature = sig,
                SignatureType = signaturetype ?? "",
                JobName = jobName,
                Grade = grade
            };
        }

        private Employee? GetEmployeeFromQuery(SqlCommand command)
        {
            using SqlConnection connection = new(_secretsProvider.GetSysIntegrationConnectionString());
            command.Connection = connection;
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    return GetEmployeeFromReader(reader);
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
            return null;
        }

        public async Task<Employee?> FindEmployeeByEmployeeNumber(string employeeNumber)
        {
            if (employeeNumber == "" || employeeNumber == null) { return null; }
            string query = QUERY_COMMON + @"WHERE vwEmployeeInformationExtra.EmployeeNo = @EmployeeNo ORDER BY [Active] DESC";
            SqlCommand command = new(query);
            command.Parameters.AddWithValue("@EmployeeNo", employeeNumber);
            return await Task.FromResult(GetEmployeeFromQuery(command));
        }

        public async Task<Employee?> FindEmployeeByUserName(string username)
        {
            if (username == "" || username == null) { return null; }
            string query = QUERY_ACTIVE + @"AND (vwEmployeeInformationExtra.UserName = @UserName OR vwEmployeeInformationExtra.email = @UserName)  ORDER BY [Active] DESC";
            SqlCommand command = new(query);
            command.Parameters.AddWithValue("@UserName", username);
            return await Task.FromResult(GetEmployeeFromQuery(command));
        }

        public async Task<List<Employee>> AllEmployees()
        {
            List<Employee> toReturn = [];

            using SqlConnection connection = new(_secretsProvider.GetSysIntegrationConnectionString());

            string query = QUERY_COMMON + @" ORDER BY FirstName ASC";
            SqlCommand command = new(query, connection);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    var employee = GetEmployeeFromReader(reader);
                    if (employee != null)
                    {
                        toReturn.Add(employee);
                    }
                }
            }
            catch
            {
                return toReturn;
            }
            finally
            {
                connection.Close();
            }

            // Ensure distinct employee numbers, prioritizing non-empty usernames
            var distinctEmployees = toReturn
                .GroupBy(e => e.EmployeeNumber)
                .Select(g => g.OrderByDescending(e => !string.IsNullOrEmpty(e.Username)).First())
                .ToList();

            return await Task.FromResult(distinctEmployees);
        }

        public async Task<List<Employee>> AllActiveEmployees()
        {
            List<Employee> toReturn = [];

            using SqlConnection connection = new(_secretsProvider.GetSysIntegrationConnectionString());

            string query = QUERY_ACTIVE + @" ORDER BY FirstName ASC";
            SqlCommand command = new(query, connection);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    var employee = GetEmployeeFromReader(reader);
                    if (employee != null)
                    {
                        toReturn.Add(employee);
                    }
                }
            }
            catch
            {
                return toReturn;
            }
            finally
            {
                connection.Close();
            }

            // Ensure distinct employee numbers, prioritizing non-empty usernames
            var distinctEmployees = toReturn
                .GroupBy(e => e.EmployeeNumber)
                .Select(g => g.OrderByDescending(e => !string.IsNullOrEmpty(e.Username)).First())
                .ToList();

            return await Task.FromResult(distinctEmployees);
        }

        public async Task<List<Department>> GetDepartments()
        {
            using SqlConnection connection = new(_secretsProvider.GetSysIntegrationConnectionString());

            try
            {
                Dictionary<string, Department> departmentMap = [];
                
                string deptQuery = @"SELECT [tblDepartments].[DeptCode], [DeptDesc], [BusinessUnit] FROM tblDepartments LEFT OUTER JOIN [tblBusDeptMap] ON [tblDepartments].[DeptCode] = [tblBusDeptMap].[DeptCode] ORDER BY DeptDesc";
                SqlCommand deptCommand = new(deptQuery, connection);

                connection.Open();
                using SqlDataReader deptReader = deptCommand.ExecuteReader();

                while (deptReader.Read())
                {
                    var deptCode = deptReader["DeptCode"].ToString();
                    var deptDesc = deptReader["DeptDesc"].ToString();
                    var businessUnit = deptReader["Businessunit"].ToString();

                    if (!string.IsNullOrWhiteSpace(deptCode) && !string.IsNullOrWhiteSpace(deptDesc))
                    {
                        departmentMap[deptCode] = new() { Code = deptCode, Name = deptDesc, BusinessUnit = businessUnit };
                    }
                }

                string sectionQuery = @"SELECT [SectionName], [DeptCode] FROM tblDepartmentSections";
                SqlCommand sectionCommand = new(sectionQuery, connection);

                using SqlDataReader sectionReader = sectionCommand.ExecuteReader();

                while (sectionReader.Read())
                {
                    var deptCode = sectionReader["DeptCode"].ToString();
                    var sectionName = sectionReader["SectionName"].ToString()?.Trim();

                    if (!string.IsNullOrWhiteSpace(deptCode) && !string.IsNullOrWhiteSpace(sectionName))
                    {
                        departmentMap[deptCode].Sections.Add(new() { Name = sectionName });
                    }

                }

                List<Department> toReturn = departmentMap.Keys.ToList().Select<string, Department>(k => departmentMap[k]).ToList();
                toReturn.ForEach(d => d.Sections.Sort());
                toReturn.ForEach(d =>
                {
                    if (d.Sections.Count == 0)
                    {
                        d.Sections.Add(new() { Name = "N/A" });
                    }
                });

                return await Task.FromResult(toReturn);
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<Department?> FindDepartmentByCode(string departmentCode)
        {
            using SqlConnection connection = new(_secretsProvider.GetSysIntegrationConnectionString());

            string deptQuery = @"SELECT [tblDepartments].[DeptCode], [DeptDesc], [BusinessUnit] FROM tblDepartments LEFT OUTER JOIN [tblBusDeptMap] ON [tblDepartments].[DeptCode] = [tblBusDeptMap].[DeptCode] WHERE [tblDepartments].[DeptCode] = @Code";
            SqlCommand deptCommand = new(deptQuery, connection);
            deptCommand.Parameters.AddWithValue("@Code", departmentCode);

            connection.Open();
            using SqlDataReader deptReader = deptCommand.ExecuteReader();

            Department? toReturn = null;

            if (deptReader.Read())
            {
                var deptCode = deptReader["DeptCode"].ToString();
                var deptDesc = deptReader["DeptDesc"].ToString();
                var businessUnit = deptReader["Businessunit"].ToString();

                if (!string.IsNullOrWhiteSpace(deptCode) && !string.IsNullOrWhiteSpace(deptDesc))
                {
                    toReturn = new() { Code = deptCode, Name = deptDesc, BusinessUnit = businessUnit };
                }

            }

            connection.Close();
            connection.Open();

            if (null == toReturn) { return toReturn; }

            string sectionQuery = @"SELECT [SectionName], [DeptCode] FROM tblDepartmentSections WHERE DeptCode = @Code";
            SqlCommand sectionCommand = new(sectionQuery, connection);
            sectionCommand.Parameters.AddWithValue("@Code", departmentCode);

            using SqlDataReader sectionReader = sectionCommand.ExecuteReader();

            while (sectionReader.Read())
            {
                var sectionName = sectionReader["SectionName"].ToString();
                if (!string.IsNullOrWhiteSpace(sectionName))
                {
                    toReturn.Sections.Add(new() { Name = sectionName });
                }
            }

            if (toReturn.Sections.Count == 0)
            {
                toReturn.Sections.Add(new() { Name = "N/A" });
            }

            return await Task.FromResult(toReturn);
        }

    }
}