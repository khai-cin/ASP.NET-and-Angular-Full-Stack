using System.Text;
using API.Data;
using API.Interface;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();
// Dependency Injection for Token Service
// AddScoped means a new instance is created per client request, but reused within that request
builder.Services.AddScoped<ITokenService, TokenService>();

// JWT Authentication Configuration
// This configures the app to use JWT Bearer tokens for authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Token validation parameters
        // Looks for token key in the set configuration
        var tokenKey = builder.Configuration["TokenKey"]
            ?? throw new Exception("Token key not found");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Validates the signing key to ensure token integrity
            ValidateIssuerSigningKey = true,
            // Sets the signing key using the token key from configuration
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            // ValidateIssuer defines whether the issuer should be validated 
            // where the issuer is the entity that issued the token
            ValidateIssuer = false,
            // ValidateAudience defines whether the audience should be validated 
            // where the audience is the intended recipient of the token
            ValidateAudience = false
        };
    });


var app = builder.Build();



// app.UseHttpsRedirection();

// app.UseAuthorization();

// Configure HTTP resquest pipeline
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
