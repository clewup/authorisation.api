using System.Text;
using authorisation.api.Managers;
using authorisation.api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var DefaultCorsPolicy = "DefaultCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: DefaultCorsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                    "https://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Managers
builder.Services.AddSingleton<UserManager>();
builder.Services.AddSingleton<LoginManager>();

// Services
builder.Services.AddSingleton<PasswordHasher>();

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

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DefaultCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();