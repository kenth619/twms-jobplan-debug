using TemplateProject;
using TemplateProject.Middleware;
using TemplateProject.Providers;
using TemplateProject.Providers.AuthProvider;
using TemplateProject.Providers.EmployeeProvider;
using TemplateProject.Providers.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.AspNetCore;
using Serilog;
using System.Text;

Serilog.Debugging.SelfLog.Enable(Console.Error);

Log.Logger = new LoggerConfiguration()
   .WriteTo.Console()
   .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Host.UseSerilog((context, services, configuration) => {
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
   }
);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IAuthProvider, MockAuthProvider>();
}
else
{
    builder.Services.AddTransient<IAuthProvider, LDAPAuthProvider>();
}

builder.Services.AddSingleton<ISecretsProvider, FileSecretsProvider>();

ISecretsProvider secretsProvider = new FileSecretsProvider(builder.Configuration);
builder.Services.AddDbContextFactory<TemplateProjectContext>(options => options.UseSqlServer(secretsProvider.GetTemplateProjectConnectionString()));

builder.Services.AddTransient<IEmployeeProvider, EmployeeProvider>();
builder.Services.AddTransient<EmployeeRolesProvider>();
builder.Services.AddTransient<JwtProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JWT");
    var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? throw new Exception("JWT Secret not configured!"));
    
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
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
});

var corsSettings = builder.Configuration.GetSection("CORS");
var origins = corsSettings.GetSection("Origins").Get<string[]>() ?? ["http://localhost:3000"];
var headers = corsSettings.GetSection("Headers").Get<string[]>() ?? ["*"];
string[] exposedHeaders = ["x-new-token"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(origins)
              .WithHeaders(headers)
              .WithExposedHeaders(exposedHeaders)
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Template Project API",
        Version = "v1",
        Description = "API for Template Project system with JWT authentication"
    });

    options.AddSecurityDefinition("jwt_auth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
    });
    
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "jwt_auth"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddQuartz(q => {});

builder.Services.AddQuartzServer(opt =>
{
    opt.StartDelay = TimeSpan.FromSeconds(5);
    opt.WaitForJobsToComplete = true;
});

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseJwtMiddleware();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();
