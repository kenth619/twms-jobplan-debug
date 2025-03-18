using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TemplateProject.Providers;
using TemplateProject.Providers.EmployeeProvider;
using Microsoft.IdentityModel.Tokens;

namespace TemplateProject.Middleware
{
    public class JwtMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        IEmployeeProvider employeeProvider,
        JwtProvider jwtProvider)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;
        private readonly JwtProvider _jwtProvider = jwtProvider;

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract token from Authorization header
            string? token = null;
            string? authHeader = context.Request.Headers.Authorization;
            
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authHeader.Substring("Bearer ".Length).Trim();
            }
            
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    // Validate the token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtSettings = _configuration.GetSection("JWT");
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

                    // This will throw an exception if the token is invalid
                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                    
                    // Get token expiration time
                    if (validatedToken is JwtSecurityToken jwtToken)
                    {
                        var expiration = jwtToken.ValidTo;
                        var timeRemaining = expiration - DateTime.UtcNow;
                        
                        // No need to add session timeout information to response headers
                        // as the client can extract this information directly from the JWT

                        // Check if token needs to be refreshed (less than halfway through its lifetime)
                        var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "30");
                        var refreshThreshold = TimeSpan.FromMinutes(expiryMinutes / 2.0);
                        
                        if (timeRemaining < refreshThreshold)
                        {
                            // Proactively refresh the token
                            var username = principal.FindFirst(ClaimTypes.Name)?.Value;
                            if (!string.IsNullOrEmpty(username))
                            {
                                // Get employee from username
                                var employee = await _employeeProvider.FindEmployeeByUserName(username);
                                if (employee != null)
                                {
                                    // Generate a new token using the JWT provider
                                    var newToken = await _jwtProvider.GenerateToken(employee);
                                    
                                    // Add the new token to the response headers
                                    context.Response.Headers.Append("X-New-Token", newToken);
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