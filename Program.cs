using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Application.Interface.IService;
using Application.Services;
using Application.Interface.IRepository;
using Infrastructure.Repositories;
using Infrastructure.Data;
using Application.Interface;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Serilog Setup -------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173") // ✅ Fix port
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});
// ------------------- Add Services -------------------
builder.Services.AddControllers();
builder.Services.AddSingleton<DapperContext>();

// Register your services and repositories
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IApplicantRepo, ApplicantRepo>();
builder.Services.AddScoped<IApplicantService, ApplicantService>();
builder.Services.AddScoped<IInterviewRepo, InterViewRepo>();
builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddScoped<IJobApplyRepo, JobApplyRepo>();
builder.Services.AddScoped<IJobApplyService, JobApplyService>();
builder.Services.AddScoped<IJobListRepo, JobListRepo>();
builder.Services.AddScoped<IJobListService, JobListService>();
builder.Services.AddScoped<IJobPostRepo, JobPostRepo>();
builder.Services.AddScoped<IJobPostService, JobPostService>();
builder.Services.AddScoped<ISelectedEmployeesRepo, SelectedEmployeesRepo>();
builder.Services.AddScoped<ISelectedEmployeesService, SelectedEmployeesService>();
builder.Services.AddScoped<ISubscriptionRepo, SubscriptionRepo>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

// ------------------- Swagger Setup -------------------
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HireIn API", Version = "v1" });

    // Cookie-based JWT
    c.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
    {
        Name = "accessToken",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Cookie,
        Description = "JWT token stored in the 'accessToken' cookie"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "cookieAuth",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });

    // Header-based JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your JWT token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ------------------- JWT Auth with Cookie Support -------------------
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

    // ✅ Extract token from cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var tokenFromCookie = context.Request.Cookies["accessToken"];
            if (!string.IsNullOrEmpty(tokenFromCookie))
            {
                context.Token = tokenFromCookie;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ------------------- Build App -------------------
var app = builder.Build();

// ------------------- Middleware -------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ Apply named CORS policy here
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ------------------- Run -------------------
try
{
    Log.Information("Starting the app...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
}
finally
{
    Log.CloseAndFlush();
}
