using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoleBasedApp.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RoleBasedAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RoleBasedAppContext") ?? throw new InvalidOperationException("Connection string 'RoleBasedAppContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Token"; // Set your desired cookie name
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set the expiration time
        options.SlidingExpiration = true; // Enable sliding expiration if needed
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
