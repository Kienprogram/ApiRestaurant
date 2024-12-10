using api_restaurant.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add PostgreSQL and Identity
builder.Services.AddDbContext<PosContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]); // Add JWT key to appsettings.json

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "auth_token";
    options.LoginPath = "/api/Employees/login";
    options.Cookie.HttpOnly = true; // Helps mitigate XSS attacks
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure cookies are sent only over HTTPS
    options.Cookie.MaxAge = TimeSpan.FromMinutes(30); // Set cookie expiration to 30 minutes
    options.SlidingExpiration = true; // Extend the expiration time with each request
    options.AccessDeniedPath = "/acce   ss-denied"; // Path for unauthorized access
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder =>
        {

            policyBuilder.WithOrigins("http://119.59.117.131:7081", "http://www.excvrstgogood.online:8080", "http://www.excvrstgogood.online", "https://localhost:7081")
                 //policyBuilder.WithOrigins("https://localhost:7081") // Frontend origin
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials(); // Important for cookies;

        });
});

// Add services to the container.
builder.Services.AddControllers();

// Configure file upload size limit
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
