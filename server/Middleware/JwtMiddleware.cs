using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TWMSServer.Providers;
using TWMSServer.Providers.EmployeeProvider;
using Microsoft.IdentityModel.Tokens;
using TWMSServer.Model.Enum;
using System.Security.Claims;
using TWMSServer.Model;

namespace TWMSServer.Middleware
{
    public class JwtMiddleware(ILogger<JwtMiddleware> logger, RequestDelegate next)
    {
        private readonly ILogger<JwtMiddleware> _logger = logger;
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, IEmployeeProvider employeeProvider, JwtProvider jwtProvider)
        {
            // Extract token from Authorization header
            string? token = null;
            string? authHeader = context.Request.Headers.Authorization;
            
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authHeader["Bearer ".Length..].Trim();
            }
            
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    // Validate the token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtSettings = configuration.GetSection("JWT");
                    var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? throw new Exception("JWT not configured!"));

                    // Validate token and get expiration time
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtSettings["Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                    
                    if (validatedToken is JwtSecurityToken jwtToken)
                    {
                        var expiration = jwtToken.ValidTo;
                        var timeRemaining = expiration - DateTime.UtcNow;
                        
                        var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "30");
                        var refreshThreshold = TimeSpan.FromMinutes(expiryMinutes / 2.0);
                        
                        if (timeRemaining < refreshThreshold)
                        {
                            var username = principal.FindFirst("username")?.Value;
                            if (!string.IsNullOrEmpty(username))
                            {
                                var employee = await employeeProvider.FindEmployeeByUserName(username);
                                if (employee != null)
                                {
                                    // Extract system and department roles from the current token
                                    var systemRoles = new List<SystemRole>();
                                    var departmentRoles = new List<DepartmentRoleMappingEntry>();
                                    
                                    foreach (var claim in principal.Claims.Where(c => c.Type == ClaimTypes.Role))
                                    {
                                        if (claim.Value.StartsWith("system:"))
                                        {
                                            var roleKey = claim.Value.Substring("system:".Length);
                                            systemRoles.Add(SystemRole.FromKey(roleKey));
                                        }
                                        else if (claim.Value.StartsWith("department:"))
                                        {
                                            var parts = claim.Value.Substring("department:".Length).Split(':');
                                            if (parts.Length == 2)
                                            {
                                                var departmentCode = parts[0];
                                                var roleKey = parts[1];
                                                departmentRoles.Add(new DepartmentRoleMappingEntry(departmentCode, DepartmentRole.FromKey(roleKey)));
                                            }
                                        }
                                    }
                                    
                                    // Use the overloaded method that accepts roles
                                    var newToken = jwtProvider.GenerateToken(employee, systemRoles, departmentRoles);
                                    context.Response.Headers.Append("X-New-Token", newToken);
                                    _logger.LogInformation("Sending new refresh token for employee with number {EmployeeNumber}", employee.EmployeeNumber);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // Don't throw exception, just log it and continue
                    // The authentication middleware will handle unauthorized requests
                    // var logger = context.RequestServices.GetService<ILogger<JwtMiddleware>>();
                    // logger?.LogWarning(ex, "JWT validation failed");
                }
            }

            // Continue processing the request
            await _next(context);
        }
    }

    // Extension method to add the middleware to the HTTP request pipeline
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}