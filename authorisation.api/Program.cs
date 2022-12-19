using System.Text;
using System.Text.RegularExpressions;
using authorisation.api.Data;
using authorisation.api.Infrastructure;
using authorisation.api.Managers;
using authorisation.api.Services;
using authorisation.api.Services.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var  CorsPolicy = "_corsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var isDevelopment = builder.Environment.IsDevelopment();

if (isDevelopment)
{
    builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
}

// Database
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

if (isDevelopment)
    connectionString = builder.Configuration.GetConnectionString("AuthorisationConnection");

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AuthDbContext>(options =>
{
    var m = Regex.Match(connectionString, @"postgres://(.*):(.*)@(.*):(.*)/(.*)");
    options.UseNpgsql(
        $"Server={m.Groups[3]};Port={m.Groups[4]};User Id={m.Groups[1]};Password={m.Groups[2]};Database={m.Groups[5]};sslmode=Prefer;Trust Server Certificate=true");
});

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy,
        policy  =>
        {
            policy.WithOrigins(
                    "http://ecommerce.clewup.co.uk",
                    "https://ecommerce.clewup.co.uk")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

if (isDevelopment)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: CorsPolicy,
            policy =>
            {
                policy.WithOrigins(
                        "http://localhost:3000",
                        "https://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
}

// Jwt
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

if (isDevelopment)
{
    jwtIssuer = builder.Configuration["Jwt:Issuer"];
    jwtAudience = builder.Configuration["Jwt:Audience"];
    jwtKey = builder.Configuration["Jwt:Key"];
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddAuthorization(options =>
{
    // Role based policies
    options.AddPolicy(RoleType.Developer, policy =>
        policy.RequireRole(RoleType.Developer));
    options.AddPolicy(RoleType.Employee, policy =>
        policy.RequireRole(RoleType.Employee, RoleType.Developer));
    options.AddPolicy(RoleType.External, policy =>
        policy.RequireRole(RoleType.External, RoleType.Employee, RoleType.Developer));
    options.AddPolicy(RoleType.User, policy =>
        policy.RequireRole(RoleType.User, RoleType.External, RoleType.Employee, RoleType.Developer));
});

// Auto Mapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new UserMapper());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Managers
builder.Services.AddTransient<UserManager>();
builder.Services.AddTransient<UserDataManager>();
builder.Services.AddTransient<LoginManager>();

// Services
builder.Services.AddTransient<PasswordHasher>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(CorsPolicy);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();