using System.Text;
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

// Database
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AuthDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("AuthorisationConnection")));

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy,
        policy  =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",
                    "https://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    // Role based policies
    options.AddPolicy(RoleType.Developer, policy =>
        policy.RequireClaim("role", RoleType.Developer));
    options.AddPolicy(RoleType.Employee, policy =>
        policy.RequireClaim("role", RoleType.Employee, RoleType.Developer));
    options.AddPolicy(RoleType.External, policy =>
        policy.RequireClaim("role", RoleType.External, RoleType.Employee, RoleType.Developer));
    options.AddPolicy(RoleType.User, policy =>
        policy.RequireClaim("role", RoleType.User, RoleType.External, RoleType.Employee, RoleType.Developer));
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