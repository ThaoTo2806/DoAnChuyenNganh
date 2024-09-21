using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using webapi.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework Core to use MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/Users/login-regular"; // Path to redirect when user is not logged in
        options.LogoutPath = "/api/Users/logout"; // Path to redirect when user logs out
        options.ExpireTimeSpan = TimeSpan.FromDays(30);  // Cookie expiration time
        options.SlidingExpiration = true; // Renew the cookie if the user is active
        options.Cookie.HttpOnly = true; // Prevent cookie access from JavaScript
        options.Cookie.IsEssential = true; // Cookie is essential for the application
        options.Cookie.SameSite = SameSiteMode.Lax; // Set SameSite for cookie
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MemoryDistributedCache for Session
builder.Services.AddDistributedMemoryCache(); // Add distributed cache services

// Configure Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30); // Set session expiration time
    options.Cookie.HttpOnly = true; // Prevent cookie access from JavaScript
    options.Cookie.IsEssential = true; // Cookie is essential for the application
    options.Cookie.SameSite = SameSiteMode.Lax; // Set SameSite for cookie
});

// Configure Kestrel to listen on port 5134
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5134); // Listen on all IP addresses on port 5134
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
