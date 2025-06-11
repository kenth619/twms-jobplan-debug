using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TWMSServer.Model;
using Microsoft.IdentityModel.Tokens;
using TWMSServer.Model.Enum;

namespace TWMSServer.Providers
{
    public class JwtProvider(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(Employee employee, List<SystemRole> systemRoles, List<DepartmentRoleMappingEntry> departmentRoles)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? throw new Exception("JWT not configured!"));

            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "30");

            List<Claim> claims = [
                new Claim(ClaimTypes.Name, employee.FullName),
                new Claim("username", employee.Username),
                new Claim("employeeNumber", employee.EmployeeNumber)
            ];

            if (!string.IsNullOrWhiteSpace(employee.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, employee.Email));
            }

            // Add system roles
            foreach (var roleAssignment in systemRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, $"system:{roleAssignment.Key}"));
            }

            // Add department roles
            foreach (var roleAssignment in departmentRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, $"department:{roleAssignment.DepartmentCode}:{roleAssignment.Role.Key}"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}