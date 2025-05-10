using AuthService.DB;
using AuthService.Middleware;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ADD PostgreSQL
var config = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PostgreSQLContext>((options) => options.UseNpgsql(config));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add Controllers
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// ADD Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// ADD JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var config = builder.Configuration.GetSection("JWT");
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["Issuer"],
        ValidAudience = config["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Key"])),
        ValidateAudience = false,
        ValidateIssuerSigningKey = false,
        ValidateIssuer = false,
        ValidateLifetime = false,
    };
});

// ADD Mappers
builder.Services.AddAutoMapper(typeof(UserServiceMapper));

// ADD Domain Services
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<AuthentificationService>();

// Start App
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AuthMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers().RequireAuthorization();

app.Run();
