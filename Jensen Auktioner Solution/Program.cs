
using Jensen_Auktioner_Solution.Automapper;
using Jensen_Auktioner_Solution.Data;
using Jensen_Auktioner_Solution.Repository;
using Jensen_Auktioner_Solution.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
// Retrieve the JWT secret key from configuration
string jwtSecretKey = builder.Configuration.GetSection("Jwt:SecretKey").Value;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ClockSkew = TimeSpan.Zero // You can adjust the tolerance for clock skew if needed
    };
});
// Add DapperContext
builder.Services.AddScoped<DapperContext>();

//// Add other services
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IUserService, UserRepository>();
builder.Services.AddScoped<IRoleService, RoleRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleRepository>();
builder.Services.AddScoped<IAuctionService, AuctionRepository>();
builder.Services.AddScoped<IBidService, BidRepository>();





// Configure Swagger
builder.Services.AddEndpointsApiExplorer();

// Inside the ConfigureServices method
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Jensen AuctionHouse API", Version = "v1" });

    // Add the security definition for JWT tokens
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Add the security requirement for Bearer tokens
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


// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use Authentication before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Your Role Authorization Middleware
app.UseMiddleware<RoleAuthorizationMiddleware>();

app.MapControllers();

app.Run();
